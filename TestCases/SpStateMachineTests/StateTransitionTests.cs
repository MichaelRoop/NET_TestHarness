using LogUtils;
using NUnit.Framework;
using SpStateMachine.Interfaces;
using System;
using System.Threading;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.SpStateMachineTests.TestImplementations.SuperStates;
using TestCases.SpStateMachineTests.TestImplementations.SuperStates.CascadeOnExit;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class StateTransitionTests {

        #region Data

        HelperLogReaderNet logReader = new HelperLogReaderNet();

        #endregion

        #region Setup

        [SetUp]
        public void TestSetup() {
            this.logReader.StartLogging();
        }

        [TearDown]
        public void TestTeardown() {
            Thread.Sleep(200);
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion


        [Test]
        public void TestDeferedTransitionsInSuperState() {

            TestHelpersNet.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState notStartedSs = new NotStartedSs(null, dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, notStartedSs);

                //this.TickAndValidateState(new MyTickMsg(), sm, "NotStarted.Idle");
                //this.TickAndValidateState(new MyTickMsg(), sm, "NotStarted.Idle");

                this.TickAndValidateState(this.GetMsg(MyMsgId.Tick), sm, "NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Start), sm, "NotStarted.Active");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Abort), sm, "NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Tick), sm, "NotStarted.Idle");
            });
        }


        [Test]
        public void TestExitStateTransitionsInSuperState() {

            TestHelpersNet.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState mainSs = new MainSs(dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, mainSs);

                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted");
                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Tick), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Start), sm, "Main.NotStarted.Active");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Stop), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Abort), sm, "Main.Recovery.Idle");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Tick), sm, "Main.Recovery.Idle");

                //Thread.Sleep(500);
            });
        }


        [Test]
        public void TestExitStateCascadeInSuperState() {

            //TestHelpersNet.CatchUnexpected(() => {

            // Setting flip count will cause back and fourth between active and idle
            MyDataClass dataClass = new MyDataClass();
                MySuperState mainSs = new LevelMainSs(dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, mainSs);

                this.ValidateState(sm, "Main.Level2.Level3.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Tick), sm, "Main.Level2.Level3.Idle");
                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.Level2.Level3.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Start), sm, "Main.Level2.Level3.Active");

                Console.WriteLine("**********************************");
                //this.Tick(this.GetMsg(MyEventType.Abort), sm);
                this.TickAndValidateState(this.GetMsg(MyMsgId.Abort), sm, "Main.Recovery.Idle");
                this.TickAndValidateState(this.GetMsg(MyMsgId.Tick), sm, "Main.Recovery.Idle");



                //Console.WriteLine("**********************************");

                //this.Tick(new MyTickMsg(), sm);
                //Console.WriteLine("**********************************");

                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.Recovery");
                //sm.Tick(new MyTickMsg());


                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted.Idle");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Start), sm, "Main.NotStarted.Active");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Stop), sm, "Main.NotStarted.Idle");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Abort), sm, "Main.Recovery.Idle");
            //});
        }



        #region Private Methods

        private ISpEventMessage GetMsg(MyMsgId eventId) {
            //Console.WriteLine("-- Sending msg:{0}", eventId.ToString());

            Log.Info("","", String.Format("---------------------- Sending msg:{0}", eventId));
            return new MyBaseMsg(MyMsgType.SimpleMsg, eventId);
        }


        private ISpEventMessage GetStartMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start);
        }

        private ISpEventMessage GetStopMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop);
        }

        private ISpEventMessage GetAbortMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Abort);
        }


        private ISpEventMessage Tick(ISpEventMessage msg, ISpStateMachine sm) {
            ISpEventMessage ret = null;
            TestHelpersNet.CatchUnexpected(() => {
                ret = sm.Tick(msg);
            });
            return ret;
        }



        private void TickAndValidateState(ISpEventMessage msg, ISpStateMachine sm, string expected) {
            ISpEventMessage ret = this.Tick(msg, sm);
            Thread.Sleep(0);
            this.ValidateState(sm, expected);
            this.ValidateReturn(msg, ret);
        }

        private void ValidateReturn(ISpEventMessage msg, ISpEventMessage ret) {
            Assert.AreEqual(msg.Uid, ret.Uid, "Mismatch in GUIDs on return");
        }


        private void ValidateState(ISpStateMachine sm, string expected) {
            Log.Info("", "", String.Format("---------------------- Validate that state is:{0}", expected));

            //Console.WriteLine("-- Validate that state is:{0}", expected);
            //Console.WriteLine("-- Validate that state is:{0} - Current State Name is: {1}", expected, sm.CurrentStateName);
            Assert.AreEqual(expected, sm.CurrentStateName);
        }

        #endregion

    }

}
