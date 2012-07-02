using System;
using System.Threading;
using NUnit.Framework;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using TestCases.TestToolSet;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SimpleEventListnerTests {
        
        #region Data

        HelperLogReader logReader = new HelperLogReader();
        private ISpEventListner listner = null;

        #endregion

        #region Setup

        [SetUp]
        public void TestSetup() {
            this.logReader.StartLogging();
            this.listner = new SimpleEventListner();
        }

        [TearDown]
        public void TestTeardown() {
            this.logReader.StopLogging();
            this.logReader.Clear();
            this.listner.Dispose();
            this.listner = null;
        }

        #endregion

        #region PostMessage
            
        [Test]
        public void _50032_PostMessage_Disposed() {
            TestHelpers.CatchExpected(50032, "SimpleEventListner", "PostMessage", "Attempting to use Disposed Object", () => {
                this.listner.Dispose();
                this.listner.PostMessage(new SpBaseMsg(25, 100));
            });
        }

        #endregion

        #region PostResponse

        [Test]
        public void _50033_PostResponse_Disposed() {
            TestHelpers.CatchExpected(50033, "SimpleEventListner", "PostResponse", "Attempting to use Disposed Object", () => {
                this.listner.Dispose();
                this.listner.PostResponse(new SpBaseMsg(25, 100));
            });
        }

        #endregion

        #region MessageReceived


        [Test]
        public void _0_MessageReceived_validMsg() {
            bool received = false;
            ISpMessage msgCopy = null;

            TestHelpers.CatchUnexpected(() => {
                this.listner.MsgReceived += new Action<ISpMessage>((msg) => {
                    Console.WriteLine("Woke up on msg received");
                    received = true;
                    msgCopy = msg;
                });
                this.listner.PostMessage(new SpBaseMsg(25, 100));
            });
            // On thread pool so have to wait for response
            Thread.Sleep(200);
            Assert.IsTrue(received, "The received event was not raised");
            Assert.IsNotNull(msgCopy, "Message was not copied");
            Assert.AreEqual(25, msgCopy.TypeId);
            Assert.AreEqual(100, msgCopy.EventId);
        }

        #endregion

        #region ResponseReceived

        [Test]
        public void _0_ResponseReceived_validMsg() {
            bool received = false;
            ISpMessage msgCopy = null;

            TestHelpers.CatchUnexpected(() => {
                this.listner.ResponseReceived += new Action<ISpMessage>((msg) => {
                    Console.WriteLine("Woke up on response received");
                    received = true;
                    msgCopy = msg;
                });
                this.listner.PostResponse(new SpBaseResponse(2, new SpBaseMsg(1, 58)));
            });
            // On thread pool so have to wait for response
            Thread.Sleep(200);
            Assert.IsTrue(received, "The received event was not raised");
            Assert.IsNotNull(msgCopy, "Message was not copied");
            Assert.AreEqual(2, msgCopy.TypeId);
            Assert.AreEqual(58, msgCopy.EventId);
        }


        [Test]
        public void _50031_RaiseEvent_ResponseNoSubscribers() {
            TestHelpers.CatchUnexpected(() => {
                this.listner.PostResponse(new SpBaseResponse(2, new SpBaseMsg(1, 1)));
            });
            Thread.Sleep(250);
            this.logReader.Validate(50031, "SimpleEventListner", "RaiseEvent", "No subscribers to 'Response' message");
        }


        [Test]
        public void _50031_MessageReceived_NoSubscribers() {
            TestHelpers.CatchUnexpected(() => {
                this.listner.PostMessage(new SpBaseMsg(1, 1));
            });
            Thread.Sleep(250);
            this.logReader.Validate(50031, "SimpleEventListner", "RaiseEvent", "No subscribers to 'Message' message");
        }


        [Test]
        public void _50030_RaiseEvent_CatchUserResponseDelegateException() {

            TestHelpers.CatchUnexpected(() => {
                this.listner.ResponseReceived += new Action<ISpMessage>((msg) => {
                    Console.WriteLine("** Response Received triggered **");
                    throw new Exception("User Exception in delegate");
                });
                this.listner.PostResponse(new SpBaseResponse(2, new SpBaseMsg(1, 1)));
            });
            // Allow the thread pool to catch up
            Thread.Sleep(250);
            this.logReader.Validate(50030, "QueueUserWorkItemCallback", "WaitCallback_Context", "Unexpected Error Raising Event 'Response'");
        }

        [Test]
        public void _50030_RaiseEvent_CatchUserMessageDelegateException() {

            TestHelpers.CatchUnexpected(() => {
                this.listner.MsgReceived += new Action<ISpMessage>((msg) => {
                    Console.WriteLine("** Message Received triggered **");
                    throw new Exception("User Exception in delegate");
                });
                this.listner.PostMessage(new SpBaseResponse(2, new SpBaseMsg(1, 1)));
            });
            // Allow the thread pool to catch up
            Thread.Sleep(250);
            this.logReader.Validate(50030, "QueueUserWorkItemCallback", "WaitCallback_Context", "Unexpected Error Raising Event 'Message'");
        }


        #endregion

    }
}
