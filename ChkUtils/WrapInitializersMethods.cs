using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;

namespace ChkUtils {


    /// <summary>
    /// Delegate signature for logging delegates
    /// </summary>
    /// <param name="errReport"></param>
    public delegate void LogingMsgDelegate(ErrReport errReport);


    /// <summary>
    /// Partial class implementation with initialisation methods for WrapErr static class
    /// </summary>
    /// <remarks>
    /// There are signatures that will allow a logging delegate to be fired on error. These are
    /// to be used at 'entry points' to a WCF service class. Any exception that occurs is caught 
    /// at this level and transformed to an ExceptionFault that can safely traverse WCF boundries.
    /// The log delegate allows the error to be logged in the component before the Exception
    /// fault is sent out accross the wire.
    /// </remarks>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

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


        #endregion
      

    }
}
