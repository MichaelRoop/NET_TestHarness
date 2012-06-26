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

        private bool finallyInvoked = false;

        #endregion

        #region Setup

        [SetUp]
        public void SetupTest() {
            this.logged = false;
            this.msgFormated = false;
            this.finallyInvoked = false;

            WrapErr.InitialiseOnExceptionLogDelegate(this.LogDelegate);
        }

        #endregion
        
        #region Action No Exception

        [Test]
        public void OnAction_NoException() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    Console.WriteLine("This is a non exception throwing block");
                });
            });
            Assert.AreEqual(0, err.Code);
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }

        [Test]
        public void OnAction_NoException_MsgFormatInvoked() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, () => { this.msgFormated = true; return "blah"; }, () => {
                    Console.WriteLine("This is a non exception throwing block");
                });
            });
            Assert.AreEqual(0, err.Code);
            Assert.IsFalse(this.msgFormated, "The message formatter should not have been invoked");
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }

        [Test]
        public void OnAction_NoException_FinallyInvoked() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    Console.WriteLine("This is a non exception throwing block");
                },
                delegate {
                    this.finallyInvoked = true;

                });
            });
            Assert.AreEqual(0, err.Code);
            Assert.IsTrue(this.finallyInvoked, "Finally block was not executed");
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }

        #endregion

        #region Action with Exception

        [Test]
        public void OnAction_Exception() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                });
            });
            this.Validate(err, 12345, "OnAction_Exception", "Unexpected Error Processing Block");
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }

        [Test]
        public void OnAction_Exception_MsgFormatInvoked() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, () => { this.msgFormated = true; return "Unexpected Error Processing Block"; }, () => {
                    new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                });
            });
            this.Validate(err, 12345, "OnAction_Exception_MsgFormatInvoked", "Unexpected Error Processing Block");
            Assert.IsTrue(this.msgFormated, "The message formatter should have been invoked");
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }

        [Test]
        public void OnAction_Exception_FinallyInvoked() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block",
                    delegate {
                        new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                    },
                    delegate {
                        Console.WriteLine("Immediately before the finally var is set to true");
                        this.finallyInvoked = true;
                    });
            });
            this.Validate(err, 12345, "OnAction_Exception_FinallyInvoked", "Unexpected Error Processing Block");
            Assert.IsTrue(this.finallyInvoked, "Finally block was not executed on exception");
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }
        
        #endregion

        #region Function No Exception

        [Test]
        public void OnFunction_NoException() {
            string s = "";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    return "This is a non exception throwing block";
                });
            });
            Assert.AreEqual(0, err.Code);
            Assert.AreEqual("This is a non exception throwing block", s);
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }

        [Test]
        public void OnFunction_NoException_MsgFormatInvoked() {
            string s = "";
            ErrReport err;
            bool formatInvoked = false;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, () => { formatInvoked = true; return "blah"; }, () => {
                    return "This is a non exception throwing block";
                });
            });
            Assert.AreEqual(0, err.Code);
            Assert.IsFalse(formatInvoked, "The message formatter should not have been invoked");
            Assert.AreEqual("This is a non exception throwing block", s);
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }

        [Test]
        public void OnFunction_NoException_FinallyInvoked() {
            string s = "";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg",
                delegate {
                    s = WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                        return "This is a non exception throwing block";
                    },
                    delegate {
                        this.finallyInvoked = true;

                    });
                });
            Assert.AreEqual(0, err.Code);
            Assert.IsTrue(this.finallyInvoked, "Finally block was not executed");
            Assert.AreEqual("This is a non exception throwing block", s);
            Assert.IsFalse(this.logged, "No exception - should not have been logged");
        }


        #endregion

        #region Function With Exception


        [Test]
        public void OnFunction_Exception() {
            string s = "lalala";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                    return "This should not be";
                });
            });
            this.Validate(err, 12345, "OnFunction_Exception", "Unexpected Error Processing Block");
            Assert.AreEqual("lalala", s);
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }

        [Test]
        public void OnFunction_Exception_MsgFormatInvoked() {
            string s = "lalala";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, () => { this.msgFormated = true; return "Unexpected Error Processing Block"; }, () => {
                    new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                    return "This should not be";
                });
            });
            this.Validate(err, 12345, "OnFunction_Exception_MsgFormatInvoked", "Unexpected Error Processing Block");
            Assert.IsTrue(this.msgFormated, "The message formatter should have been invoked");
            Assert.AreEqual("lalala", s);
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }

        [Test]
        public void OnFunction_Exception_FinallyInvoked() {
            string s = "lalala";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block",
                    delegate {
                        new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                        return "This should not be";
                    },
                    delegate {
                        Console.WriteLine("Immediately before the finally var is set to true");
                        this.finallyInvoked = true;
                    });
            });
            this.Validate(err, 12345, "OnFunction_Exception_FinallyInvoked", "Unexpected Error Processing Block");
            Assert.IsTrue(this.finallyInvoked, "Finally block was not executed on exception");
            Assert.AreEqual("lalala", s);
            Assert.IsTrue(this.logged, "Exception - The log delegate should have fired");
        }

        [Test]
        public void OnFunction_Exception_NoLogging() {
            WrapErr.InitialiseOnExceptionLogDelegate(null);
            string s = "lalala";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                s = WrapErr.ToErrorReportFaultException(12345, "Unexpected Error Processing Block", () => {
                    new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                    return "This should not be";
                });
            });
            this.Validate(err, 12345, "OnFunction_Exception_NoLogging", "Unexpected Error Processing Block");
            Assert.AreEqual("lalala", s);
            Assert.IsFalse(this.logged, "Exception - But the logger should not have been fired");
        }

        #endregion

        #region Private Methods

        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrorReportFaultExceptionTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");
        }

        private void LogDelegate(ErrReport report) {
            //Console.WriteLine("LogDelegateInvoked:{0} {1}.{2} : {3}", report.Code, report.AtClass, report.AtMethod, report.Msg);
            this.logged = true;
        }

        #endregion


    }
}
