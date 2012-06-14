using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;
using System.ServiceModel;

namespace ChkUtils {

    public delegate void LogingMsgDelegate(ErrReport errReport);


    /// <summary>
    /// Partial class implementation with catch action to ErrReportFaultException
    /// </summary>
    /// <remarks>
    /// There are signatures that will allow a logging delegate to be fired on error. These are
    /// to be used at 'entry points' to a WCF service class. Any exception that occurs is caught 
    /// at this level and transformed to an ExceptionFault that can safely traverse WCF boundries.
    /// The log delegate allows the error to be logged in the component before the Exception
    /// fault is sent out accross the wire.
    /// </remarks>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        #region Action Onlye

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportFaultException(int code, string msg, Action action) {
            WrapErr.ToLoggedErrorReportFaultException(code, msg, (report) => { /* no logging */ }, action);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Invoke log action
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="logAction">The log action to invoke on error</param>
        /// <param name="action">The action to invoke</param>
        public static void ToLoggedErrorReportFaultException(int code, string msg, LogingMsgDelegate logAction, Action action) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Detail));
                throw;
            }
            catch (ErrReportException e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Report));
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                ErrReport err = WrapErr.GetErrReport(code, msg, e);
                WrapErr.SafeAction(() => logAction.Invoke(err));
                throw new FaultException<ErrReport>(err);
            }
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Efficient 
        /// form where error message formating Func is not invoked unless there is an error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportFaultException(int code, Func<string> errMsgFunc, Action action) {
            WrapErr.ToLoggedErrorReportFaultException(code, errMsgFunc, (report) => { /* No logging */ }, action);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and converted to 
        /// FaultException<ErrReport> to traverse WCF boundries. Efficient form where error 
        /// message formating Func is not invoked unless there is an error. Includes a log delegate
        /// to push log message before sending over WCF connection
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="logAction">The log action to invoke on error</param>
        /// <param name="action">The action to invoke</param>
        public static void ToLoggedErrorReportFaultException(int code, Func<string> errMsgFunc, LogingMsgDelegate logAction, Action action) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Detail));
                throw;
            }
            catch (ErrReportException e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Report));
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                ErrReport err = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
                WrapErr.SafeAction(() => logAction.Invoke(err));
                throw new FaultException<ErrReport>(err);
            }
        }
        
        #endregion

        #region Action and Finally

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        /// finally action also 
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action</param>
        public static void ToErrorReportFaultException(int code, string msg, Action action, Action finallyAction) {
            WrapErr.ToLoggedErrorReportFaultException(code, msg, (report) => { /* no logging */ }, action, finallyAction);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Invoke log action
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="logAction">The log action to invoke on error</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action</param>
        public static void ToLoggedErrorReportFaultException(int code, string msg, LogingMsgDelegate logAction, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Detail));
                throw;
            }
            catch (ErrReportException e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Report));
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                ErrReport err = WrapErr.GetErrReport(code, msg, e);
                WrapErr.SafeAction(() => logAction.Invoke(err));
                throw new FaultException<ErrReport>(err);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        /// finally action also. Efficient form where error message formating Func is 
        /// not invoked unless there is an error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrorReportFaultException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            WrapErr.ToLoggedErrorReportFaultException(code, errMsgFunc, (report) => { /* No logging */ }, action, finallyAction);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and converted to 
        /// FaultException<ErrReport> to traverse WCF boundries. Efficient form where error 
        /// message formating Func is not invoked unless there is an error. Includes a log delegate
        /// to push log message before sending over WCF connection
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="logAction">The log action to invoke on error</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action</param>
        public static void ToLoggedErrorReportFaultException(int code, Func<string> errMsgFunc, LogingMsgDelegate logAction, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Detail));
                throw;
            }
            catch (ErrReportException e) {
                WrapErr.SafeAction(() => logAction.Invoke(e.Report));
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                ErrReport err = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
                WrapErr.SafeAction(() => logAction.Invoke(err));
                throw new FaultException<ErrReport>(err);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion
        
    }
}
