using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using System.Xml;

namespace TestCases {

    [TestFixture, Explicit]
    public class Scratch {

        [Test, Explicit]
        public void DoScratch() {

            try {

              //  int i = Int32.Parse("22r");

                XmlException x = new XmlException("Blah error", null, 25, 100);
                throw x;
            }
            catch (XmlException e) {
                Console.WriteLine("Line Number {0}", e.LineNumber);
                Console.WriteLine("Line Position {0}", e.LinePosition);
                Console.WriteLine("Source URI {0}", e.SourceUri);
                Console.WriteLine("Message {0}", e.Message);
                Console.WriteLine("Stack Trace {0}", e.StackTrace);

            }

            catch (FormatException e) {
                Console.WriteLine("Caught Format Exception");

                Console.WriteLine("Helplink {0}", e.HelpLink);
                Console.WriteLine("Message {0}", e.Message);
                Console.WriteLine("Source {0}", e.Source);
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");

                if (e.Data != null) {
                    Console.WriteLine("Getting extra details");
                    foreach (DictionaryEntry item in e.Data) {
                        Console.WriteLine("The key is '{0}' and the value is: {1}", item.Key, item.Value);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Caught Exception");
                if (e.Data != null) {
                    foreach (DictionaryEntry item in e.Data) {
                        Console.WriteLine("The key is '{0}' and the value is: {1}", item.Key, item.Value);
                    }
                }
            }


        }



    }
}
