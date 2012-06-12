using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChkUtils;
using ChkUtils.ErrObjects;



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
        public void ValidateArgTest() {
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, "Validate arg", () => {
                WrapErr.ValidateParam(null, "zork", 8888);
            });
            Assert.AreEqual(8888, err.Code);
            Assert.AreEqual("TestTests", err.AtClass);
            Assert.AreEqual("ValidateArgTest", err.AtMethod);
            Assert.AreEqual("Null zork Argument", err.Msg);
            Assert.AreEqual("", err.StackTrace);
       
        }




        [Test]
        public void ToErrReportNoErrWithErrStringMethod () {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, 
                () => { 
                    Assert.Fail("The error formating should not have been invoked"); 
                    return "this error"; 
                },
                () => {
                    this.NonExceptionAction();
                });
        }


        [Test]
        public void ToErrReportWithException() {
            // Confirms that the error message formating section is not invoked unless there is actually an error
            ErrReport err;
            WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => { 
                new ClassTwo().DoIt(); 
            });
            Assert.AreEqual(1111, err.Code);
            Assert.AreEqual("TestTests", err.AtClass);
            Assert.AreEqual("ToErrReportWithException", err.AtMethod);
            Assert.AreEqual("The error formating has been invoked", err.Msg);
            Assert.IsTrue(err.StackTrace.Contains("ClassOne"));
            Assert.IsTrue(err.StackTrace.Contains("ClassTwo"));
        }



        [Test]
        public void TriggerExcepTest () {
            try {

                WrapErr.ToErrorReportException(12345, "Unexpected Error Processing Block", () => {
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


        private void NonExceptionAction() {
        }

    }

}
