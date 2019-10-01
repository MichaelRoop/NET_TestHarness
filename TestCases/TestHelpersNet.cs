using ChkUtils.Net;
using ChkUtils.Net.ErrObjects;
using ChkUtils.Net.ExceptionFormating;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases {
    public class TestHelpersNet {


        #region Non throwing methods

        public static void NonExceptionAction() {
        }

        public static int NonExceptionFunc() {
            return 100;
        }

        #endregion

        #region Test Wrappers

        public static void CatchUnexpected(Action action) {
            ErrReport err = new ErrReport();
            WrapErr.ToErrReport(out err, -1, "Unexpected Error running Test", () => {
                action.Invoke();
            });
            if (err.Code != 0) {
                TestHelpersNet.ErrToConsole(err);
                Assert.Fail("Unexpected Exception Occured on test:{0} {1}", err.Code, err.Msg);
            }
        }

        public static ErrReport CatchExpected(int code, string atClass, string atMethod, string msg, Action action) {
            ErrReport err = new ErrReport();
            WrapErr.ToErrReport(out err, -999999, "Unexpected Error running Test", () => {
                action.Invoke();
            });
            Assert.AreNotEqual(-999999, err.Code, "The CatchExpected has put out its own error {0}", err.Code);
            TestHelpersNet.ValidateErrReport(err, code, atClass, atMethod, msg);
            return err;
        }



        #endregion

        #region ErrReport and stack trace compare

        public static void ValidateErrReport(ErrReport err, int code, string atClass, string atMethod, string msg) {
            ValidateErrReport(err, code, atClass, atMethod, msg, new List<string>());
        }

        public static void ValidateErrReport(ErrReport err, int code, string atClass, string atMethod, string msg, params string[] stackText) {
            ValidateErrReport(err, code, atClass, atMethod, msg, stackText.ToList());
        }

        public static void ValidateErrReport(ErrReport err, int code, string atClass, string atMethod, string msg, List<string> stackText) {
            TestHelpersNet.ErrToConsole(err);
            Assert.AreEqual(code, err.Code);
            Assert.AreEqual(atClass, err.AtClass);
            Assert.AreEqual(atMethod, err.AtMethod);
            Assert.AreEqual(msg, err.Msg);
            ValidateErrReportStack(err, stackText);
        }


        public static void ValidateErrReportStack(ErrReport err, params string[] msgs) {
            ValidateErrReportStack(err, msgs.ToList());
        }

        public static void ValidateErrReportStack(ErrReport err, List<string> msgs) {
            msgs.ForEach(
                item => Assert.IsTrue(err.StackTrace.Contains(item.ToString()), String.Format("ErrReport in Stack Does not contain:{0}", item.ToString())));
        }

        #endregion

        #region Writing messages to console for debug

        public static void ErrToConsole(ErrReport e) {
            //Console.WriteLine("{0} {1}.{2} : {3}{4}{5}", e.Code, e.AtClass, e.AtMethod, e.Msg, Environment.NewLine, e.StackTrace);
            System.Diagnostics.Trace.WriteLine(String.Format("{0} {1}.{2} : {3}{4}{5}", e.Code, e.AtClass, e.AtMethod, e.Msg, Environment.NewLine, e.StackTrace));

        }

        #endregion

        #region ErrReport and stack trace compare for .NET Standard

        //public static void ValidateErrReport(ChkUtils.Net.ErrObjects.ErrReport err, int code, string atClass, string atMethod, string msg) {
        //    ValidateErrReport(err, code, atClass, atMethod, msg, new List<string>());
        //}

        //public static void ValidateErrReport(ChkUtils.Net.ErrObjects.ErrReport err, int code, string atClass, string atMethod, string msg, params string[] stackText) {
        //    ValidateErrReport(err, code, atClass, atMethod, msg, stackText.ToList());
        //}

        //public static void ValidateErrReport(ChkUtils.Net.ErrObjects.ErrReport err, int code, string atClass, string atMethod, string msg, List<string> stackText) {
        //    TestHelpers.ErrToConsole(err);
        //    Assert.AreEqual(code, err.Code);
        //    Assert.AreEqual(atClass, err.AtClass);
        //    Assert.AreEqual(atMethod, err.AtMethod);
        //    Assert.AreEqual(msg, err.Msg);
        //    ValidateErrReportStack(err, stackText);
        //}


        //public static void ValidateErrReportStack(ChkUtils.Net.ErrObjects.ErrReport err, params string[] msgs) {
        //    ValidateErrReportStack(err, msgs.ToList());
        //}

        //public static void ValidateErrReportStack(ChkUtils.Net.ErrObjects.ErrReport err, List<string> msgs) {
        //    msgs.ForEach(
        //        item => Assert.IsTrue(err.StackTrace.Contains(item.ToString()), String.Format("ErrReport in Stack Does not contain:{0}", item.ToString())));
        //}

        #endregion

        #region Writing messages to console for debug

        //public static void ErrToConsole(ChkUtils.Net.ErrObjects.ErrReport e) {
        //    //Console.WriteLine("{0} {1}.{2} : {3}{4}{5}", e.Code, e.AtClass, e.AtMethod, e.Msg, Environment.NewLine, e.StackTrace);
        //    System.Diagnostics.Trace.WriteLine(String.Format("{0} {1}.{2} : {3}{4}{5}", e.Code, e.AtClass, e.AtMethod, e.Msg, Environment.NewLine, e.StackTrace));

        //}

        #endregion

        #region Stack Trace Formating Helpers

        public static void SetSingleLineException() {
            ExceptionFormaterFactory.SetFormater(new SingleLineExceptionFormater());
        }

        public static void SetMultiLineException() {
            ExceptionFormaterFactory.SetFormater(new MultiLineExceptionFormater());
        }

        #endregion




    }
}
