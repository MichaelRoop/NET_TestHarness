using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace TestCases {

    [TestFixture, Explicit]
    public class Scratch {

        #region Cast from Interface

        public interface ITestInt {
            int Id { get; set; }
        }

        public class ChildBase : ITestInt {
            private int id = 1;

            public int Id {
                get { return id; }
                set { id = value; }
            }

            public ChildBase(int id) {
                this.id = id;
            }
        }

        public class Child1 : ChildBase {
            private int intProp = 99;
            public Child1(int id) : base(id) { }
            public int IntProperty { get { return this.intProp; } set { this.intProp = value; } }
        }

        public class Child2 : ChildBase {
            string stringProp = "Hi";
            public Child2(int id) : base(id) { }
            public string StringProperty { get { return this.stringProp; } set { this.stringProp = value; } }
        }


        [Test, Explicit]
        public void TestCastFromInterface() {

            ITestInt intProp = new Child1(22);
            ITestInt strProp = new Child2(33);
            Trace.WriteLine(string.Format("Child1 cast id:{0} - int prop:{1}", ((Child1)intProp).Id, ((Child1)intProp).IntProperty));
            Trace.WriteLine(string.Format("Child2 cast id:{0} - str prop:{1}", ((Child2)strProp).Id, ((Child2)strProp).StringProperty));

        }


        #endregion

        #region Do Scratch

        [Test, Explicit]
        public void DoScratch() {

            try {

              //  int i = Int32.Parse("22r");

                XmlException x = new XmlException("Blah error", null, 25, 100);
                throw x;
            }
            catch (XmlException e) {
                Trace.WriteLine(string.Format("Line Number {0}", e.LineNumber));
                Trace.WriteLine(string.Format("Line Position {0}", e.LinePosition));
                Trace.WriteLine(string.Format("Source URI {0}", e.SourceUri));
                Trace.WriteLine(string.Format("Message {0}", e.Message));
                Trace.WriteLine(string.Format("Stack Trace {0}", e.StackTrace));
            }

            catch (FormatException e) {
                Trace.WriteLine("Caught Format Exception");

                Trace.WriteLine(string.Format("Helplink {0}", e.HelpLink));
                Trace.WriteLine(string.Format("Message {0}", e.Message));
                Trace.WriteLine(string.Format("Source {0}", e.Source));
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");

                if (e.Data != null) {
                    Trace.WriteLine("Getting extra details");
                    foreach (DictionaryEntry item in e.Data) {
                        Trace.WriteLine(string.Format("The key is '{0}' and the value is: {1}", item.Key, item.Value));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Caught Exception");
                if (e.Data != null) {
                    foreach (DictionaryEntry item in e.Data) {
                        Console.WriteLine(string.Format("The key is '{0}' and the value is: {1}", item.Key, item.Value));
                    }
                }
            }


        }

        #endregion


    }
}
