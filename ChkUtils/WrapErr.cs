using System;
using System.Diagnostics;
using System.ServiceModel;
using ChkUtils.ErrObjects;
using ChkUtils.ErrOjbects;
using ChkUtils.ExceptionFormating;
using System.Collections.Generic;
using System.Text;

namespace ChkUtils {

    /// <summary>
    /// Delegate signature for logging delegates
    /// </summary>
    /// <param name="errReport">The ErrReport object that contains information for the logger</param>
    public delegate void LogingMsgDelegate(ErrReport errReport);


    /// <summary>
    /// Partial class implementation with catch Func to ErrReportException
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class WrapErr {
        
        #region Data

        /// <summary>
        /// Delegate to log the original conversion of an exception to an
        /// ErrReportException or an ExceptionFault (ErrReport type)
        /// </summary>
        static LogingMsgDelegate onExceptionLog = null;

        /// <summary>
        /// lock to enforce safe access to exception log delegate
        /// </summary>
        static object onExceptionLogLock = new object();

        #endregion

        #region Public Initialisation Methods
        
        /// <summary>
        /// Initialise the method which will post a log entry on all wrappers the ToErrFaultException wrappers
        /// where the original transformation of Exception to ExceptionFault occurs
        /// </summary>
        /// <param name="logDelegate">The delegate to invoke when exception occurs</param>
        public static void InitialiseOnExceptionLogDelegate(LogingMsgDelegate logDelegate) {
            lock (WrapErr.onExceptionLogLock) {
                WrapErr.onExceptionLog = logDelegate;
            }
        }


        /// <summary>
        /// Set the style of format for the exception string information
        /// </summary>
        /// <param name="formater">The formater to use</param>
        public static void SetExceptionFormating(IExceptionOutputFormater formater) {
            ExceptionFormaterFactory.SetFormater(formater);
        }


        #endregion
        
        #region Safe Action Methods

        /// <summary>
        /// Safely executes an action. All exceptions caught and ignored
        /// </summary>
        /// <param name="action">The action to invoke</param>
        public static void SafeAction(Action action) {
            try {
                action.Invoke();
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format(" ### {0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace));
                // At this point we do not want to report on any error back to the application
            }
        }


        /// <summary>
        /// Safely executes a function that returns a value . All exceptions caught and ignored. If an exception
        /// is thrown, the default for the type T will be returned so it has to be checked
        /// </summary>
        /// <typeparam name="T">The function return type</typeparam>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public static T SafeAction<T>(Func<T> func) {
            try {
                return func.Invoke();
            }
            catch (Exception e) {
                Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
                // At this point we do not want to report on any error back to the application
                return default(T);
            }
        }

        #endregion
        
        #region Wrap Action to ErrReportException

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportException(int code, Action action) {
            WrapErr.WrapTryToErrorReportException(code, () => { return "Unexpected Error Occured"; }, action, () => { ;});
        }



        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportException(int code, string msg, Action action) {
            WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, action, () => { ;});
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Efficient form where error message formating
        /// Func is not invoked unless there is an error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportException(int code, Func<string> errMsgFunc, Action action) {
            WrapErr.WrapTryToErrorReportException(code, errMsgFunc, action, () => { ;});
        }

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrorReportException(int code, Action action, Action finallyAction) {
            WrapErr.WrapTryToErrorReportException(code, () => { return "Unexpected Error Occured"; }, action, finallyAction);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Has a finally action also
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrorReportException(int code, string msg, Action action, Action finallyAction) {
            WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, action, finallyAction);
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Has a finally action also. Efficient form 
        /// where error message formating Func is not invoked unless there is an error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static void ToErrorReportException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            WrapErr.WrapTryToErrorReportException(code, errMsgFunc, action, finallyAction);
        }

        #endregion

