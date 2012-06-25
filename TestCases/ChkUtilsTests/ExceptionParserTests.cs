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

        #region Recursive Inner Excpetion classes

        public class Level3 {
            public void DoIt() {
                throw new Exception("Level3 Exception - most inner exception");
            }
        }

        public class Level2 {
            public void DoIt() {
                try {
                    new Level3().DoIt();
                }
                catch (Exception e) {
                    throw new FormatException("Level2 Format Exception - middle exception", e);
                }
            }
        }

        public class Level1 {
            public void DoIt() {
                try {
                    new Level2().DoIt();
                }
                catch (Exception e) {
                    throw new Exception("Level1 Exception - highest level exception", e);
                }
            }
        }

        #endregion


        [Test]
        public void XmlExceptionParserTest() {

            try {
                XmlException x = new XmlException("Blah Error", null, 25, 100);
                x.Data.Add("UserKey1", "UserValue1");
                throw x;
            }
            catch (XmlException e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                Assert.AreEqual(4, parser.ExtraInfo.Count, "The count of extra info items is off");
                this.ValidateExtraInfo(parser, "UserKey1", "UserValue1");
                this.ValidateExtraInfo(parser, "Line Number", "25");
                this.ValidateExtraInfo(parser, "Line Position", "100");
                this.ValidateExtraInfo(parser, "Source URI", null);
                Assert.AreEqual("XmlException", parser.Info.Name);
                Assert.AreEqual("Blah Error Line 25, position 100.", parser.Info.Msg);
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
                Assert.AreEqual(1, parser.ExtraInfo.Count, "The count of extra info items is off");
                this.ValidateExtraInfo(parser, "DefaultUserKey1", "DefaultUserValue1");
                Assert.AreEqual("Exception", parser.Info.Name);
                Assert.AreEqual("Default Blah Error", parser.Info.Msg);
            }
        }



        [Test]
        public void InnerExceptionParserTest() {
            try {
                new Level1().DoIt();
            }
            catch (Exception e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);

                StringBuilder stackTrace = new StringBuilder();
                while (parser != null) {
                    //Console.WriteLine("Name:{0}", parser.GetInfo().Name); 
                    //Console.WriteLine("Msg:{0}", parser.GetInfo().Msg);

                    stackTrace.AppendLine(String.Format("{0} : {1}", parser.Info.Name, parser.Info.Msg));
                    parser.ExtraInfo.ForEach(
                        item => stackTrace.AppendLine(String.Format("{0}={1}", item.Name, item.Value)));
                    parser.GetStackFrames(true).ForEach(
                        item => stackTrace.AppendLine(item));
                    parser = parser.InnerParser;
                }

                Console.WriteLine(stackTrace.ToString());


                //Assert.AreEqual(1, parser.GetExtraInfoInfo().Count, "The count of extra info items is off");
                //this.ValidateExtraInfo(parser, "DefaultUserKey1", "DefaultUserValue1");
                //Assert.AreEqual("Exception", parser.GetInfo().Name);
                //Assert.AreEqual("Default Blah Error", parser.GetInfo().Msg);
            }
        }




        private void ValidateExtraInfo(IExceptionParser parser, string key, string value) {
            ExceptionExtraInfo i = parser.ExtraInfo.Find((item) => item.Name == key);
            Assert.IsNotNull(i);
            Assert.AreEqual(value, i.Value);
        }






    }
}
