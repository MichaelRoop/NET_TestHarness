//using System;
//using ChkUtils.ErrObjects;
//using System.ServiceModel;

//namespace ChkUtils {

//    /// <summary>
//    /// Partial class implementation with catch action to ErrReportException
//    /// </summary>
//    /// <author>Michael Roop</author>
//    public static partial class WrapErr {

//        #region Action Only

//        ///// <summary>
//        ///// Wrap an action to catch and convert exceptions not previously caught and 
//        ///// converted to ErrReportExceptions.
//        ///// </summary>
//        ///// <param name="code">The error code</param>
//        ///// <param name="msg">The error message</param>
//        ///// <param name="action">The action to invoke</param>
//        //public static void ToErrorReportException(int code, string msg, Action action) {
//        //    WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, action, () => {;});
//        //    //try {
//        //    //    action.Invoke();
//        //    //}
//        //    //catch (ErrReportException) {
//        //    //    throw;
//        //    //}
//        //    //catch (Exception e) {
//        //    //    throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
//        //    //}
//        //}


//        ///// <summary>
//        ///// Wrap an action to catch and convert exceptions not previously caught and 
//        ///// converted to ErrReportExceptions. Efficient form where error message formating
//        ///// Func is not invoked unless there is an error
//        ///// </summary>
//        ///// <param name="code">The error code</param>
//        ///// <param name="errMsgFunc">The error message on error function</param>
//        ///// <param name="action">The action to invoke</param>
//        //public static void ToErrorReportException(int code, Func<string> errMsgFunc, Action action) {
//        //    WrapErr.WrapTryToErrorReportException(code, errMsgFunc, action, () => {;});
//        //    //try {
//        //    //    action.Invoke();
//        //    //}
//        //    //catch (ErrReportException) {
//        //    //    throw;
//        //    //}
//        //    //catch (Exception e) {
//        //    //    throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    //}
//        //}

//        //#endregion

//        ////private static void WrapTryToErrorReportException(int code, string msg, Action action) {
//        ////    try {
//        ////        action.Invoke();
//        ////    }
//        ////    catch (FaultException<ErrReport> e) {
//        ////        throw new ErrReportException(e.Detail);
//        ////    }
//        ////    catch (FaultException) {
//        ////        // Get info from Fault exception
//        ////    }
//        ////    catch (ErrReportException) {
//        ////        throw;
//        ////    }
//        ////    catch (Exception e) {
//        ////        throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
//        ////    }
//        ////}






//        //#region Action and Finally

//        ///// <summary>
//        ///// Wrap an action to catch and convert exceptions not previously caught and 
//        ///// converted to ErrReportExceptions. Has a finally action also
//        ///// </summary>
//        ///// <param name="code">The error code</param>
//        ///// <param name="msg">The error message</param>
//        ///// <param name="action">The action to invoke</param>
//        //public static void ToErrorReportException(int code, string msg, Action action, Action finallyAction) {
//        //    WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, action, finallyAction);
//        //    //try {
//        //    //    action.Invoke();
//        //    //}
//        //    //catch (ErrReportException) {
//        //    //    throw;
//        //    //}
//        //    //catch (Exception e) {
//        //    //    throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
//        //    //}
//        //    //finally {
//        //    //    WrapErr.SafeAction(finallyAction);
//        //    //}
//        //}
        

//        ///// <summary>
//        ///// Wrap an action to catch and convert exceptions not previously caught and 
//        ///// converted to ErrReportExceptions. Has a finally action also. Efficient form 
//        ///// where error message formating Func is not invoked unless there is an error
//        ///// </summary>
//        ///// <param name="code">The error code</param>
//        ///// <param name="errMsgFunc">The error message on error function</param>
//        ///// <param name="action">The action to invoke</param>
//        ///// <param name="finallyAction">The finally action to invoke</param>
//        //public static void ToErrorReportException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
//        //    WrapErr.WrapTryToErrorReportException(code, errMsgFunc, action, finallyAction);
//        //    //try {
//        //    //    action.Invoke();
//        //    //}
//        //    //catch (ErrReportException) {
//        //    //    throw;
//        //    //}
//        //    //catch (Exception e) {
//        //    //    throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    //}
//        //    //finally {
//        //    //    WrapErr.SafeAction(finallyAction);
//        //    //}
//        //}

//        #endregion





//    }
//}
