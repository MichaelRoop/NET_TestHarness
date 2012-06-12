using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;

namespace ChkUtils {    

    /// <summary>
    /// Partial class implementation with catch Func to ErrReportException
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {


        #region Func Onlye

        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="msg">The error message</param>
        /// <param name="action">The func to invoke</param>
        public static T ToErrorReportException<T>(int code, string msg, Func<T> func) {
            try {
                return func.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));
            }
        }


        /// <summary>
        /// Wrap a Func to catch and convert exceptions not previously caught and 
        /// converted to ErrReportExceptions. Efficient form where error message formating
        /// Func is not invoked unless there is an error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="errMsgFunc"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T ToErrorReportException<T>(int code, Func<string> errMsgFunc, Func<T> func) {
            try {
                return func.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e));
            }
        }

        #endregion
        



    }
}
