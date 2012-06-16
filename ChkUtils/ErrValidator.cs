using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;
using System.ServiceModel;
using ChkUtils.ErrOjbects;

namespace ChkUtils {

    /// <summary>
    /// Partial class of WrapErr with the validator Methods
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        #region ChkParam

        /// <summary>
        /// Validates a parameter for null with a standard message and default ErrReportException
        /// </summary>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="name">The name of the object</param>
        public static void ChkParam(object obj, string name, int code) {
            WrapErr.ChkParam(ExceptionType.Regular, obj, name, code);
        }


        /// <summary>
        /// Validates a parameter for null with a standard message and defined Exception type
        /// </summary>
        /// <param name="type">Exception type</param>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="name">The name of the object</param>
        public static void ChkParam(ExceptionType type, object obj, string name, int code) {
            if (obj == null) {
                ErrReport err = WrapErr.GetErrReport(code, String.Format("Null {0} Argument", name));
                if (type == ExceptionType.Regular) {
                    throw new ErrReportException(err);
                }
                else {
                    throw new FaultException<ErrReport>(err);
                }
            }
        }

        #endregion

        #region ChkVar

        /// <summary>
        /// Validates an object for null
        /// </summary>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="msg">The error message</param>
        public static void ChkVar(object obj, int code, string msg) {
            WrapErr.ChkVar(ExceptionType.Regular, obj, code, msg);
        }


        /// <summary>
        /// Validates an object for null with a standard message and defined Exception type
        /// </summary>
        /// <param name="type">Exception type to throw</param>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="msg">The error message</param>
        public static void ChkVar(ExceptionType type, object obj, int code, string msg) {
            if (obj == null) {
                ErrReport err = WrapErr.GetErrReport(code, msg);
                if (type == ExceptionType.Regular) {
                    throw new ErrReportException(err);
                }
                else {
                    throw new FaultException<ErrReport>(err);
                }
            }
        }
        

        /// <summary>
        /// Validates an object for null with defered excecution of error message 
        /// formating and default ErrReportException
        /// </summary>
        /// <param name="code">The error code if null</param>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="msg">The error message function</param>
        public static void ChkVar(object obj, int code, Func<string> msgFunc) {
            if (obj == null) {
                WrapErr.ChkVar(ExceptionType.Regular, obj, code, WrapErr.SafeAction(msgFunc));
            }
        }


        /// <summary>
        /// Validates an object for null with defered excecution of error message 
        /// and defined Exception type
        /// </summary>
        /// <param name="type">Exception type to throw</param>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="msg">The error message</param>
        public static void ChkVar(ExceptionType type, object obj, int code, Func<string> msgFunc) {
            if (obj == null) {
                WrapErr.ChkVar(type, obj, code, WrapErr.SafeAction(msgFunc));
            }
        }

        #endregion

        #region ChkTrue

        /// <summary>
        /// Validates a condition to be true
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message</param>
        public static void ChkTrue(bool condition, int code, string msg) {
            WrapErr.ChkTrue(ExceptionType.Regular, condition, code, msg);
        }


        /// <summary>
        /// Validates a condition to be true, otherwise throw the 
        /// defined exception type
        /// </summary>
        /// <param name="type">The exception type on failure</param>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message</param>
        public static void ChkTrue(ExceptionType type, bool condition, int code, string msg) {
            if (!condition) {
                ErrReport err = WrapErr.GetErrReport(code, msg);
                if (type == ExceptionType.Regular) {
                    throw new ErrReportException(err);
                }
                else {
                    throw new FaultException<ErrReport>(err);
                }
            }
        }


        /// <summary>
        /// Validates a condition to be true with defered formating of error message
        /// with ErrReportException thrown on failure
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message function</param>
        public static void ChkTrue(bool condition, int code, Func<string> msgFunc) {
            if (!condition) {
                // Format method only invoked on failure of condition
                WrapErr.ChkTrue(condition, code, WrapErr.SafeAction(msgFunc));
            }
        }


        /// <summary>
        /// Validates a condition to be true with defered formating of error message
        /// with defined exception type thrown on failure
        /// </summary>
        /// <param name="type">The exception type on failure</param>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message function</param>
        public static void ChkTrue(ExceptionType type, bool condition, int code, Func<string> msgFunc) {
            if (!condition) {
                // Format method only invoked on failure of condition
                WrapErr.ChkTrue(type, condition, code, WrapErr.SafeAction(msgFunc));
            }
        }

        #endregion

        #region ChkFalseTrue

        /// <summary>
        /// Validates a condition to be false
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if true</param>
        /// <param name="msg">The error message</param>
        public static void ChkFalse(bool condition, int code, string msg) {
            WrapErr.ChkFalse(ExceptionType.Regular, condition, code, msg);
        }


        /// <summary>
        /// Validates a condition to be false, otherwise throw the 
        /// defined exception type
        /// </summary>
        /// <param name="type">The exception type on failure</param>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if true</param>
        /// <param name="msg">The error message</param>
        public static void ChkFalse(ExceptionType type, bool condition, int code, string msg) {
            if (condition) {
                ErrReport err = WrapErr.GetErrReport(code, msg);
                if (type == ExceptionType.Regular) {
                    throw new ErrReportException(err);
                }
                else {
                    throw new FaultException<ErrReport>(err);
                }
            }
        }


        /// <summary>
        /// Validates a condition to be false with defered formating of error message
        /// with ErrReportException thrown on failure
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if true</param>
        /// <param name="msg">The error message function</param>
        public static void ChkFalse(bool condition, int code, Func<string> msgFunc) {
            if (condition) {
                // Format method only invoked on failure of condition
                WrapErr.ChkFalse(condition, code, WrapErr.SafeAction(msgFunc));
            }
        }


        /// <summary>
        /// Validates a condition to be false with defered formating of error message
        /// with defined exception type thrown on failure
        /// </summary>
        /// <param name="type">The exception type on failure</param>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if true</param>
        /// <param name="msg">The error message function</param>
        public static void ChkFalse(ExceptionType type, bool condition, int code, Func<string> msgFunc) {
            if (condition) {
                // Format method only invoked on failure of condition
                WrapErr.ChkFalse(type, condition, code, WrapErr.SafeAction(msgFunc));
            }
        }

        #endregion

        #region ChkStr

        /// <summary>
        /// Validates a string not null and not empty with standard error message
        /// and ErrReportException on failure
        /// </summary>
        /// <param name="nullCode">Error code if string is null</param>
        /// <param name="zeroLenCode">Error code if string is empty</param>
        /// <param name="name">The string variable name</param>
        /// <param name="value">The string value to evaluate</param>
        public static void ChkStr(int nullCode, int zeroLenCode, string name, string value) {
            WrapErr.ChkStr(ExceptionType.Regular, nullCode, zeroLenCode, name, value);
        }


        /// <summary>
        /// Validates a string not null and not empty with standard error message
        /// and defined exception type on failure
        /// </summary>
        /// <param name="nullCode">Error code if string is null</param>
        /// <param name="zeroLenCode">Error code if string is empty</param>
        /// <param name="name">The string variable name</param>
        /// <param name="value">The string value to evaluate</param>
        public static void ChkStr(ExceptionType type, int nullCode, int zeroLenCode, string name, string value) {
            WrapErr.ChkTrue(type, value != null, nullCode, () => {
                return String.Format("String '{0}' is Null", name);
            });
            WrapErr.ChkTrue(type, value.Length > 0, zeroLenCode, () => {
                return String.Format("String '{0}' is Empty", name);
            });
        }

        #endregion

    }
}
