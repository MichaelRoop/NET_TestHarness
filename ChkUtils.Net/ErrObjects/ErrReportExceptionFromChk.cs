using System;
using System.Collections.Generic;
using System.Text;

namespace ChkUtils.Net.ErrObjects {

    /// <summary>Used internally in the WrapErr class to throw exceptions from Chk methods to preserve stack</summary>
    public class ErrReportExceptionFromChk : ErrReportException {

        /// <summary>Constructor</summary>
        /// <param name="report">The Report with the info for location of error</param>
        public ErrReportExceptionFromChk(ErrReport report)
            : base(report) {
        }
    }
}

