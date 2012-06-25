//using System;
//using System.Diagnostics;
//using System.Reflection;
//using ChkUtils.ErrObjects;
//using System.ServiceModel;

//namespace ChkUtils {

//    /// <summary>
//    /// Partial WrapErr class implementation with common utilities
//    /// </summary>
//    /// <author>Michael Roop</author>
//    public static partial class WrapErr {
        
//        #region Safe Action Methods

//        ///// <summary>
//        ///// Safely executes an action. All exceptions caught and ignored
//        ///// </summary>
//        ///// <param name="action">The action to invoke</param>
//        //public static void SafeAction (Action action) {
//        //    try {
//        //        action.Invoke();
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
//        //        // At this point we do not want to report on any error back to the application
//        //    }
//        //}


//        ///// <summary>
//        ///// Safely executes a function that returns a value . All exceptions caught and ignored. If an exception
//        ///// is thrown, the default for the type T will be returned so it has to be checked
//        ///// </summary>
//        ///// <typeparam name="T">The function return type</typeparam>
//        ///// <param name="func">The function to invoke</param>
//        ///// <returns></returns>
//        //public static T SafeAction<T>(Func<T> func) {
//        //    try {
//        //        return func.Invoke();
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
//        //        // At this point we do not want to report on any error back to the application
//        //        return default(T);
//        //    }
//        //}


//        ///// <summary>
//        ///// Safely executes a function that returns a value . All exceptions caught and ignored. If an exception
//        ///// is thrown, the default for the type T will be returned so it has to be checked
//        ///// </summary>
//        ///// <typeparam name="T">The function return type</typeparam>
//        ///// <param name="func">The function to invoke</param>
//        ///// <returns></returns>
//        //public static string SafeAction(Func<string> func) {
//        //    try {
//        //        return func.Invoke();
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
//        //        // At this point we do not want to report on any error back to the application
//        //        return "Exception trying to format error string:" + e.Message;
//        //    }
//        //}



//        #endregion

//        #region Stack Frame Methods

//        ///// <summary>
//        ///// Get the file name with the path stripped off from the stack frame
//        ///// </summary>
//        ///// <param name="frame">The frame with the information</param>
//        ///// <returns>The file name</returns>
//        //public static string FileName(StackFrame frame) {
//        //    if (frame == null) {
//        //        Debug.WriteLine("WrapErr.GetFileName : Null frame");
//        //        return "NoFileName";
//        //    }
            
//        //    string name = frame.GetFileName();
//        //    if (name == null) {
//        //        Debug.WriteLine("WrapErr.GetFileName : Null name in frame");
//        //        return "NoFileName";
//        //    }

//        //    // Strip off the path
//        //    if (name.Length > 0) {
//        //        int pos = name.LastIndexOf('\\');
//        //        if (name.Length > 0 && pos != -1 && pos < (name.Length - 1)) {
//        //            name = name.Substring(pos + 1);
//        //        }
//        //    }
//        //    return name;
//        //}


//        ///// <summary>
//        ///// Get the method name from the stack frame
//        ///// </summary>
//        ///// <param name="frame">The frame with the information</param>
//        ///// <returns>The method name</returns>
//        //public static string MethodName(StackFrame frame) {
//        //    try {
//        //        return frame.GetMethod().Name;
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine(String.Format("WrapErr.GetMethodName : Exception {0} getting method name:{1}", e.GetType().Name, e.Message));
//        //        return "NoMethodName";
//        //    }
//        //}


//        ///// <summary>
//        ///// Get the line number from the stack frame
//        ///// </summary>
//        ///// <param name="frame">The frame with the information</param>
//        ///// <returns>The method name</returns>
//        //public static int Line(StackFrame frame) {
//        //    try {
//        //        return frame.GetFileLineNumber();
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine(String.Format("WrapErr.GetLine : Exception {0} getting line number:{1}", e.GetType().Name, e.Message));
//        //        return 0;
//        //    }
//        //}
        

