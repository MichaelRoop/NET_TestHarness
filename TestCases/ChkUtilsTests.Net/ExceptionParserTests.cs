using ChkUtils.Net.ExceptionParsers;
using NUnit.Framework;
using System;
using System.Text;
using System.Xml;

namespace TestCases.ChkUtilsTests.Net {

    [TestFixture]
    public class ExceptionParserTests {

        [Test]
        public void XmlExceptionParserTest() {
            try {
                XmlException x = new XmlException("Blah Error", null, 25, 100);
                x.Data.Add("User Key 1", "User Value 1");
                throw x;
            }
            catch (XmlException e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                // There are 3 for native Xml fields and 1 for user added above
                this.TestRegularFields(parser, 4, "XmlException", "Blah Error Line 25, position 100.");
                this.ValidateExtraInfo(parser, "User Key 1", "User Value 1");
                this.ValidateExtraInfo(parser, "Line Number", "25");
                this.ValidateExtraInfo(parser, "Line Position", "100");
                this.ValidateExtraInfo(parser, "Source URI", null);
            }
        }


        [Test]
        public void ExceptionParserTest() {
            try {
                Exception x = new Exception("Default Blah Error");
                x.Data.Add("DefaultUserKey1", "DefaultUserValue1");
                throw x;
            }
            catch (Exception e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                this.TestRegularFields(parser, 1, "Exception", "Default Blah Error");
                this.ValidateExtraInfo(parser, "DefaultUserKey1", "DefaultUserValue1");
            }
        }


        [Test]
        public void DefaultExceptionParserTest() {
            string msg = "This is an out of range exception defaulting to Exception level";
            try {
                throw new IndexOutOfRangeException(msg);
            }
            catch (Exception e) {
                this.TestRegularFields(ExceptionParserFactory.Get(e), 0, "IndexOutOfRangeException", msg);
            }
        }



        [Test, Description("Test the recursive iteration through nested Exception Parsers")]
        public void InnerExceptionParserTest() {
            try {
                new ChkUtilsTestHelpers.Level1().DoIt();
            }
            catch (Exception e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);

                StringBuilder stackTrace = new StringBuilder();

                int index = 1;
                while (parser != null) {
                    // Test for order
                    switch (index++) {
                        case 1:
                            this.TestRegularFields(parser, 0, "Exception", "Level1 Exception - highest level exception");
                            break;
                        case 2:
                            this.TestRegularFields(parser, 0, "FormatException", "Level2 Format Exception - middle exception");
                            break;
                        case 3:
                            this.TestRegularFields(parser, 0, "Exception", "Level3 Exception - most inner exception");
                            break;
                        default:
                            Assert.Fail("There should only be three levels");
                            break;
                    }

                    // For show
                    stackTrace.AppendLine(String.Format("{0} : {1}", parser.Info.Name, parser.Info.Msg));
                    parser.ExtraInfo.ForEach(
                        item => stackTrace.AppendLine(String.Format("{0}={1}", item.Name, item.Value)));
                    parser.GetStackFrames(true).ForEach(
                        item => stackTrace.AppendLine(item));
                    parser = parser.InnerParser;
                }

                Console.WriteLine(stackTrace.ToString());
            }
        }


        #region Private Methods

        private void ValidateExtraInfo(IExceptionParser parser, string key, string value) {
            ExceptionExtraInfo i = parser.ExtraInfo.Find((item) => item.Name == key);
            Assert.IsNotNull(i);
            Assert.AreEqual(value, i.Value);
        }

        private void TestRegularFields(IExceptionParser parser, int extraCount, string name, string msg) {
            Assert.IsNotNull(parser);
            Assert.AreEqual(extraCount, parser.ExtraInfo.Count, "The count of extra info items is off");
            Assert.AreEqual(name, parser.Info.Name);
            Assert.AreEqual(msg, parser.Info.Msg);
        }

        #endregion




    }
}
