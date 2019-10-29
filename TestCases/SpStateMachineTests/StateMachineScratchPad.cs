using ChkUtils.Net;
using LogUtils.Net;
using NUnit.Framework;
using SpStateMachine.Behaviours;
using SpStateMachine.Core;
using SpStateMachine.EventListners;
using SpStateMachine.EventStores;
using SpStateMachine.Interfaces;
using SpStateMachine.PeriodicTimers;
using System;
using System.Threading;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.SpStateMachineTests.TestImplementations.SuperStates;

namespace TestCases.SpStateMachineTests {

    /// <summary>
    /// To drive development of framework
    /// </summary>
    [TestFixture, Explicit]
    public class StateMachineScratchPad {

        #region Setup 

        NUnitTraceWriter consoleWriter = new NUnitTraceWriter();

        [OneTimeSetUp]
        public void Setup() {
            Log.SetVerbosity(MsgLevel.Info);
            Log.SetMsgNumberThreshold(1);

            WrapErr.InitialiseOnExceptionLogDelegate(Log.LogExceptionDelegate);

            this.consoleWriter.StartLogging();
        }

        [OneTimeTearDown]
        public void Teardown() {
            this.consoleWriter.StopLogging();
        }

        #endregion

        //NUnit 3 seems to ignore the Explicit so will just comment them out until needed


       [Test, Explicit]
        public void TestInitialGenericSpState() {

            TestHelpers.CatchUnexpected(() => {

                MyDataClass dataClass = new MyDataClass();
                // This would normally be the main superstate which includes all other states cascading within it's and it's children's constructors

                MyState sParent = new MyState(MyStateID.NotStarted, dataClass);
                MyState s = new MyState(sParent, MyStateID.WaitingForUserInput, dataClass);
                MyState s2 = new MyState(sParent, MyStateID.Active, dataClass);


                Console.WriteLine("SuperState sParent name:{0}", sParent.FullName);
                Console.WriteLine("State s name:{0}", s.FullName);
                Console.WriteLine("State s2 name:{0}", s2.FullName);

                s.ToNextOnEvent(MyEventType.Stop, s2);

                ISpStateMachine sm = new MyStateMachine(dataClass, s);
                ISpEventStore store = new SimpleDequeEventStore(new MyTickMsg());
                ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
                ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 1000));
                ISpEventListner listner = new SimpleEventListner();

                listner.ResponseReceived += new Action<ISpEventMessage>((msg) => { });



                // TODO - Need a default response msg

                // Simulates DI
                SpStateMachineEngine engine =
                new SpStateMachineEngine(listner, store, behavior, sm, timer);

                engine.Start();

                Thread.Sleep(3000);

                //sm.Tick(new BaseMsg(99, 456));

                //listner.PostMessage(new SpBaseMsg(MyMessageType 777, 12345));

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop));

                Thread.Sleep(3000);
                engine.Stop();

                Thread.Sleep(3000);

                Console.WriteLine("Disposing Engine - thread should not output while I wait 3 seconds");
                engine.Dispose();

                Thread.Sleep(3000);
                Console.WriteLine("Done");

                // Multi call test
                engine.Dispose();
                engine.Dispose();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");

                //SpState<DataClass> state = new SpState<DataClass>(dataClass);
                //SpStateMachine<DataClass> stateMachine = new SpStateMachine<DataClass>(dataClass, state);

            });
        }


        //[Test, Explicit("experimental")]
        public void TestEventTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass() { FlipStateCount = 2 };
                MySuperState notStartedSs = new NotStartedSs(null, dataClass);
                ISpEventListner listner;
                SpStateMachineEngine engine = this.GetEngine(out listner, dataClass, notStartedSs);

                engine.Start();
                Thread.Sleep(2000);

                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }


        [Test, Explicit]
        public void TestDeferedTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState notStartedSs = new NotStartedSs(null, dataClass);
                ISpEventListner listner;
                SpStateMachineEngine engine = this.GetEngine(out listner, dataClass, notStartedSs);

                engine.Start();
                Thread.Sleep(600);

                ////listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Tick));


                Assert.AreEqual("NotStarted.Idle", notStartedSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start));
                Thread.Sleep(700);
                Assert.AreEqual("NotStarted.Active", notStartedSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Abort));
                Thread.Sleep(700);
                Assert.AreEqual("NotStarted.Idle", notStartedSs.CurrentStateName);

                Thread.Sleep(200);
                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }


        [Test, Explicit]
        public void TestExitStateTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();


                MySuperState mainSs = new MainSs(dataClass);
                ISpEventListner listner;
                SpStateMachineEngine engine = this.GetEngine(out listner, dataClass, mainSs);

                engine.Start();

                // Just move the inner states around
                Thread.Sleep(600);
                //Assert.AreEqual("Main.NotStarted.Idle", mainSs.FullName);
                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start));
                Thread.Sleep(600);

                Assert.AreEqual("Main.NotStarted.Active", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop));
                Thread.Sleep(600);
                Assert.AreEqual("Main.NotStarted.Idle", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start));
                Thread.Sleep(600);
                Assert.AreEqual("Main.NotStarted.Active", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop));
                Thread.Sleep(800);
                Assert.AreEqual("Main.NotStarted.Idle", mainSs.CurrentStateName);

                // Should be back to Main.NotStarted.Idle by now - it has a ExitState transition registered to that state
                Console.WriteLine("Sending the Abort event to provoke a ExitState transition");
                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Abort));
                Thread.Sleep(800);
                Assert.AreEqual("Main.Recovery.Idle", mainSs.CurrentStateName);


                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }




        private SpStateMachineEngine GetEngine(out ISpEventListner listner, MyDataClass dataClass, ISpState<MyEventType> firstState) {

            ISpStateMachine sm = new MyStateMachine(dataClass, firstState);
            ISpEventStore store = new SimpleDequeEventStore(new MyTickMsg());
            ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
            ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 500));
            //ISpEventListner listner = new SimpleEventListner();
            listner = new SimpleEventListner();

            listner.ResponseReceived += new Action<ISpEventMessage>((msg) => { });

            // Simulates DI
            return new SpStateMachineEngine(listner, store, behavior, sm, timer);
        }


    }
}
