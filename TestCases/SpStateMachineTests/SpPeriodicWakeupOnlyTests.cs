using NUnit.Framework;
using SpStateMachine.Behaviours;
using SpStateMachine.Core;
using System.Threading;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SpPeriodicWakeupOnlyTests {

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

        #region EventReceived

        [Test]
        public void _50080_EventReceived_Disposed() {
            TestHelpersNet.CatchExpected(50080, "SpPeriodicWakeupOnly", "EventReceived", "Attempting to use Disposed Object", () => {
                SpPeriodicWakeupOnly w = new SpPeriodicWakeupOnly();
                w.Dispose();
                Thread.Sleep(100);
                w.EventReceived(BehaviorResponseEventType.MsgArrived);
            });
        }

        [Test]
        public void _50081_EventReceived_UnhandledType() {
            TestHelpersNet.CatchUnexpected(() => {
                SpPeriodicWakeupOnly w = new SpPeriodicWakeupOnly();
                w.EventReceived(BehaviorResponseEventType.Undefined);
                w.Dispose();
                Thread.Sleep(100);
            });
            this.logReader.Validate(50081, "SpPeriodicWakeupOnly", "EventReceived", "The Behavior Response Event Type 'Undefined' is not Supported");
        }

        #endregion

        #region WaitOnEvent

        [Test]
        public void _50082_WaitOnEvent_Disposed() {
            TestHelpersNet.CatchExpected(50082, "SpPeriodicWakeupOnly", "WaitOnEvent", "Attempting to use Disposed Object", () => {
                SpPeriodicWakeupOnly w = new SpPeriodicWakeupOnly();
                w.Dispose();
                Thread.Sleep(100);
                w.WaitOnEvent();
            });
        }

        #endregion

        #region OnPeriodicTimer

        [Test]
        public void _50084_OnPeriodicTimer_Busy() {
            TestHelpersNet.CatchUnexpected(() => {
                SpPeriodicWakeupOnly w = new SpPeriodicWakeupOnly();

                // setting terminate allows it to drop through the waitOnEvent
                w.EventReceived(BehaviorResponseEventType.TerminateRequest);
                w.WaitOnEvent();
                Thread.Sleep(100);
                
                // we then hit it with another event. Normaly the user would loop back to the wait to set it not busy
                w.EventReceived(BehaviorResponseEventType.PeriodicWakeup);
                
                w.Dispose();
                Thread.Sleep(100);
            });
            this.logReader.Validate(50084, "SpPeriodicWakeupOnly", "OnPeriodicTimer", "Still Busy When the Periodic Timer Woke Up");
        }

        #endregion

    }
}
