using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.ChkUtilsTests {

    public class ChkUtilsTestHelpers {


        #region Recursive Inner Exception test classes

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


    }
}
