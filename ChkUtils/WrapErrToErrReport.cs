using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;
using System.ServiceModel;

namespace ChkUtils {

    /// <summary>
    /// Partial class of WrapErr with the ErrToReport wrappers
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        #region Action Only

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, string msg, Action action) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
            }
        }

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate.
        /// The error message is an anonymous method to prevent costly string formating when no error
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, Func<string> errMsgFunc, Action action) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
            }
        }

        #endregion

        #region Action And Finally

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will 
        /// propagate and a finally will be safely executed
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, string msg, Action action, Action finallyAction) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }


        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
        /// and a finally will be executed.  The error message is an anonymous method to prevent costly 
        /// string formating when no error.
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion

        #region Func Only

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will 
        /// propagate and a T value will be returned. Check for successful ErrReport before
        /// using the return T value since it may not be valid.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>A T value or default(T) on failure</returns>
        public static T ToErrReport<T>(out ErrReport report, int code, string msg, Func<T> func) {
            try {
                report = new ErrReport();
                return func.Invoke();
            }
            catch (ErrReportException e) {
                report = e.Report;
                return default(T);
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
                return default(T);
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
                return default(T);
            }
        }


        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will 
        /// propagate and a T value will be returned. Check for successful ErrReport before
        /// using the return T value since it may not be valid.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>A T value or default(T) on failure</returns>
        public static T ToErrReport<T>(out ErrReport report, int code, Func<string> errMsgFunc, Func<T> func) {
            try {
                report = new ErrReport();
                return func.Invoke();
            }
            catch (ErrReportException e) {
                report = e.Report;
                return default(T);
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
                return default(T);
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
                return default(T);
            }
        }
        
        #endregion

        #region Func And Finally

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will 
        /// propagate and a T value will be returned. Check for successful ErrReport before
        /// using the return T value since it may not be valid.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="func">The function to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        /// <returns>A T value or default(T) on failure</returns>
        public static T ToErrReport<T>(out ErrReport report, int code, string msg, Func<T> func, Action finallyAction) {
            try {
                report = new ErrReport();
                return func.Invoke();
            }
            catch (ErrReportException e) {
                report = e.Report;
                return default(T);
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
                return default(T);
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
                return default(T);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }


        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will 
        /// propagate and a T value will be returned. Check for successful ErrReport before
        /// using the return T value since it may not be valid.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="func">The function to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        /// <returns>A T value or default(T) on failure</returns>
        public static T ToErrReport<T>(out ErrReport report, int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            try {
                report = new ErrReport();
                return func.Invoke();
            }
            catch (ErrReportException e) {
                report = e.Report;
                return default(T);
            }
            catch (FaultException<ErrReport> e) {
                report = e.Detail;
                return default(T);
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
                return default(T);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion

    }
}
