using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ChkUtils.ErrObjects;

namespace ChkUtils {

    /// <summary>
    /// Partial class implementation with catch funt to ErrReportFaultException
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
        
        #region Func Only

        ///// <summary>
        ///// Wrap a function returning T to catch and convert exceptions not previously caught and 
        ///// converted to FaultException<ErrReport> to traverse WCF boundries.
        ///// </summary>
        ///// <typeparam name="T">The return type of the function</typeparam>
        ///// <param name="code">The error code</param>
        ///// <param name="msg">The error message</param>
        ///// <param name="action">The action to invoke</param>
        ///// <returns>A T value</returns>
        //public static T ToErrorReportFaultException<T>(int code, string msg, Func<T> func) {
        //    return WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, func, () => { ; });
        //    //try {
        //    //    return func.Invoke();
        //    //}
        //    //catch (FaultException<ErrReport>) {
        //    //    throw;
        //    //}
        //    //catch (ErrReportException e) {
        //    //    throw new FaultException<ErrReport>(e.Report);
        //    //}
        //    //catch (Exception e) {
        //    //    throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, msg, e));
        //    //}
        //}


        ///// <summary>
        ///// Wrap a function to catch and convert exceptions not previously caught and 
        ///// converted to FaultException<ErrReport> to traverse WCF boundries. Efficient 
        ///// form where error message formating Func is not invoked unless there is an error
        ///// </summary>
        ///// <typeparam name="T">The return type of the function</typeparam>
        ///// <param name="code">The error code</param>
        ///// <param name="errMsgFunc">The error message on error function</param>
        ///// <param name="func">The function to invoke</param>
        ///// <returns>A T value</returns>
        //public static T ToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func) {
        //    return WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, func, () => { ; });
        //    //try {
        //    //    return func.Invoke();
        //    //}
        //    //catch (FaultException<ErrReport>) {
        //    //    throw;
        //    //}
        //    //catch (ErrReportException e) {
        //    //    throw new FaultException<ErrReport>(e.Report);
        //    //}
        //    //catch (Exception e) {
        //    //    throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
        //    //}
        //}

        //#endregion

        //#region Func and Finally

        ///// <summary>
        ///// Wrap a function to catch and convert exceptions not previously caught and 
        ///// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        ///// finally action also 
        ///// </summary>
        ///// <typeparam name="T">The return type of the function</typeparam>
        ///// <param name="code">The error code</param>
        ///// <param name="msg">The error message</param>
        ///// <param name="func">The function to invoke</param>
        ///// <param name="finallyAction">The finally action</param>
        //public static T ToErrorReportFaultException<T>(int code, string msg, Func<T> func, Action finallyAction) {
        //    return WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, func, finallyAction);
        //    //try {
        //    //    return func.Invoke();
        //    //}
        //    //catch (FaultException<ErrReport>) {
        //    //    throw;
        //    //}
        //    //catch (ErrReportException e) {
        //    //    throw new FaultException<ErrReport>(e.Report);
        //    //}
        //    //catch (Exception e) {
        //    //    throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, msg, e));
        //    //}
        //    //finally {
        //    //    WrapErr.SafeAction(finallyAction);
        //    //}
        //}


        ///// <summary>
        ///// Wrap a function to catch and convert exceptions not previously caught and 
        ///// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        ///// finally action also. Efficient form where error message formating Func is 
        ///// not invoked unless there is an error
        ///// </summary>
        ///// <typeparam name="T">The return type of the function</typeparam>
        ///// <param name="code">The error code</param>
        ///// <param name="errMsgFunc">The error message on error function</param>
        ///// <param name="func">The function to invoke</param>
        ///// <param name="finallyAction">The finally action to invoke</param>
        //public static T ToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
        //    return WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, func, finallyAction);
        //    //try {
        //    //    return func.Invoke();
        //    //}
        //    //catch (FaultException<ErrReport>) {
        //    //    throw;
        //    //}
        //    //catch (ErrReportException e) {
        //    //    throw new FaultException<ErrReport>(e.Report);
        //    //}
        //    //catch (Exception e) {
        //    //    throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
        //    //}
        //    //finally {
        //    //    WrapErr.SafeAction(finallyAction);
        //    //}
        //}

        #endregion
        
    }
}
