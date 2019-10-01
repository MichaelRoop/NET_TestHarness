using NUnit.Framework;
using SpStateMachine.Converters;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using System;
using System.Threading;
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
                this.listner.PostMessage(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
            });
        }

        #endregion

        #region PostResponse

        [Test]
        public void _50033_PostResponse_Disposed() {
            TestHelpersNet.CatchExpected(50033, "SimpleEventListner", "PostResponse", "Attempting to use Disposed Object", () => {
                this.listner.Dispose();
                this.listner.PostResponse(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
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
                this.listner.PostMessage(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
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
            Assert.AreEqual(25, msgCopy.TypeId);
            Assert.AreEqual(100, msgCopy.EventId);
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
                this.listner.PostResponse(new SpBaseEventResponse(new SpIntToInt(2), new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(58)), new SpIntToInt(0), ""));
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
            Assert.AreEqual(2, msgCopy.TypeId);
            Assert.AreEqual(58, msgCopy.EventId);
        }


        [Test]
        public void _50031_RaiseEvent_ResponseNoSubscribers() {
            TestHelpersNet.CatchUnexpected(() => {
                this.listner.PostResponse(new SpBaseEventResponse(new SpIntToInt(2), new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)), new SpIntToInt(0), ""));
            });
            this.logReader.Validate(50031, "SimpleEventListner", "RaiseEvent", "No subscribers to 'Response' message");
        }


        [Test]
        public void _50031_MessageReceived_NoSubscribers() {
            TestHelpersNet.CatchUnexpected(() => {
                this.listner.PostMessage(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)));
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
                this.listner.PostResponse(new SpBaseEventResponse(new SpIntToInt(2), new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)), new SpIntToInt(0), ""));
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
                this.listner.PostMessage(new SpBaseEventResponse(new SpIntToInt(2), new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)), new SpIntToInt(0), ""));
            });
            // Allow the thread pool to catch up
            this.logReader.Validate(50030, "QueueUserWorkItemCallback", "WaitCallback_Context", "Unexpected Error Raising Event 'Message'");
        }


        #endregion

    }
}
