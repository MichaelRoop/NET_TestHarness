using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;
using ChkUtils.ErrObjects;
using NUnit.Framework;

namespace TestCases {

    public class TestHelpers {

        #region Exception Throwers

        /// <summary>
        /// Do not move this class - line number detection tested. This classed called by 
        /// another class
        /// </summary>
        public class InnerClass {

            /// <summary>
            /// Throw an exception from this method
            /// </summary>
            /// <param name="name"></param>
            public void DoException(string name) {
                throw new Exception("Throw from InnerClass.DoIt() with name:" + name);  // @ Line 26 DO NOT CHANGE
            }

        }

        /// <summary>
        /// Do not move this class - line number detection tested
        /// </summary>
        public class OuterClass {

            /// <summary>
            /// Class another class method that throws an exception
            /// </summary>
            public void DoNestedException() {
                new InnerClass().DoException("Fred"); // @ Line 39 DO NOT CHANGE
            }

            /// <summary>
            /// Throw an exception from this method
            /// </summary>
            /// <param name="name"></param>
            public void DoException(string name) {
                throw new Exception("Throw from OuterClass.DoIt() with name:" + name);
            }

            public void DoNestedFaultException() {
                WrapErr.ToErrorReportFaultException(9191, "Unexpected error", () => new InnerClass().DoException("George"));
            }

            public void DoNestedErrReportException() {
                WrapErr.ToErrorReportException(9292, "Unexpected error", () => new InnerClass().DoException("Ziffle"));
            }

            public int RetDoNestedFaultException() {
                WrapErr.ToErrorReportFaultException(9191, "Unexpected error", () => new InnerClass().DoException("George"));
                return 1;
            }

            public int RetDoNestedErrReportException() {
                WrapErr.ToErrorReportException(9292, "Unexpected error", () => new InnerClass().DoException("Ziffle"));
                return 1;
            }

        }

        #endregion


        public static void NonExceptionAction() {
        }

        public static int NonExceptionFunc() {
            return 100;
        }


        #region Test Wrappers

        public static void CatchUnexpected(Action action) {
            try {
                WrapErr.ToErrorReportException(-1, "Unexpected Error running Test", () => {
                    action.Invoke();
                });
            }
            catch (ErrReportException e) {
                TestHelpers.ErrToConsole(e.Report);
                Assert.Fail("Unexpected Exception Occured on test");
            }
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
            TestHelpers.ErrToConsole(err);
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
            Console.WriteLine("{0} {1}.{2} : {3}{4}{5}", e.Code, e.AtClass, e.AtMethod, e.Msg, Environment.NewLine, e.StackTrace);
        }

        #endregion
    }
}

