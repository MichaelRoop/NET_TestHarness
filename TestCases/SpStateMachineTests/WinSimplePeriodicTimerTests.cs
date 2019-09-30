using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpStateMachine.PeriodicTimers;
using System.Threading;
using System.Diagnostics;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class WinSimplePeriodicTimerTests {

        #region Setup

        WinSimpleTimer timer = null;

        [SetUp]
        public void SetupTests() {
            if (this.timer != null) {
                this.timer.Dispose();
                this.timer = null;
            }
            this.timer = new WinSimpleTimer();
        }

        #endregion

        #region SetInterval

        [Test]
        public void _0_SetInterval_OK() {
            TestHelpers.CatchUnexpected(() => {
                this.timer.SetInterval(new TimeSpan(0, 0, 1));
            });
        }

        [Test]
        public void _50000_SetInterval_ZeroTimespan() {
            TestHelpers.CatchExpected(50000, "WinSimpleTimer", "SetInterval", "The interval cannot be 0 milliseconds total", () => {
                this.timer.SetInterval(new TimeSpan());
            });
        }

        [Test]
        public void _50002_SetInterval_Disposed() {
            TestHelpers.CatchExpected(50002, "WinSimpleTimer", "SetInterval", "Attempting to use Disposed Object", () => {
                this.timer.Dispose();
                this.timer.SetInterval(new TimeSpan());
            });
        }
        
        #endregion

        #region Dispose

        [Test]
        public void MultiDispose() {
            Assert.DoesNotThrow(() => {
                this.timer.Dispose();
                this.timer.Dispose();
                this.timer.Dispose();
            });
        }

        #endregion

        #region OnWakeup

        [Test]
        public void OnWakeup_noSubscribers() {
            Assert.DoesNotThrow(() => {
                this.timer.SetInterval(new TimeSpan(0, 0, 0, 0, 25));
                this.timer.Start();
                Thread.Sleep(200);
                this.timer.Stop();
            });
        }

        /// <summary>
        /// To make sure the timer is tiggering the event
        /// </summary>
        [Test]
        public void OnWakeup_PulseCount() {
            int count = 0;
            this.timer.SetInterval(new TimeSpan(0, 0, 0, 0, 100));
            this.timer.OnWakeup+=new Action(() => { 
                count++;
                Trace.WriteLine(string.Format("Wakeup {0}", count));
            });
            this.timer.Start();
            Thread.Sleep(1000);
            this.timer.Stop();
            Thread.Sleep(500);

            Assert.IsTrue(count >= 9 && count <= 11, String.Format("pulse count:{0} was not between 9 & 11", count));
            Console.WriteLine("Pulse Count on every 100ms for 1 second is {0}", count);
        }



        #endregion

        #region Start

        [Test]
        public void _0_Start_Multi() {
            TestHelpers.CatchUnexpected(() => {
                this.timer.Start();
                this.timer.Start();
                this.timer.Start();
            });
        }


        [Test]
        public void _50003_Start_Disposed() {
            TestHelpers.CatchExpected(50003, "WinSimpleTimer", "Start", "Attempting to use Disposed Object", () => {
                this.timer.Dispose();
                this.timer.Start();
            });
        }

        #endregion

        #region Stop

        [Test]
        public void _0_Stop_Multi() {
            TestHelpers.CatchUnexpected(() => {
                this.timer.Stop();
                this.timer.Stop();
                this.timer.Stop();
            });
        }

        [Test]
        public void _50005_Start_Disposed() {
            TestHelpers.CatchExpected(50005, "WinSimpleTimer", "Stop", "Attempting to use Disposed Object", () => {
                this.timer.Dispose();
                this.timer.Stop();
            });
        }

        #endregion

    }
}
