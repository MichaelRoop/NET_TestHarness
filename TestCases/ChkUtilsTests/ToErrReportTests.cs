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
        public void NoErr_ConfirmMsgFormaterNotInvoked() {
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
        public void WithException_ErrorFormater() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
                new TestHelpers.OuterClass().DoNestedException();
            });
            Validate(err, 1111, "WithException_ErrorFormater", "The error formating has been invoked");
        }


        #region Private Methods

        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrReportTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");

        }

        #endregion


    }
}
