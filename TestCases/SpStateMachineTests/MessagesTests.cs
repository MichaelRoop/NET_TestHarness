using NUnit.Framework;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class MessagesTests {

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
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion
        
        [Test]
        public void _0_SpBaseResponseCopyOverGuid() {
            TestHelpersNet.CatchUnexpected(() => {
                ISpEventMessage msg = new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Tick);
                ISpEventMessage response = new MyBaseResponse(MyMsgType.SimpleMsg, msg, MyReturnCode.Success, "");
                Assert.AreEqual(msg.Uid, response.Uid, "Guid mismatch between message and response");
            });
        }

    }
}
