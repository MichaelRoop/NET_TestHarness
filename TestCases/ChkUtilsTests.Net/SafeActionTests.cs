using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils.Net;

namespace TestCases.ChkUtilsTests.Net {

    [TestFixture]
    public class SafeActionTests {

        [Test]
        public void NoExceptionPropagation() {
            Assert.DoesNotThrow(() => {
                WrapErr.SafeAction(() => {
                    Console.WriteLine("Throwing Exception within safe block");
                    throw new Exception("This should be caught");
                });
            });
        }


        class obj1 {
        }

        [Test]
        public void DefaultObjectOnThrow() {
            // The safe wrapper should overwrite the valid object with default(obj1)
            obj1 o1 = new obj1();
            Assert.DoesNotThrow(() => {
                o1= WrapErr.SafeAction(() => {
                    string s2 = null;
                    string s3 = s2.Substring(0, 10);
                    return new obj1();
                });
            });
            Assert.AreEqual(o1, default(obj1));
            Assert.IsNull(o1);
        }


        [Test]
        public void ReturnsValidObj() {
            // The safe wrapper should overwrite the valid object with default(obj1)
            obj1 o1 = null;
            Assert.DoesNotThrow(() => {
                o1 = WrapErr.SafeAction(() => {
                    return new obj1();
                });
            });
            Assert.IsNotNull(o1);
        }



    }
}
