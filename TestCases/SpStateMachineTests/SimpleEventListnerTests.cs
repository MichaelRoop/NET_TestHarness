using NUnit.Framework;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;
using System;
using System.Threading;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SimpleEventListnerTests {

        #region Data

        HelperLogReaderNet logReader = new HelperLogReaderNet();
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
            TestHelpersNet.CatchExpected(50032, "SimpleEventListner", "PostMessage", "Attempting to use Disposed Object", () => {
                this.listner.Dispose();
                this.listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start));
            });
        }

        #endregion

        #region PostResponse

        [Test]
        public void _50033_PostResponse_Disposed() {
            TestHelpersNet.CatchExpected(50033, "SimpleEventListner", "PostResponse", "Attempting to use Disposed Object", () => {
                this.listner.Dispose();
                this.listner.PostResponse(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Tick));
            });
        }

        #endregion

        #region MessageReceived


        [Test]
        public void _0_MessageReceived_validMsg() {
            bool received = false;
            ISpEventMessage msgCopy = null;

            TestHelpersNet.CatchUnexpected(() => {
                this.listner.MsgReceived += new Action<ISpEventMessage>((msg) => {
                    Console.WriteLine("Woke up on msg received");
                    received = true;
                    msgCopy = msg;
                });
                this.listner.PostMessage(new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start));

            });
            // On thread pool so have to wait for response
            for (int i = 0; i < 21; i++) {
                if (received) {
                    break;
                }
                Thread.Sleep(25);
            }
            Assert.IsTrue(received, "The received event was not raised");
            Assert.IsNotNull(msgCopy, "Message was not copied");
            Assert.AreEqual((int)MyMsgType.SimpleMsg, msgCopy.TypeId);
            Assert.AreEqual((int)MyEventType.Start, msgCopy.EventId);
        }

        #endregion

        #region ResponseReceived

        [Test]
        public void _0_ResponseReceived_validMsg() {
            bool received = false;
            ISpEventMessage msgCopy = null;

            TestHelpersNet.CatchUnexpected(() => {
                this.listner.ResponseReceived += new Action<ISpEventMessage>((msg) => {
                    Console.WriteLine("Woke up on response received");
                    received = true;
                    msgCopy = msg;
                });

                this.listner.PostResponse(
                    new MyBaseResponse(
                        MyMsgType.SimpleMsg, 
                        new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Tick), MyReturnCode.Success, ""));
            });
            // On thread pool so have to wait for response
            for (int i = 0; i < 21; i++) {
                if (received) {
                    break;
                }
                Thread.Sleep(25);
            }
            Assert.IsTrue(received, "The received event was not raised");
            Assert.IsNotNull(msgCopy, "Message was not copied");
            Assert.AreEqual((int)MyMsgType.SimpleMsg, msgCopy.TypeId);
            Assert.AreEqual((int)MyEventType.Tick, msgCopy.EventId);
        }


        [Test]
        public void _50031_RaiseEvent_ResponseNoSubscribers() {
            TestHelpersNet.CatchUnexpected(() => {
                this.listner.PostResponse(
                    new MyBaseResponse(
                        MyMsgType.SimpleMsg,
                        new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Tick), MyReturnCode.Success, ""));
            });
            this.logReader.Validate(50031, "SimpleEventListner", "RaiseEvent", "No subscribers to 'Response' message");
        }


        [Test]
        public void _50031_MessageReceived_NoSubscribers() {
            TestHelpersNet.CatchUnexpected(() => {
                this.listner.PostMessage(new MyBaseMsg( MyMsgType.SimpleMsg, MyEventType.Tick));
            });
            this.logReader.Validate(50031, "SimpleEventListner", "RaiseEvent", "No subscribers to 'Message' message");
        }


        [Test]
        public void _50030_RaiseEvent_CatchUserResponseDelegateException() {

            TestHelpersNet.CatchUnexpected(() => {
                this.listner.ResponseReceived += new Action<ISpEventMessage>((msg) => {
                    Console.WriteLine("** Response Received triggered **");
                    throw new Exception("User Exception in delegate");
                });
                this.listner.PostResponse(
                    new MyBaseResponse(
                        MyMsgType.SimpleMsg,
                        new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Tick), MyReturnCode.Success, ""));

            });
            // Allow the thread pool to catch up
            this.logReader.Validate(50030, "QueueUserWorkItemCallback", "WaitCallback_Context", "Unexpected Error Raising Event 'Response'");
        }


        [Test]
        public void _50030_RaiseEvent_CatchUserMessageDelegateException() {

            TestHelpersNet.CatchUnexpected(() => {
                this.listner.MsgReceived += new Action<ISpEventMessage>((msg) => {
                    Console.WriteLine("** Message Received triggered **");
                    throw new Exception("User Exception in delegate");
                });
                this.listner.PostMessage(
                    new MyBaseResponse(
                        MyMsgType.SimpleMsg,
                        new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Tick), MyReturnCode.Success, ""));
            });
            // Allow the thread pool to catch up
            this.logReader.Validate(50030, "QueueUserWorkItemCallback", "WaitCallback_Context", "Unexpected Error Raising Event 'Message'");
        }

        #endregion

    }
}
