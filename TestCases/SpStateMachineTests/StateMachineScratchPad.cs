using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LogUtils;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using SpStateMachine.EventListners;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using SpStateMachine.EventStores;
using SpStateMachine.PeriodicTimers;
using System.Threading;
using SpStateMachine.Behaviours;
using ChkUtils;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests {

    /// <summary>
    /// To drive development of framework
    /// </summary>
    [TestFixture]
    public class StateMachineScratchPad {

        #region Setup 

        ConsoleWriter consoleWriter = new ConsoleWriter();

        [TestFixtureSetUp]
        public void Setup() {
            Log.SetVerbosity(MsgLevel.Info);

            Log.SetMsgNumberThreshold(1);
            this.consoleWriter.StartLogging();
        }

        [TestFixtureTearDown]
        public void Teardown() {
            this.consoleWriter.StopLogging();
        }

        #endregion


        [Test, Explicit]
        public void TestInitialGenericSpState() {

            TestHelpers.CatchUnexpected(() => {

                MyDataClass dataClass = new MyDataClass();
                // This would normally be the main superstate which includes all other states cascading within it's and it's children's constructors

                ISpState sParent = new MyState(MyStateID.NotStarted, dataClass);
                ISpState s = new MyState(sParent, MyStateID.WaitingForUserInput, dataClass);
                ISpState s2 = new MyState(sParent, MyStateID.Active, dataClass);


                Console.WriteLine("SuperState sParent name:{0}", sParent.FullName);
                Console.WriteLine("State s name:{0}", s.FullName);
                Console.WriteLine("State s2 name:{0}", s2.FullName);


                //s.RegisterOnEventTransition(12345, new SpStateTransition(SpStateTransitionType.NextState, s2, null));
                s.RegisterOnEventTransition(MyEventType.Stop.Int(), new SpStateTransition(SpStateTransitionType.NextState, s2, null));


                ISpStateMachine sm = new MyStateMachine(dataClass, s);
                ISpEventStore store = new SimpleDequeEventStore(new MyTickMsg());
                ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
                ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 1000));
                ISpEventListner listner = new SimpleEventListner();

                listner.ResponseReceived += new Action<ISpMessage>((msg) => { });



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



    }
}
