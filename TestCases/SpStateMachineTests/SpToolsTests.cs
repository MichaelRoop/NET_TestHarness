using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SpToolsTests {

        #region Data

        HelperLogReader logReader = new HelperLogReader();

        #endregion

        #region Setup

        [SetUp]
        public void TestSetup() {
            this.logReader.StartLogging();
        }

        [TearDown]
        public void TestTeardown() {
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion

        #region Test Data

        private readonly string className = "SpTools";

        #endregion


        #region GetIdString

        [Test]
        public void _0_GetIdString_OkFromCache() {

            Dictionary<int, string> cache = new Dictionary<int, string>();
            cache.Add(0, "Zero State");
            cache.Add(100, "One Hundred State");
            cache.Add(1000, "One Thousand State");

            string ret = "";
            bool converterCalled = false;
            TestHelpers.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "Blah"; });
            });
            Assert.IsFalse(converterCalled, "Converter should not have been called");
            Assert.AreEqual("One Hundred State", ret);
        }


        [Test]
        public void _0_GetIdString_OkAddedToCache() {

            Dictionary<int, string> cache = new Dictionary<int, string>();
            cache.Add(0, "Zero State");
            cache.Add(1000, "One Thousand State");

            // Not cached
            string ret = "";
            bool converterCalled = false;
            TestHelpers.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "One Hundred State"; });
            });
            Assert.IsTrue(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);

            // Check if it is good and well cached by the method
            ret = "";
            converterCalled = false;
            TestHelpers.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "Blah!"; });
            });
            Assert.IsFalse(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);
        }



        [Test]
        public void _51000_GetIdString_NullDictionary() {
            TestHelpers.CatchExpected(51000, this.className, "GetIdString", "Null currentStrings Argument", () => {
                SpTools.GetIdString(0, null, (key) => { return ""; });
            });
        }

        [Test]
        public void _51001_GetIdString_NullFunc() {
            TestHelpers.CatchExpected(51001, this.className, "GetIdString", "Null converterFunc Argument", () => {
                SpTools.GetIdString(0, new Dictionary<int,string>(), null);
            });
        }

        [Test]
        public void _51003_GetIdString_ConverterError() {
            TestHelpers.CatchExpected(51003, this.className, "GetIdString", "Error in Calling Id to String Converter Method", () => {
                SpTools.GetIdString(0, new Dictionary<int, string>(), (key) => { throw new Exception("Blah!"); });
            });
        }

        #endregion


    }
}