        #region Wrap Action to FaultException<ErrReport>

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrorReportFaultException(int code, string msg, Action action) {
            WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, action, () => { ; });
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
            WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, action, () => { ; });
        }


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
            WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, action, finallyAction);
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
            WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, action, finallyAction);
        }

        #endregion

        #region Wrap Function to ErrReportException

        /// <summary>
        /// Wrap a function to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static T ToErrorReportException<T>(int code, Func<T> func) {
            return WrapErr.WrapTryToErrorReportException(code, () => { return "Unexpected Error Occured"; }, func, () => { ;});
        }


        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>The T type of the func method or default value on error</returns>
        public static T ToErrorReportException<T>(int code, string msg, Func<T> func) {
            return WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, func, () => { ; });
        }


        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Efficient form where error message formating
        /// Func is not invoked unless there is an error
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="code">The unique error code</param>
        /// <param name="errMsgFunc">The error msg formating method to invoke on failure</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>The T type of the func method or default value on error</returns>
        public static T ToErrorReportException<T>(int code, Func<string> errMsgFunc, Func<T> func) {
            return WrapErr.WrapTryToErrorReportException(code, errMsgFunc, func, () => { ; });
        }


        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>The T type of the func method or default value on error</returns>
        public static T ToErrorReportException<T>(int code, string msg, Func<T> func, Action finallyAction) {
            return WrapErr.WrapTryToErrorReportException(code, () => { return msg; }, func, finallyAction);
        }


        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Efficient form where error message formating
        /// Func is not invoked unless there is an error
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="code">The unique error code</param>
        /// <param name="errMsgFunc">The error msg formating method to invoke on failure</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>The T type of the func method or default value on error</returns>
        public static T ToErrorReportException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            return WrapErr.WrapTryToErrorReportException(code, errMsgFunc, func, finallyAction);
        }

        #endregion

        #region Wrap Function to FaultException<ErrReport>

        /// <summary>
        /// Wrap a function returning T to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries.
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        /// <returns>A T value</returns>
        public static T ToErrorReportFaultException<T>(int code, string msg, Func<T> func) {
            return WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, func, () => { ; });
        }


        /// <summary>
        /// Wrap a function to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Efficient 
        /// form where error message formating Func is not invoked unless there is an error
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="func">The function to invoke</param>
        /// <returns>A T value</returns>
        public static T ToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func) {
            return WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, func, () => { ; });
        }


        /// <summary>
        /// Wrap a function to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        /// finally action also 
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="func">The function to invoke</param>
        /// <param name="finallyAction">The finally action</param>
        public static T ToErrorReportFaultException<T>(int code, string msg, Func<T> func, Action finallyAction) {
            return WrapErr.WrapTryToErrorReportFaultException(code, () => { return msg; }, func, finallyAction);
        }


        /// <summary>
        /// Wrap a function to catch and convert exceptions not previously caught and 
        /// converted to FaultException<ErrReport> to traverse WCF boundries. Has a 
        /// finally action also. Efficient form where error message formating Func is 
        /// not invoked unless there is an error
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="func">The function to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static T ToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            return WrapErr.WrapTryToErrorReportFaultException(code, errMsgFunc, func, finallyAction);
        }
        
        #endregion

        #region Wrap Action To ErrReport

        /// <summary>
        /// Wrap to an ErrReport that is not accessible by user. The results will
        /// be logged if the delegate is assigned to the Wrap architecture
        /// </summary>
        /// <param name="code">The error code on error</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="code"></param>
        public static void ToErrReport(int code, Action action) {
            ErrReport err = WrapErr.WrapTryToErrReport(code, () => { return "Unexpected Error"; }, action, () => { ; });
        }


        /// <summary>
        /// Wrap to an ErrReport that is not accessible by user. The results will
        /// be logged if the delegate is assigned to the Wrap architecture
        /// </summary>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The message</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="code"></param>
        public static void ToErrReport(int code, string msg, Action action) {
            ErrReport err = WrapErr.WrapTryToErrReport(code, () => { return msg; }, action, () => { ; });
        }


        /// <summary>
        /// Wrap to an ErrReport that is not accessible by user. The results will
        /// be logged if the delegate is assigned to the Wrap architecture
        /// </summary>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The message formating function</param>
        /// <param name="action">The action to invoke</param>
        /// <param name="code"></param>
        public static void ToErrReport(int code, Func<string> msg, Action action) {
            ErrReport err = WrapErr.WrapTryToErrReport(code, msg, action, () => { ; });
        }


        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, Action action) {
            report = WrapErr.WrapTryToErrReport(code, () => { return "Unexpected Error"; }, action, () => { ; });
        }


        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="action">The action to invoke</param>
        public static void ToErrReport(out ErrReport report, int code, string msg, Action action) {
            report = WrapErr.WrapTryToErrReport(code, () => { return msg; }, action, () => { ; });
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
            report = WrapErr.WrapTryToErrReport(code, errMsgFunc, action, () => { ; });
        }


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
            report = WrapErr.WrapTryToErrReport(code, () => { return msg; }, action, finallyAction);
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
            report = WrapErr.WrapTryToErrReport(code, errMsgFunc, action, finallyAction);
        }

        #endregion

        #region Wrap Function To ErrReport

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
            return WrapErr.WrapTryToErrReport(out report, code, () => { return msg; }, func, () => { ;});
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
            return WrapErr.WrapTryToErrReport(out report, code, errMsgFunc, func, () => { ;});
        }


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
            return WrapErr.WrapTryToErrReport(out report, code, () => { return msg; }, func, finallyAction);
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
            return WrapErr.WrapTryToErrReport(out report, code, errMsgFunc, func, finallyAction);
        }

        #endregion

        #region Public Object Validators

        #region ChkDisposed

        /// <summary>
        /// Validates a condition to be true
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message</param>
        public static void ChkDisposed(bool disposed, int code) {
            WrapErr.ChkDisposed(ExceptionType.Regular, disposed, code);
        }


        /// <summary>
        /// Validates a condition to be true, otherwise throw the 
        /// defined exception type
        /// </summary>
        /// <param name="type">The exception type on failure</param>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="code">The error code if false</param>
        /// <param name="msg">The error message</param>
        public static void ChkDisposed(ExceptionType type, bool disposed, int code) {
            WrapErr.ChkFalse(type, disposed, code, "Attempting to use Disposed Object");
        }

        #endregion

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
                if (type == ExceptionType.Regular) {
                    throw new ErrReportException(WrapErr.GetErrReport(code, msg)); 
                }
                else {
                    throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, msg));
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

        #region ChkFalse

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
        
        #endregion
        
        #region Private Wrap Main Macros

        /// <summary>
        /// Wrapper of try catch finally to consitently produce an ErrReportException no matter what the
        /// Exception. Information from previous ErrReportExceptions is just retthrown while 
        /// FaultException ErrReport are converted to ErrReportException with embedded ErrReport info
        /// </summary>
        /// <param name="code">The unique error number</param>
        /// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
        /// <param name="action">The action to execute</param>
        /// <param name="finallyAction">The finally action to execute</param>
        private static void WrapTryToErrorReportException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                throw new ErrReportException(e.Detail);
            }
            catch (FaultException e) {
                throw WrapErr.FaultExceptionToErrReportException(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(() => finallyAction.Invoke());
            }
        }


        /// <summary>
        /// Wrapper of try catch finally to consitently produce an FaultException ErrReport no matter what 
        /// the Exception. Information from previous FaultException ErrReport is just retthrown while 
        /// ErrReportException are converted to ExceptionFault ErrReport with embedded ErrReport info</summary>
        /// <param name="code">The unique error number</param>
        /// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
        /// <param name="action">The action to execute</param>
        /// <param name="finallyAction">The finally action to execute</param>
        private static void WrapTryToErrorReportFaultException(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (FaultException e) {
                throw WrapErr.FaultExceptionToErrReportFaultException(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(() => finallyAction.Invoke());
            }
        }


        /// <summary>
        /// Wrapper of try catch finally to consitently produce an ErrReportException no matter what the
        /// Exception. Information from previous ErrReportExceptions is just retthrown while 
        /// FaultException ErrReport are converted to ErrReportException with embedded ErrReport info
        /// </summary>
        /// <param name="code">The unique error number</param>
        /// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
        /// <param name="action">The action to execute</param>
        /// <param name="finallyAction">The finally action to execute</param>
        private static T WrapTryToErrorReportException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            try {
                return func.Invoke();
            }
            catch (FaultException<ErrReport> e) {
                throw new ErrReportException(e.Detail);
            }
            catch (FaultException e) {
                throw WrapErr.FaultExceptionToErrReportException(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(() => finallyAction.Invoke());
            }
        }


        /// <summary>
        /// Wrapper of try catch finally to consitently produce a FaultException ErrReport no matter what the
        /// Exception. Information from previous FaultException ErrReport is just retthrown while 
        /// ErrReportException are converted to FaultException ErrReport with embedded ErrReport info
        /// </summary>
        /// <param name="code">The unique error number</param>
        /// <param name="errMsgFunc">The error message formating method that is only invoked on exception</param>
        /// <param name="action">The action to execute</param>
        /// <param name="finallyAction">The finally action to execute</param>
        private static T WrapTryToErrorReportFaultException<T>(int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            try {
                return func.Invoke();
            }
            catch (FaultException<ErrReport>) {
                throw;
            }
            catch (FaultException e) {
                throw WrapErr.FaultExceptionToErrReportFaultException(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            catch (ErrReportException e) {
                throw new FaultException<ErrReport>(e.Report);
            }
            catch (Exception e) {
                throw new FaultException<ErrReport>(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(() => finallyAction.Invoke());
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
        public static ErrReport WrapTryToErrReport(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            try {
                action.Invoke();
                return new ErrReport();
            }
            catch (ErrReportException e) {
                return e.Report;
            }
            catch (FaultException<ErrReport> e) {
                return e.Detail;
            }
            catch (Exception e) {
                return WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
            }
            finally {
                WrapErr.SafeAction(() => finallyAction.Invoke());
            }
        }


        /// <summary>
        /// Wrap a function and report the results with an ErrReport object. No exceptions will propegate
        /// and a finally will be executed.  The error message is an anonymous method to prevent costly 
        /// string formating when no error.
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="func">The function to invoke</param>
        /// <param name="finallyAction">The finally action to invoke</param>
        public static T WrapTryToErrReport<T>(out ErrReport report, int code, Func<string> errMsgFunc, Func<T> func, Action finallyAction) {
            // TODO change to ref
            try {
                T ret = func.Invoke();
                report = new ErrReport();
                return ret;
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
                WrapErr.SafeAction(() => finallyAction.Invoke());
            }
            return default(T);
        }

        #endregion

        #region Private Error Type Conversions

        /// <summary>
        /// Convert a FaultException to a FautException Err Report
        /// </summary>
        /// <param name="code">The unique error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="e">The FaultException to convert</param>
        /// <returns>A FaultException ErrReport</returns>
        private static FaultException<ErrReport> FaultExceptionToErrReportFaultException(int code, string msg, FaultException e) {
            // TODO - may need to move up some code here to tansfer the Action and other info into the FaultException proper
            return new FaultException<ErrReport>(FaultExceptionToErrReport(code, msg, e));
        }


        /// <summary>
        /// Convert a FaultException to an ErrReportException
        /// </summary>
        /// <param name="code">The unique error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="e">The FaultException to convert</param>
        /// <returns>An ErrReportException</returns>
        private static ErrReportException FaultExceptionToErrReportException(int code, string msg, FaultException e) {
            // TODO - may need to move up some code here to tansfer the Action and other info into the FaultException proper
            return new ErrReportException(FaultExceptionToErrReport(code, msg, e));
        }


        /// <summary>
        /// Convert a non ErrReport FaultException to an ErrReport object 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static ErrReport FaultExceptionToErrReport(int code, string msg, FaultException e) {
            // TODO - get the info and pass it to a new FaultException<ErrReport>
            // Need to figure out the Action string SOAP and other Properties and what can be passed and how
            if (e.InnerException != null) {
                return WrapErr.GetErrReport(code, msg, e);
            }
            else {
                return WrapErr.GetErrReport(code, msg);
            }
        }

        #endregion
        
        #region Private ErrReport Helper Methods

        /// <summary>
        /// Helper to create an error report object with the class and method names of calling method that
        /// had an exeption occur. Names are found by relection
        /// </summary>
        /// <param name="code">Unique error code</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">Exception of origine</param>
        /// <returns>And error report object</returns>
        private static ErrReport GetErrReport(int code, string msg, Exception e) {
            try {
                // Always needs to be called directly from one of the wrap objects to get the right number of the stack frame
                // This will give us the class and method names in which the wrap method is used
                //                return new ErrReport(code, new StackTrace().GetFrame(2).GetMethod(), msg, e);
                return WrapErr.LogErr(new ErrReport(code, StackFrameTools.FirstNonWrappedMethod(typeof(WrapErr)), msg, e));
            }
            catch (Exception ex) {
                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                return WrapErr.LogErr(new ErrReport(code, "UnknownClass", "UnknownMethod", msg, e));
            }
        }


        private static ErrReport GetErrReport(int code, string msg) {
            try {
                ErrReport err = new ErrReport(code, StackFrameTools.FirstNonWrappedMethod(typeof(WrapErr)), msg);
                List<string> stackFrames = StackFrameTools.FirstNonWrappedTraceStack(typeof(WrapErr), new StackTrace(1, true));

                StringBuilder sb = new StringBuilder(100);
                stackFrames.ForEach((item) => sb.AppendLine(item));
                err.StackTrace = sb.ToString();  

                return WrapErr.LogErr(err);
            }
            catch (Exception ex) {
                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                return WrapErr.LogErr(new ErrReport(code, "UnknownClass", "UnknownMethod", msg));
            }
        }


        /// <summary>
        /// Log the ErrReport
        /// </summary>
        /// <param name="err"></param>
        private static ErrReport LogErr(ErrReport err) {
            lock (WrapErr.onExceptionLogLock) {
                if (WrapErr.onExceptionLog != null) {
                    WrapErr.SafeAction(() => WrapErr.onExceptionLog.Invoke(err));
                }
            }
            return err;
        }

        #endregion

    }

}
