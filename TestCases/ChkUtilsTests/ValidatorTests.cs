using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils.ErrObjects;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ValidatorTests {

        #region ValidateParam Tests

        [Test]
        public void ValidatePara_NullArg() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ValidateParam(null, "zork", 8888);
            });
            this.Validate(err, 8888, "ValidatePara_NullArg", "Null zork Argument");
        }

        [Test]
        public void ValidateParam_ValidArg() {
            string zork = "Zorker";
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ValidateParam(zork, "zork", 8888);
            });
            Assert.AreEqual(0, err.Code, "Should not have been an error");
        }

        #endregion
        
        #region Private Methods

        private void Validate(ErrReport err, int code, string method, string msg) {
            TestHelpers.ValidateErrReport(err, code, "ValidatorTests", method, msg);
            Assert.AreEqual("", err.StackTrace);
        }

        #endregion



    }
}
