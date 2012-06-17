﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils.ErrObjects;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ToErrReportTests {

        #region Action

        [Test]
        public void Action_Valid() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                TestHelpers.NonExceptionAction();
            });
            Assert.AreEqual(0, err.Code, String.Format("Encountered error '{0}'", err.Msg));
        }

        [Test]
        public void Action_Valid_FinallyExceptionCaught() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; },
                delegate {
                    TestHelpers.NonExceptionAction();
                },
                delegate {
                    throw new Exception("This should be ignored");
                });
            Assert.AreEqual(0, err.Code, String.Format("Encountered error '{0}'", err.Msg));
        }
        
        [Test]
        public void Action_Valid_ConfirmMsgFormaterNotInvoked() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111,
                () => {
                    Assert.Fail("The error formating should not have been invoked");
                    return "this error";
                },
                () => { TestHelpers.NonExceptionAction(); });
        }

        [Test]
        public void Action_Caught_Exception_SimpleString() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Woka woka error", () => {
                new TestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "Action_Caught_Exception_SimpleString", "Woka woka error");
        }


        [Test]
        public void Action_Caught_Exception_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new TestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "Action_Caught_Exception_ErrorFormater", "The error formating has been invoked");
        }

        [Test]
        public void Action_Caught_FaultException() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                new TestHelpers.OuterClass().DoNestedFaultException();
            });
            Assert.AreEqual(9191, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("DoNestedFaultException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:George")); 
            Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
            Assert.IsTrue(err.StackTrace.Contains("DoNestedFaultException"));
            Assert.IsTrue(err.StackTrace.Contains("InnerClass.DoException"));
        }

        [Test]
        public void Action_Caught_ErrReportException() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                new TestHelpers.OuterClass().DoNestedErrReportException();
            });
            Assert.AreEqual(9292, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("DoNestedErrReportException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:Ziffle"));
            Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
            Assert.IsTrue(err.StackTrace.Contains("DoNestedErrReportException"));
            Assert.IsTrue(err.StackTrace.Contains("InnerClass.DoException"));
        }

        #endregion

        #region Func

        [Test]
        public void Func_Valid() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                return TestHelpers.NonExceptionFunc();
            });
            Assert.AreEqual(0, err.Code, String.Format("Encountered error '{0}'", err.Msg));
        }

        [Test]
        public void Func_Valid_FinallyExceptionCaught() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; },
                delegate {
                return TestHelpers.NonExceptionFunc();
                },
                delegate {
                    throw new Exception("This should be ignored");
                });
            Assert.AreEqual(0, err.Code, String.Format("Encountered error '{0}'", err.Msg));
        }

        [Test]
        public void Func_Valid_ConfirmMsgFormaterNotInvoked() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111,
                () => {
                    Assert.Fail("The error formating should not have been invoked");
                    return "this error";
                },
                () => { return TestHelpers.NonExceptionFunc(); });
        }

        [Test]
        public void Func_Caught_Exception_SimpleString() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, "Woka woka error", () => {
                new TestHelpers.OuterClass().DoNestedException();
                return 1;
            });
            Validate(err, 1111, "Func_Caught_Exception_SimpleString", "Woka woka error");
        }


        [Test]
        public void Func_Caught_Exception_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new TestHelpers.OuterClass().DoNestedException();
                return 1;
            });
            Validate(err, 1111, "Func_Caught_Exception_ErrorFormater", "The error formating has been invoked");
        }

        [Test]
        public void Func_Caught_FaultException() {
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                return new TestHelpers.OuterClass().RetDoNestedFaultException();
            });
            Assert.AreEqual(9191, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("RetDoNestedFaultException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:George"));
            Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
            Assert.IsTrue(err.StackTrace.Contains("RetDoNestedFaultException"));
            Assert.IsTrue(err.StackTrace.Contains("InnerClass.DoException"));
        }

        [Test]
        public void Func_Caught_ErrReportException() {
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                return new TestHelpers.OuterClass().RetDoNestedErrReportException();
            });
            Assert.AreEqual(9292, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("RetDoNestedErrReportException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:Ziffle"));
            Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
            Assert.IsTrue(err.StackTrace.Contains("RetDoNestedErrReportException"));
            Assert.IsTrue(err.StackTrace.Contains("InnerClass.DoException"));
        }


        #endregion

        #region Private Methods


        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrReportTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");

        }

        #endregion


    }
}
