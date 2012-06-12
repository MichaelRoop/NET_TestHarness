using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;

namespace ChkUtils {

    /// <summary>
    /// Partial class of WrapErr with the validator Methods
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        /// <summary>
        /// Validates a parameter for null with a standard message
        /// </summary>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="name">The name of the object</param>
        public static void ValidateParam(object obj, string name, int code) {
            if (obj == null) {
                throw new ErrReportException(
                    WrapErr.GetErrReport(code, String.Format("Null {0} Argument", name)));
            }
        }


        /// <summary>
        /// Validates an object for null
        /// </summary>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="code">The error code if null</param>
        /// <param name="msg">The error message</param>
        public static void ValidateVar(object obj, int code, string msg) {
            if (obj == null) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg));
            }
        }


        /// <summary>
        /// Validates an object for null with defered excecution of error message formating
        /// </summary>
        /// <param name="code">The error code if null</param>
        /// <param name="obj">The object to evaluate</param>
        /// <param name="msg">The error message function</param>
        public static void ValidateVar(object obj, int code, Func<string> msgFunc) {
            if (obj == null) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(msgFunc)));
            }
        }


        /// <summary>
        /// Validates a condition to be true
        /// </summary>
        /// <param name="obj">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message</param>
        public static void ValidateTrue(bool condition, int code, string msg) {
            if (!condition) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg));
            }
        }


        /// <summary>
        /// Validates a condition to be true with defered formating of error message
        /// </summary>
        /// <param name="obj">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message function</param>
        public static void ValidateTrue(bool condition, int code, Func<string> msgFunc) {
            if (!condition) {
                throw new ErrReportException(
                    WrapErr.GetErrReport(code, WrapErr.SafeAction(msgFunc)));
            }
        }
        

        /// <summary>
        /// Validates a string not null and not empty with standard error message
        /// </summary>
        /// <param name="nullCode">Error code if string is null</param>
        /// <param name="zeroLenCode">Error code if string is empty</param>
        /// <param name="name">The string variable name</param>
        /// <param name="value">The string value to evaluate</param>
        public static void ValidateStr(int nullCode, int zeroLenCode, string name, string value) {
            WrapErr.ValidateParam(value, name, nullCode);
            WrapErr.ValidateTrue(value.Length > 0, zeroLenCode, () => { 
                return String.Format("String '{0}' is Zero Length", name); 
            });
        }


    }
}
