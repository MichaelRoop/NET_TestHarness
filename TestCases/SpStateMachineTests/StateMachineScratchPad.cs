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
using TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS;

namespace TestCases.SpStateMachineTests {

    /// <summary>
    /// To drive development of framework
    /// </summary>
    [TestFixture, Explicit]
    public class StateMachineScratchPad {

        #region Setup 

        NUnitTraceWriter consoleWriter = new NUnitTraceWriter();
        SpTestHelpers helpers = new SpTestHelpers();
        ClassLog log = new ClassLog("StateMachineScratchPad");

        [OneTimeSetUp]
        public void Setup() {
            Log.SetVerbosity(MsgLevel.Info);
            Log.SetMsgNumberThreshold(1);
            Log.SetStackTools(new StackTools());
            WrapErr.InitialiseOnExceptionLogDelegate(Log.LogExceptionDelegate);

            this.consoleWriter.StartLogging();
        }

        [OneTimeTearDown]
        public void Teardown() {
            Thread.Sleep(300);
            this.consoleWriter.StopLogging();
        }

        #endregion

        //NUnit 3 seems to ignore the Explicit so will just comment them out until needed


                

        public class EnumTestClass {
            public Enum TypeId { get; set; }
            public Enum MsgId { get; set; }
        }


        [Test]
        public void DoEnumParamTest() {
            EnumTestClass tc = new EnumTestClass() {
                TypeId = MyMsgType.DataStrMsg,
                MsgId = MyMsgId.ExitAborted,
            };
            Thread.Sleep(500);


            this.log.Info("DoEnumParamTest", () => string.Format("Msg type {0} Msg Id {1}", 
                (MyMsgType)tc.TypeId, (MyMsgId)tc.MsgId));

            //string t = tc.TypeId.ToString();
            //string i = tc.MsgId.ToString();

            ////this.log.Info("DoEnumParamTest", () => string.Format("Msg type {0} Msg Id {1}",
            ////    t, i));

            ////this.log.Info("sdfsd", "Hi there");

            //Log.Warning(1233, "Log warning");

        }




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

                s.ToNextOnEvent(MyMsgId.Stop, s2);

                ISpStateMachine sm = new MyStateMachine(dataClass, s);
                ISpEventStore store = new SimpleDequeEventStore(new MyTickMsg());
                ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
                ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 1000));
                ISpEventListner listner = new SimpleEventListner();

                listner.ResponseReceived += new EventHandler((o,e) => { });



                // TODO - Need a default response msg

                // Simulates DI
                SpStateMachineEngine engine =
                new SpStateMachineEngine(listner, store, behavior, sm, timer);

                engine.Start();

                Thread.Sleep(3000);

                //sm.Tick(new BaseMsg(99, 456));

                //listner.PostMessage(new SpBaseMsg(MyMessageType 777, 12345));

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));

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
                listner.MsgReceived += this.helpers.ListnerMsgDumpDelegate;
                listner.ResponseReceived += this.helpers.ListnerResponseDumpDelegate;

                engine.Start();
                Thread.Sleep(600);

                Assert.AreEqual("NotStarted.Idle", notStartedSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start));
                Thread.Sleep(700);
                Assert.AreEqual("NotStarted.Active", notStartedSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Abort));
                Thread.Sleep(700);
                Assert.AreEqual("NotStarted.Idle", notStartedSs.CurrentStateName);

                listner.MsgReceived -= this.helpers.ListnerMsgDumpDelegate;
                listner.ResponseReceived -= this.helpers.ListnerResponseDumpDelegate;


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
                Assert.AreEqual("Main.NotStarted.Idle", mainSs.CurrentStateName);
                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start));
                Thread.Sleep(600);

                Assert.AreEqual("Main.NotStarted.Active", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                Thread.Sleep(600);
                Assert.AreEqual("Main.NotStarted.Idle", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start));
                Thread.Sleep(600);
                Assert.AreEqual("Main.NotStarted.Active", mainSs.CurrentStateName);

                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                Thread.Sleep(800);
                Assert.AreEqual("Main.NotStarted.Idle", mainSs.CurrentStateName);

                // Should be back to Main.NotStarted.Idle by now - it has a ExitState transition registered to that state
                Console.WriteLine("Sending the Abort event to provoke a ExitState transition");
                listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Abort));
                Thread.Sleep(800);
                Assert.AreEqual("Main.Recovery.Idle", mainSs.CurrentStateName);


                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }


        private void DoTick(ISpEventListner listner, ISpState<MyMsgId> st, string expected, bool assertOnFail = true) {
            listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Tick));
            Thread.Sleep(600);
            //Log.Warning(9, () => string.Format("State:{0}", st.CurrentStateName));
            Log.Warning(0, () => string.Format(" -*-*- Current state: {0} -*-*-", st.CurrentStateName));
            if (assertOnFail) {
                Assert.AreEqual(expected, st.CurrentStateName);
            }

        }

        [Test, Explicit]
        public void TestResultExitTickStateTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {
                MyDataClass dataClass = new MyDataClass();
                SS_M m = new SS_M(dataClass);
                ISpEventListner listner;
                SpStateMachineEngine engine = this.GetEngine(out listner, dataClass, m);

                listner.MsgReceived += this.helpers.ListnerMsgDumpDelegate;
                listner.ResponseReceived += this.helpers.ListnerResponseDumpDelegate;

                engine.Start();

                this.DoTick(listner, m, "SS_M.SS_A1.SS_A1");
                this.DoTick(listner, m, "SS_M.SS_A1.SS_A1");
                this.DoTick(listner, m, "SS_M.SS_A1.SS_A1");
                // After third it should have transitioned and stays the same
                this.DoTick(listner, m, "SS_M.SS_B1.S_B1");
                this.DoTick(listner, m, "SS_M.SS_B1.S_B1");
                this.DoTick(listner, m, "SS_M.SS_B1.S_B1");

                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }


        [Test, Explicit]
        public void TestResultExitEntryStateTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {
                MyDataClass dataClass = new MyDataClass();
                SS_M2 m = new SS_M2(dataClass);
                ISpEventListner listner;
                SpStateMachineEngine engine = this.GetEngine(out listner, dataClass, m);

                engine.Start();
                // First tick will drive cascade of state changes because the first SS aborts on entry of first sub state entry
                this.DoTick(listner, m, "SS_M2.SS_B1.S_B1", true);

                Thread.Sleep(1000);
                Log.Warning(0, () => string.Format(" -*-*- Current state: {0} -*-*-", m.CurrentStateName));
                Thread.Sleep(1000);
                engine.Stop();
                engine.Dispose();
                Console.WriteLine("Engine Disposed");
            });
        }

        private SpStateMachineEngine GetEngine(out ISpEventListner listner, MyDataClass dataClass, ISpState<MyMsgId> firstState) {

            ISpStateMachine sm = new MyStateMachine(dataClass, firstState);
            ISpEventStore store = new SimpleDequeEventStore(new MyTickMsg());
            ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
            ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 500));
            //ISpEventListner listner = new SimpleEventListner();
            listner = new SimpleEventListner();

            // To avoid log errors if no subscribers
            listner.ResponseReceived += new EventHandler((o,e) => { });

            // Simulates DI
            return new SpStateMachineEngine(listner, store, behavior, sm, timer);
        }


    }
}
