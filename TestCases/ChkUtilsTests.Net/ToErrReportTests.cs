using ChkUtils.Net;
using ChkUtils.Net.ErrObjects;
using NUnit.Framework;
using System;

namespace TestCases.ChkUtilsTests.Net {

    [TestFixture]
    public class ToErrReportTests {

        #region Setup

        [SetUp]
        public void TestSetup() {
            ChkUtils.Net.WrapErr.SetStackTools(new StackTools());
        }

        [TearDown]
        public void TestTeardown() {
        }

        #endregion

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
                new ChkUtilsTestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "Action_Caught_Exception_SimpleString", "Woka woka error");
        }


        [Test]
        public void Action_Caught_Exception_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new ChkUtilsTestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "Action_Caught_Exception_ErrorFormater", "The error formating has been invoked");
        }


        [Test]
        public void Action_Caught_ErrReportException() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                new ChkUtilsTestHelpers.OuterClass().DoNestedErrReportException();
            });
            Assert.AreEqual(9292, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("DoNestedErrReportException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:Ziffle"));
//            Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
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
                new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                return 1;
            });
            Validate(err, 1111, "Func_Caught_Exception_SimpleString", "Woka woka error");
        }


        [Test]
        public void Func_Caught_Exception_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new ChkUtilsTestHelpers.OuterClass().DoNestedException();
                return 1;
            });
            Validate(err, 1111, "Func_Caught_Exception_ErrorFormater", "The error formating has been invoked");
        }

        [Test]
        public void Func_Caught_ErrReportException() {
            ErrReport err;
            int ret = WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                return new ChkUtilsTestHelpers.OuterClass().RetDoNestedErrReportException();
            });
            Assert.AreEqual(9292, err.Code);
            Assert.AreEqual("OuterClass", err.AtClass);
            Assert.AreEqual("RetDoNestedErrReportException", err.AtMethod);
            Assert.AreEqual("Unexpected error", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("Throw from InnerClass.DoIt() with name:Ziffle"));
            //Assert.IsTrue(err.StackTrace.Contains("OuterClass"));
            Assert.IsTrue(err.StackTrace.Contains("RetDoNestedErrReportException"));
            Assert.IsTrue(err.StackTrace.Contains("InnerClass.DoException"));
        }


        #endregion

        #region Private Methods


        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpersNet.ValidateErrReport(err, code, "ToErrReportTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");

        }

        #endregion


    }
}
