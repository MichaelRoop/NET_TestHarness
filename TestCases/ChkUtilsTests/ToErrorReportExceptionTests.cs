using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils;
using ChkUtils.ErrObjects;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ToErrorReportExceptionTests {

        #region Action Only Tests

        [Test]
        public void ExceptionCaughtOnAction() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ToErrorReportException(12345, "Unexpected Error Processing Block", () => {
                    new TestHelpers.OuterClass().DoNestedException();
                });
            });
            this.Validate(err, 12345, "ExceptionCaughtOnAction", "Unexpected Error Processing Block");
        }

        #endregion

        #region Private Methods

        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ToErrorReportExceptionTests", method, msg, "OuterClass.DoNestedException", "InnerClass.DoException");

        }


        #endregion

    }


}

