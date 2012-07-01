using System;
using System.Threading;
using NUnit.Framework;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SimpleEventListnerTests {

        #region Test Setup

        private ISpEventListner listner = null;

        [SetUp]
        public void Setup() {
            this.listner = new SimpleEventListner();
        }

        [TearDown]
        public void Teardown() {
            this.listner = null;
        }

        #endregion

        #region MessageReceived

        [Test]
        public void MessageReceived_NoSubscribers() {
            Assert.DoesNotThrow(() => {
                this.listner.PostMessage(new SpBaseMsg(1, 1));
            });
        }

        [Test]
        public void MessageReceived_validMsg() {
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
        public void ResponseReceived_validMsg() {
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
        public void ResponseReceived_NoSubscribers() {
            Assert.DoesNotThrow(() => {
                this.listner.PostResponse(new SpBaseResponse(2, new SpBaseMsg(1, 1)));
            });
        }

        #endregion

    }
}