//        ///// <summary>
//        ///// Get the class name from the stack frame
//        ///// </summary>
//        ///// <param name="frame">The frame with the information</param>
//        ///// <returns>The class name</returns>
//        //public static string ClassName(StackFrame frame) {
//        //    try {
//        //        return frame.GetMethod().DeclaringType.Name;
//        //    }
//        //    catch (Exception e) {
//        //        Debug.WriteLine(String.Format("WrapErr.GetClassName : Exception {0} getting class name:{1}", e.GetType().Name, e.Message));
//        //        return "NoClassName";
//        //    }
//        //}

//        #endregion

//        #region ErrReport Helper Methods

//        ///// <summary>
//        ///// Experimental method to walk through stack until you encounter the first non ErrWrap class method that does not have the <>
//        ///// </summary>
//        ///// <returns></returns>
//        //private static MethodBase FirstNonWrappedMethod() {
//        //    // Go at least one up.
//        //    int index = 1;
//        //    StackTrace st = new StackTrace();

//        //    MethodBase mb = st.GetFrame(index).GetMethod();
//        //    while (true) {

//        //        //Console.WriteLine("{0}  {1}  {2}", mb.DeclaringType.Name, typeof(WrapErr).Name, mb.Name);
//        //        if (mb.DeclaringType.Name != typeof(WrapErr).Name && !mb.Name.Contains("<")) {
//        //            return mb;
//        //        }

//        //        ++index;
//        //        StackFrame sf = st.GetFrame(index);
//        //        if (sf == null) {
//        //            return mb;
//        //        }

//        //        MethodBase tmp = sf.GetMethod();
//        //        if (tmp == null) {
//        //            return mb;
//        //        }
//        //        mb = tmp;
//        //    }
//        //}


////        /// <summary>
////        /// Helper to create an error report object with the class and method names of calling method that
////        /// had an exeption occur. Names are found by relection
////        /// </summary>
////        /// <param name="code">Unique error code</param>
////        /// <param name="msg">Error message</param>
////        /// <param name="e">Exception of origine</param>
////        /// <returns>And error report object</returns>
////        private static ErrReport GetErrReport(int code, string msg, Exception e) {
////            try {
////                // Always needs to be called directly from one of the wrap objects to get the right number of the stack frame
////                // This will give us the class and method names in which the wrap method is used
//////                return new ErrReport(code, new StackTrace().GetFrame(2).GetMethod(), msg, e);
////                return WrapErr.LogErr(new ErrReport(code, StackFrameTools.FirstNonWrappedMethod(typeof(WrapErr)), msg, e));
////            }
////            catch (Exception ex) {
////                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
////                return WrapErr.LogErr(new ErrReport(code, "UnknownClass", "UnknownMethod", msg, e));
////            }
////        }



////        private static ErrReport GetErrReport(int code, string msg) {
////            try {
////                // Called directly from a method in an application. One level up and no stack trace
//////                return new ErrReport(code, new StackTrace().GetFrame(1).GetMethod(), msg);
////                return WrapErr.LogErr(new ErrReport(code, StackFrameTools.FirstNonWrappedMethod(typeof(WrapErr)), msg));
////            }
////            catch (Exception ex) {
////                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
////                return WrapErr.LogErr(new ErrReport(code, "UnknownClass", "UnknownMethod", msg));
////            }
////        }


////        /// <summary>
////        /// Log the ErrReport
////        /// </summary>
////        /// <param name="err"></param>
////        private static ErrReport LogErr(ErrReport err) {
////            lock (WrapErr.onExceptionLogLock) {
////                if (WrapErr.onExceptionLog != null) {
////                    WrapErr.SafeAction(() => WrapErr.onExceptionLog.Invoke(err));
////                }
////            }
////            return err;
////        }
        
//        #endregion

//        //#region Wrap Main Macros

