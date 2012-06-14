using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ServiceModel;
using ChkUtils.ErrObjects;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ToErrorReportFaultExceptionTests {

        #region Data

        private bool logged = false;

        private bool msgFormated = false;

        #endregion

        #region Setup
        
        [SetUp]
        public void SetupTest() {
            this.logged = false;
            this.msgFormated = false;
        }

        #endregion

        #region Logging delegate tests

        [Test]
        public void FaultException_LogingDelegateTest() {
            try {
                WrapErr.ToLoggedErrorReportFaultException(12345, "Big Booboo", this.LogDelegate, () => {
                    this.ExceptionThrowTest("Blah blah blah");
                });
            }
            catch (FaultException<ErrReport> e) {
                ErrReport err = e.Detail;
                Assert.AreEqual(12345, err.Code);
                Assert.AreEqual("ToErrorReportFaultExceptionTests", err.AtClass);
                Assert.AreEqual("FaultException_LogingDelegateTest", err.AtMethod);
                Assert.AreEqual("Big Booboo", err.Msg);
                Assert.IsTrue(this.logged, "The logging delegate did not get invoked");
                Assert.IsTrue(err.StackTrace.Contains("ToErrorReportFaultExceptionTests"));
                Assert.IsTrue(err.StackTrace.Contains("Blah blah blah"));
                return;
            }
            Assert.Fail("Should have caught exception and not gotten here");
        }

        [Test]
        public void FaultException_LogingAndFormatingDelegateTest() {
            try {
                WrapErr.ToLoggedErrorReportFaultException(9999,
                    () => { this.msgFormated = true; return "Oops"; }, 
                    this.LogDelegate, 
                    () => { this.ExceptionThrowTest("It Blew!"); });
            }
            catch (FaultException<ErrReport> e) {
                ErrReport err = e.Detail;
                Assert.AreEqual(9999, err.Code);
                Assert.AreEqual("ToErrorReportFaultExceptionTests", err.AtClass);
                Assert.AreEqual("FaultException_LogingAndFormatingDelegateTest", err.AtMethod);
                Assert.AreEqual("Oops", err.Msg);
                Assert.IsTrue(this.msgFormated, "The message formating delegate did not get invoked");
                Assert.IsTrue(err.StackTrace.Contains("ToErrorReportFaultExceptionTests"));
                Assert.IsTrue(err.StackTrace.Contains("It Blew!"));
                return;
            }
            Assert.Fail("Should have caught exception and not gotten here");
        }

        #endregion




        #region Private Methods

        /// <summary>
        /// A method which throws an exception
        /// </summary>
        private void ExceptionThrowTest(string msg) {
            Console.WriteLine("Throwing exception from method");
            throw new Exception(msg);
        }


        private void LogDelegate(ErrReport report) {
            this.logged = true;
        }

        #endregion


    }
}
