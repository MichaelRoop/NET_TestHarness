using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ServiceModel;
using ChkUtils.ErrObjects;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    [TestFixture]
    public class ToErrorReportFaultExceptionTests {


        private void ExceptionThrowTest() {
            Console.WriteLine("Throwing exception from method");

            throw new Exception("Blah blah blah");
        }


        [Test]
        public void FaultExceptionLogingDelegateTest() {

            bool logged = false;
            ChkUtils.LogingMsgDelegate logDelegate = new ChkUtils.LogingMsgDelegate((report) => {
                logged = true;
            });


            try {
                //WrapErr.ToErrorReportFaultException(12345, "Big Booboo", (errReport) => { logged = true; }, () => {
                WrapErr.ToErrorReportFaultException(12345, "Big Booboo", logDelegate, () => {
                    this.ExceptionThrowTest();
                });

                Console.WriteLine("After wrap");

            }
            catch (FaultException<ErrReport> e) {

                Console.WriteLine("In catch");


                ErrReport err = e.Detail;
                Assert.AreEqual(12345, err.Code);
                Assert.AreEqual("ToErrorReportFaultExceptionTests", err.AtClass);
                Assert.AreEqual("FaultExceptionLogingDelegateTest", err.AtMethod);
                Assert.AreEqual("Big Booboo", err.Msg);
                Assert.IsTrue(logged, "The logging delegate did not get invoked");

                Assert.IsTrue(err.StackTrace.Contains("ToErrorReportFaultExceptionTests"));
                Assert.IsTrue(err.StackTrace.Contains("Blah blah blah"));


                return;
            }


            Assert.Fail("Should have caught exception and not gotten here");


            //ErrReport err;
            //WrapErr.ToErrReport(out err, 1111, () => { return "The error formating has been invoked"; }, () => {
            //    new ClassTwo().DoIt();
            //});



        }




    }
}