//        ///// <summary>
//        ///// Wrapper of try catch finally to consitently produce an ErrReportException no matter what the
//        ///// Exception. Information from previous ErrReportExceptions is just retthrown while 
//        ///// FaultException ErrReport are converted to ErrReportException with embedded ErrReport info
//        ///// </summary>
//        ///// <param name="code">The unique error number</param>
//        ///// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
//        ///// <param name="action">The action to execute</param>
//        ///// <param name="finallyAction">The finally action to execute</param>
//        //private static void WrapTryToErrorReportException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
//        //    try {
//        //        action.Invoke();
//        //    }
//        //    catch (FaultException<ErrReport> e) {
//        //        throw new ErrReportException(e.Detail);
//        //    }
//        //    catch (FaultException e) {
//        //        throw WrapErr.FaultExceptionToErrReportException(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    catch (ErrReportException) {
//        //        throw;
//        //    }
//        //    catch (Exception e) {
//        //        throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(() => finallyAction);
//        //    }
//        //}


//        ///// <summary>
//        ///// Wrapper of try catch finally to consitently produce an FaultException ErrReport no matter what 
//        ///// the Exception. Information from previous FaultException ErrReport is just retthrown while 
//        ///// ErrReportException are converted to ExceptionFault ErrReport with embedded ErrReport info</summary>
//        ///// <param name="code">The unique error number</param>
//        ///// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
//        ///// <param name="action">The action to execute</param>
//        ///// <param name="finallyAction">The finally action to execute</param>
//        //private static void WrapTryToErrorReportFaultException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
//        //    try {
//        //        action.Invoke();
//        //    }
//        //    catch (FaultException<ErrReport>) {
//        //        throw;
//        //    }
//        //    catch (FaultException e) {
//        //        throw WrapErr.FaultExceptionToErrReportFaultException(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    catch (ErrReportException e) {
//        //        throw new FaultException<ErrReport>(e.Report);
//        //    }
//        //    catch (Exception e) {
//        //        throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(() => finallyAction);
//        //    }
//        //}
        

//        ///// <summary>
//        ///// Wrapper of try catch finally to consitently produce an ErrReportException no matter what the
//        ///// Exception. Information from previous ErrReportExceptions is just retthrown while 
//        ///// FaultException ErrReport are converted to ErrReportException with embedded ErrReport info
//        ///// </summary>
//        ///// <param name="code">The unique error number</param>
//        ///// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
//        ///// <param name="action">The action to execute</param>
//        ///// <param name="finallyAction">The finally action to execute</param>
//        //private static T WrapTryToErrorReportException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
//        //    try {
//        //        return func.Invoke();
//        //    }
//        //    catch (FaultException<ErrReport> e) {
//        //        throw new ErrReportException(e.Detail);
//        //    }
//        //    catch (FaultException e) {
//        //        throw WrapErr.FaultExceptionToErrReportException(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    catch (ErrReportException) {
//        //        throw;
//        //    }
//        //    catch (Exception e) {
//        //        throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(() => finallyAction);
//        //    }
//        //}


//        ///// <summary>
//        ///// Wrapper of try catch finally to consitently produce a FaultException ErrReport no matter what the
//        ///// Exception. Information from previous FaultException ErrReport is just retthrown while 
//        ///// ErrReportException are converted to FaultException ErrReport with embedded ErrReport info
//        ///// </summary>
//        ///// <param name="code">The unique error number</param>
//        ///// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
//        ///// <param name="action">The action to execute</param>
//        ///// <param name="finallyAction">The finally action to execute</param>
//        //private static T WrapTryToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
//        //    try {
//        //        return func.Invoke();
//        //    }
//        //    catch (FaultException<ErrReport>) {
//        //        throw;
//        //    }
//        //    catch (FaultException e) {
//        //        throw WrapErr.FaultExceptionToErrReportFaultException(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    catch (ErrReportException e) {
//        //        throw new FaultException<ErrReport>(e.Report);
//        //    }
//        //    catch (Exception e) {
//        //        throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(() => finallyAction);
//        //    }
//        //}


