using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils;

//using NUnit.f

namespace TestCases.TestNUnit {
 
    public class ClassOne {
        public void DoIt(string name) {
            throw new Exception ("Throw from ClassOne.DoIt() with name:" + name);
        }
    }

    public class ClassTwo {
        public void DoIt () {
            ClassOne c = new ClassOne ();
            c.DoIt ("Fred");
        }
    }


    [TestFixture]
    public class TestTests {

        [Test]
        public void TriggerNUnit () {
            Console.WriteLine ("I have done it");
        }

        [Test]
        public void TriggerExcepTest () {

            try {

                WrapErr.ActionOnly(12345, "This is unexpected", () => {
                    ClassTwo cc = new ClassTwo();
                    cc.DoIt();
                });

            }
            catch (ErrReportException e) {
                Console.WriteLine("Catching the error exception");
                ErrReport r = e.Report;
                Console.WriteLine("{0} {1}.{2}. {3} - {4}", r.Code, r.AtClass, r.AtMethod, r.Msg, r.StackTrace);

            }
            catch (Exception e) {
                Console.WriteLine("Caught exception that should not have propagated");

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }


        }


    }

}
