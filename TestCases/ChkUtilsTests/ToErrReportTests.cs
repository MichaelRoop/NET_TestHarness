using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils.ErrObjects;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ToErrReportTests {

        [Test]
        public void Valid() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                TestHelpers.NonExceptionAction();
            });
            Assert.AreEqual(0, err.Code, String.Format("Encountered error '{0}'", err.Msg));
        }

        [Test]
        public void Valid_FinallyExceptionCaught() {
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
        public void Valid_ConfirmMsgFormaterNotInvoked() {
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
        public void Exception_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new TestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "Exception_ErrorFormater", "The error formating has been invoked");
        }

        [Test]
        public void FaultExceptionCaught() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Unexpected error", () => {
                new TestHelpers.OuterClass().DoNestedFaultException();
            });

            Console.WriteLine("StackTrace:" + err.StackTrace);

            //Validate(err, 9191, "FaultException_ErrorFormater", "The error formating has been invoked");
            TestHelpers.ValidateErrReport(err, 9191, 
                "OuterClass", 
                "DoNestedFaultException", 
                "Unexpected error",
                "Throw from InnerClass.DoIt() with name:George", 
                "OuterClass",
                "DoNestedFaultException", 
                "InnerClass.DoException");
        }


        #region Private Methods


        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrReportTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");

        }

        #endregion


    }
}
