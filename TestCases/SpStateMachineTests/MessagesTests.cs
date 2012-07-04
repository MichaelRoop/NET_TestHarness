using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class MessagesTests {

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
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion
        
        [Test]
        public void _0_SpBaseResponseCopyOverGuid() {
            TestHelpers.CatchUnexpected(() => {
                ISpEventMessage msg = new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100));
                ISpEventMessage response = new SpBaseEventResponse(new SpIntToInt(33), msg);
                Assert.AreEqual(msg.Uid, response.Uid, "Guid mismatch between message and response");
            });
        }

    }
}
