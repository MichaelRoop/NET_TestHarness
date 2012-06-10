using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ChkUtils {
    
    /// <summary>
    /// Common exception to be used with error wrapping library
    /// </summary>
    public class ErrReportException : Exception {

        #region Data

        ErrReport report = null;

        #endregion

        #region Properties

        /// <summary>
        /// Error report object
        /// </summary>
        public ErrReport Report {
            get {
                return this.report;
            }
        }

        #endregion

        #region Constructors


        public ErrReportException ()
            : base () {
            this.report  = new ErrReport();
        }


        public ErrReportException (ErrReport report)
            : base () {
                this.report = report;
        }


        public ErrReportException (int code, string atClass, string atMethod, string msg, Exception e)
            : this (new ErrReport (code, atClass, atMethod, msg, e)) {
        }


        public ErrReportException (int code, string atClass, string atMethod, string msg)
            : this (code, atClass, atMethod, msg, null) {
        }

        #endregion
    }
}
