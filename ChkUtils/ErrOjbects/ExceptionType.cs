using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils.ErrOjbects {

    /// <summary>
    /// Enumeration of exception types handled in the WrapErr
    /// </summary>
    /// <author>Michael Roop</author>
    public enum ExceptionType {

        /// <summary>
        /// ErrReportException are not safe accross WCF boundries 
        /// </summary>
        Regular,

        /// <summary>
        /// ErrReportFaultException Fault Exception are safe accross WCF boundries
        /// </summary>
        Fault,

    }
}
