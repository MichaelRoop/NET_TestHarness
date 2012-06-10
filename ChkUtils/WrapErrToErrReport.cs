using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils {

    /// <summary>
    /// Partial class of WrapErr with the ErrToReport wrappers
    /// </summary>
    public static partial class WrapErr {

        #region Simple String Error Message Methods

        /// <summary>
        /// Wrap an action and report the results with an ErrReport object. No exceptions will propegate
        /// </summary>
        /// <param name="report">The ErrReport object to initialise</param>
        /// <param name="code">The error code on error</param>
        /// <param name="msg">The error message on error</param>
        /// <param name="action"></param>
        public static void ToErrReport(out ErrReport report, int code, string msg, Action action) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
            }
        }

        #endregion


        #region Efficient Defered Error Message Formating Methods

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
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, WrapErr.SafeAction(errMsgFunc), e);
            }
        }

        #endregion



    }
}
