using NUnit.Framework;
using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
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
                ISpEventMessage msg = new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100));
                ISpEventMessage response = new SpBaseEventResponse(new SpIntToInt(33), msg, new SpIntToInt(0), "");
                Assert.AreEqual(msg.Uid, response.Uid, "Guid mismatch between message and response");
            });
        }

    }
}
