using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ChkUtils.ExceptionParsers;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ExceptionParserTests {


        [Test]
        public void XmlExceptionParserTest() {

            try {
                XmlException x = new XmlException("Blah Error", null, 25, 100);
                x.Data.Add("UserKey1", "UserValue1");
                throw x;
            }
            catch (XmlException e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                Assert.AreEqual(4, parser.GetExtraInfoInfo().Count, "The count of extra info items is off");
                this.ValidateExtraInfo(parser, "UserKey1", "UserValue1");
                this.ValidateExtraInfo(parser, "Line Number", "25");
                this.ValidateExtraInfo(parser, "Line Position", "100");
                this.ValidateExtraInfo(parser, "Source URI", null);
                Assert.AreEqual("XmlException", parser.GetInfo().Name);
                Assert.AreEqual("Blah Error Line 25, position 100.", parser.GetInfo().Msg);
            }
        }


        [Test]
        public void DefaultExceptionParserTest() {
            try {
                Exception x = new Exception("Default Blah Error");
                x.Data.Add("DefaultUserKey1", "DefaultUserValue1");
                throw x;
            }
            catch (Exception e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                Assert.AreEqual(1, parser.GetExtraInfoInfo().Count, "The count of extra info items is off");
                this.ValidateExtraInfo(parser, "DefaultUserKey1", "DefaultUserValue1");
                Assert.AreEqual("Exception", parser.GetInfo().Name);
                Assert.AreEqual("Default Blah Error", parser.GetInfo().Msg);
            }
        }



        private void ValidateExtraInfo(IExceptionParser parser, string key, string value) {
            ExceptionExtraInfo i = parser.GetExtraInfoInfo().Find((item) => item.Name == key);
            Assert.IsNotNull(i);
            Assert.AreEqual(value, i.Value);
        }


    }
}
