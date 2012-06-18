﻿using System;
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
        public void LogingDelegateTest() {

            WrapErr.InitialiseOnExceptionLogDelegate(this.LogDelegate);

            try {
                WrapErr.ToErrorReportFaultException(12345, "Big Booboo", () => {
                    new TestHelpers.OuterClass().DoNestedException();
                });
            }
            catch (FaultException<ErrReport> e) {
                this.Validate(e.Detail, 12345, "LogingDelegateTest", "Big Booboo");
                Assert.IsTrue(this.logged, "The logging delegate did not get invoked");
                return;
            }
            Assert.Fail("Should have caught exception and not gotten here");
        }


        [Test]
        public void LogingAndFormatingDelegateTest() {

            WrapErr.InitialiseOnExceptionLogDelegate(this.LogDelegate);

            try {
                WrapErr.ToErrorReportFaultException(9999,
                    () => { this.msgFormated = true; return "Oops"; }, 
                    () => { new TestHelpers.OuterClass().DoNestedException(); });
            }
            catch (FaultException<ErrReport> e) {
                this.Validate(e.Detail, 9999, "LogingAndFormatingDelegateTest", "Oops");
                Assert.IsTrue(this.msgFormated, "The message formating delegate did not get invoked");
                return;
            }
            Assert.Fail("Should have caught exception and not gotten here");
        }

        #endregion




        #region Private Methods

        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrorReportFaultExceptionTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");
        }


        /// <summary>
        /// A method which throws an exception
        /// </summary>
        private void ExceptionThrowTest(string msg) {
            Console.WriteLine("Throwing exception from method");
            throw new Exception(msg);
        }


        private void LogDelegate(ErrReport report) {
            Console.WriteLine("LogDelegateInvoked:{0} {1}.{2} : {3}", report.Code, report.AtClass, report.AtMethod, report.Msg);
            this.logged = true;
        }

        #endregion


    }
}