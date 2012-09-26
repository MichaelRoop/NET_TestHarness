using System;

namespace ChkUtils.ErrObjects {
    
    /// <summary>
    /// Common exception to be used with error wrapping library. This object is only used as
    /// a vehicle to hold the ErrReport with the original data. It was derived from exception
    /// to enable it to be caught and rethrown in the Wrap methods
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class ErrReportException : Exception {

        #region Data

        ErrReport report = new ErrReport();

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public ErrReportException ()
            : base () {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="report">The error report object</param>
        public ErrReportException (ErrReport report)
            : base () {
                this.report = report;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">Exception of origine</param>
        public ErrReportException (int code, string atClass, string atMethod, string msg, Exception e)
            : this (new ErrReport (code, atClass, atMethod, msg, e)) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Error message</param>
        public ErrReportException(int code, string atClass, string atMethod, string msg)
            : this (code, atClass, atMethod, msg, null) {
        }

        #endregion
    }
}
