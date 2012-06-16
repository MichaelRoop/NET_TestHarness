using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;
using System.ServiceModel;

namespace ChkUtils {

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
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, msg, e));
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
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
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
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, msg, e));
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
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion
        
    }
}