//        ///// <summary>
//        ///// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
//        ///// and a finally will be executed.  The error message is an anonymous method to prevent costly 
//        ///// string formating when no error.
//        ///// </summary>
//        ///// <param name="report">The ErrReport object to initialise</param>
//        ///// <param name="code">The error code on error</param>
//        ///// <param name="errMsgFunc">The error message on error function</param>
//        ///// <param name="action">The action to invoke</param>
//        ///// <param name="finallyAction">The finally action to invoke</param>
//        //public static ErrReport WrapTryToErrReport(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
//        //    try {
//        //        action.Invoke();
//        //        return new ErrReport();
//        //    }
//        //    catch (ErrReportException e) {
//        //        return e.Report;
//        //    }
//        //    catch (FaultException<ErrReport> e) {
//        //        return e.Detail;
//        //    }
//        //    catch (Exception e) {
//        //        return WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(finallyAction);
//        //    }
//        //}


//        ///// <summary>
//        ///// Wrap a function and report the results with an ErrReport object. No exceptions will propegate
//        ///// and a finally will be executed.  The error message is an anonymous method to prevent costly 
//        ///// string formating when no error.
//        ///// </summary>
//        ///// <param name="report">The ErrReport object to initialise</param>
//        ///// <param name="code">The error code on error</param>
//        ///// <param name="errMsgFunc">The error message on error function</param>
//        ///// <param name="func">The function to invoke</param>
//        ///// <param name="finallyAction">The finally action to invoke</param>
//        //public static T WrapTryToErrReport<T>(out ErrReport report, int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
//        //    // TODO change to ref
//        //    try {
//        //        T ret = func.Invoke(); 
//        //        report = new ErrReport();
//        //        return ret;
//        //    }
//        //    catch (ErrReportException e) {
//        //        report = e.Report;
//        //    }
//        //    catch (FaultException<ErrReport> e) {
//        //        report = e.Detail;
//        //    }
//        //    catch (Exception e) {
//        //        report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
//        //    }
//        //    finally {
//        //        WrapErr.SafeAction(finallyAction);
//        //    }
//        //    return default(T);
//        //}
        

//        ///// <summary>
//        ///// Convert a FaultException to a FautException Err Report
//        ///// </summary>
//        ///// <param name="code">The unique error code</param>
//        ///// <param name="msg">The error message</param>
//        ///// <param name="e">The FaultException to convert</param>
//        ///// <returns>A FaultException ErrReport</returns>
//        //private static FaultException<ErrReport> FaultExceptionToErrReportFaultException(int code, string msg, FaultException e) {
//        //    // TODO - may need to move up some code here to tansfer the Action and other info into the FaultException proper
//        //    return new FaultException<ErrReport>(FaultExceptionToErrReport(code, msg, e));
//        //}


//        ///// <summary>
//        ///// Convert a FaultException to an ErrReportException
//        ///// </summary>
//        ///// <param name="code">The unique error code</param>
//        ///// <param name="msg">The error message</param>
//        ///// <param name="e">The FaultException to convert</param>
//        ///// <returns>An ErrReportException</returns>
//        //private static ErrReportException FaultExceptionToErrReportException(int code, string msg, FaultException e) {
//        //    // TODO - may need to move up some code here to tansfer the Action and other info into the FaultException proper
//        //    return new ErrReportException(FaultExceptionToErrReport(code, msg, e));
//        //}


//        ///// <summary>
//        ///// Convert a non ErrReport FaultException to an ErrReport object 
//        ///// </summary>
//        ///// <param name="code"></param>
//        ///// <param name="msg"></param>
//        ///// <param name="e"></param>
//        ///// <returns></returns>
//        //private static ErrReport FaultExceptionToErrReport(int code, string msg, FaultException e) {
//        //    // TODO - get the info and pass it to a new FaultException<ErrReport>
//        //    // Need to figure out the Action string SOAP and other Properties and what can be passed and how
//        //    if (e.InnerException != null) {
//        //        return WrapErr.GetErrReport(code, msg, e);
//        //    }
//        //    else {
//        //        return WrapErr.GetErrReport(code, msg);
//        //    }
//        //}
        
//        //#endregion
        

//    }
//}
