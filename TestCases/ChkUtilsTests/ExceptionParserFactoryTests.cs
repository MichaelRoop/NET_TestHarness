using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils.ExceptionParsers;
using System.Xml;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ExceptionParserFactoryTests {

        [Test]
        public void XmlExeptionParserFetch() {
            this.TestParserFetch(typeof(XmlExceptionParser), () => {
                throw new XmlException("Blah Error", null, 25, 100);
            });
        }


        [Test]
        public void ExeptionParserFetch() {
            this.TestParserFetch(typeof(DefaultExceptionParser), () => {
                throw new Exception("Blah Error");
            });
        }


        [Test]
        public void DefaultExeptionParserFetch() {
            this.TestParserFetch(typeof(DefaultExceptionParser), () => {
                throw new InsufficientMemoryException("Wonka wonka out of memory");
            });
        }



        private void TestParserFetch(Type expected, Action action) {
            try {
                action.Invoke();
            }
            catch (Exception e) {
                IExceptionParser parser = ExceptionParserFactory.Get(e);
                Assert.AreEqual(expected.Name, parser.GetType().Name, "Factory returned wrong type of parser");
            }
        }




    }
}
