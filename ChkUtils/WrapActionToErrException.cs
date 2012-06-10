using System;

namespace ChkUtils {

    /// <summary>
    /// Partial class implementation with catch action to ErrReportException
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        #region Simple String Error Message Methods

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ActionOnly(int code, string msg, Action action) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
            }
        }


        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Has a finally action also
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The action to invoke</param>
        public static void ActionFinaly(int code, string msg, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion
        
        #region Efficient Defered Error Message Formating Methods

        /// <summary>
        /// Wrap an action to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Efficient form where error message formating
        /// Func is not invoked unless there is an error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="errMsgFunc">The error message on error function</param>
        /// <param name="action">The action to invoke</param>
        public static void ActionOnly(int code, Func<string> errMsgFunc, Action action) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
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
        public static void ActionFinaly(int code, Func<string> errMsgFunc, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }

        #endregion

    }
}
