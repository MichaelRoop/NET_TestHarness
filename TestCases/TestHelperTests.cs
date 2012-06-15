using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestCases {

    [TestFixture]
    public class TestHelperTests {

        [Test, Explicit]
        public void TestCatchUnexpected() {
            TestHelpers.CatchUnexpected(() => { throw new Exception("Blah Exception"); });

        }


    }

}
