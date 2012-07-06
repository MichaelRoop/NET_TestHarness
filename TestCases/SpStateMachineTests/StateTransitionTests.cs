using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.SuperStates;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Converters;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.SpStateMachineTests.TestImplementations.SuperStates.CascadeOnExit;
using System.Threading;
using LogUtils;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class StateTransitionTests {

        #region Data

        HelperLogReader logReader = new HelperLogReader();

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

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState notStartedSs = new NotStartedSs(null, dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, notStartedSs);

                this.TickAndValidateState(new MyTickMsg(), sm, "NotStarted.Idle");
                this.TickAndValidateState(new MyTickMsg(), sm, "NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyEventType.Start), sm, "NotStarted.Active");
                this.TickAndValidateState(this.GetMsg(MyEventType.Abort), sm, "NotStarted.Idle");
            });
        }


        [Test]
        public void TestExitStateTransitionsInSuperState() {

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState mainSs = new MainSs(dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, mainSs);

                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted");
                this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyEventType.Start), sm, "Main.NotStarted.Active");
                this.TickAndValidateState(this.GetMsg(MyEventType.Stop), sm, "Main.NotStarted.Idle");
                this.TickAndValidateState(this.GetMsg(MyEventType.Abort), sm, "Main.Recovery.Idle");
            });
        }


        [Test]
        public void TestExitStateCascadeInSuperState() {

            TestHelpers.CatchUnexpected(() => {

                // Setting flip count will cause back and fourth between active and idle
                MyDataClass dataClass = new MyDataClass();
                MySuperState mainSs = new LevelMainSs(dataClass);
                ISpStateMachine sm = new MyStateMachine(dataClass, mainSs);

                this.ValidateState(sm, "Main.Level2.Level3.Idle");
                this.TickAndValidateState(new MyTickMsg(), sm, "Main.Level2.Level3.Idle");
                this.TickAndValidateState(this.GetMsg(MyEventType.Start), sm, "Main.Level2.Level3.Active");

                Console.WriteLine("**********************************");
                //this.Tick(this.GetMsg(MyEventType.Abort), sm);
                this.TickAndValidateState(this.GetMsg(MyEventType.Abort), sm, "Main.Recovery.Idle");
                Console.WriteLine("**********************************");

                //this.Tick(new MyTickMsg(), sm);
                //Console.WriteLine("**********************************");

                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.Recovery");
                //sm.Tick(new MyTickMsg());


                //this.TickAndValidateState(new MyTickMsg(), sm, "Main.NotStarted.Idle");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Start), sm, "Main.NotStarted.Active");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Stop), sm, "Main.NotStarted.Idle");
                //this.TickAndValidateState(this.GetMsg(MyEventType.Abort), sm, "Main.Recovery.Idle");
            });
        }



        #region Private Methods

        private ISpEventMessage GetMsg(MyEventType eventId) {
            Console.WriteLine("-- Sending msg:{0}", eventId.ToString());
            return new MyBaseMsg(MyMsgType.SimpleMsg, eventId);
        }


        private ISpEventMessage GetStartMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start);
        }

        private ISpEventMessage GetStopMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop);
        }

        private ISpEventMessage GetAbortMsg() {
            return new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Abort);
        }


        private void Tick(ISpEventMessage msg, ISpStateMachine sm) {
            TestHelpers.CatchUnexpected(() => {
                sm.Tick(msg);
            });
        }



        private void TickAndValidateState(ISpEventMessage msg, ISpStateMachine sm, string expected) {
            this.Tick(msg, sm);
            Thread.Sleep(0);
            this.ValidateState(sm, expected);
        }


        private void ValidateState(ISpStateMachine sm, string expected) {
            Console.WriteLine("-- Validate that state is:{0}", expected);
            //Console.WriteLine("-- Validate that state is:{0} - Current State Name is: {1}", expected, sm.CurrentStateName);
            Assert.AreEqual(expected, sm.CurrentStateName);
        }

        #endregion

    }

}
