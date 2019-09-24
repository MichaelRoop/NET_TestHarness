using System;
using System.Collections.Generic;
using ChkUtils;
using ChkUtils.ErrObjects;
using LogUtils;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;

namespace TestCases.TestToolSet {

    /// <summary>
    /// Common functionality to access error information that may not propegate from
    /// a class on test.
    /// </summary>
    public class HelperLogReader {

        #region Data

        List<ErrReport> errors = new List<ErrReport>();

        NUnitTraceWriter consoleWriter = new NUnitTraceWriter();

        LogingMsgEventDelegate myLogReadDelegate = null;

        bool isStarted = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor with verbosity Low and above and dump every message
        /// to log immediately
        /// </summary>
        public HelperLogReader()
            : this(MsgLevel.Info, 1) {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="level">Log level verbosity</param>
        /// <param name="numberOfMsgBeforeDumpToLog">How many messages are put out before they are dumped to log</param>
        public HelperLogReader(MsgLevel level, int numberOfMsgBeforeDumpToLog) {
            // Create delegate to attach to Log
            this.myLogReadDelegate = new LogingMsgEventDelegate(this.Log_OnLogMsgEvent);

            // Set log verbosity and number of messages that have to arrive before the log
            // thread fires
            Log.SetVerbosity(level);
            Log.SetMsgNumberThreshold(numberOfMsgBeforeDumpToLog);

            // Stream the errors caught by WrapErr to the Log
            WrapErr.InitialiseOnExceptionLogDelegate(Log.LogExceptionDelegate);
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Start up the loging system
        /// </summary>
        public void StartLogging() {
            if (!this.isStarted) {
                this.isStarted = true;
                Log.OnLogMsgEvent += this.myLogReadDelegate;
                this.consoleWriter.StartLogging();
            }
        }

        /// <summary>
        /// Shut down the logging system
        /// </summary>
        public void StopLogging() {
            if (this.isStarted) {
                this.isStarted = false;
                this.consoleWriter.StopLogging();
                Log.OnLogMsgEvent -= this.myLogReadDelegate;
            }
        }

        /// <summary>
        /// Clear existing errors
        /// </summary>
        public void Clear() {
            Trace.WriteLine("<Clearing Log List>");
            this.errors.Clear();
        }


        /// <summary>
        /// Validate that the error has been generated to the logging system and 
        /// matches the values passed in
        /// </summary>
        /// <param name="code">The error code</param>
        public ErrReport Validate(int code) {
            Trace.WriteLine(String.Format("<Validating> - Looking for err {0}", code));

            // Since the thread results are dumped out by a separate thread we may have to wait for 
            // the results we will wait for 20 of 25ms time slices for max of half second
            ErrReport err = null;
            for (int i = 0; i < 26; i++) {
                err = this.errors.Find((item) => item.Code == code);
                if (err != null) {
                    break;
                }
                Thread.Sleep(100);
            }

            if (err == null) {
                Trace.WriteLine(String.Format("<Failed Validation> - err {0} - here is a list of existing errors", code));
                foreach(var e in this.errors) {
                    Trace.WriteLine(String.Format("     err {0}", e.Code));
                }
            }


            Assert.IsNotNull(err, String.Format("There was no error logged for code:{0}", code));
            return err;
        }


        /// <summary>
        /// Validate that the error has been generated to the logging system and 
        /// matches the values passed in
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="className">The class name of origine</param>
        /// <param name="method">The method name of origine</param>
        public ErrReport Validate(int code, string className, string method) {
            ErrReport err = this.Validate(code);
            Assert.AreEqual(className, err.AtClass, "Class Name Mismatch");
            Assert.AreEqual(method, err.AtMethod, "Method Name Mismatch");
            return err;
        }


        /// <summary>
        /// Validate that the error has been generated to the logging system and 
        /// matches the values passed in
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="className">The class name of origine</param>
        /// <param name="method">The method name of origine</param>
        /// <param name="msg">The error message</param>
        public void Validate(int code, string className, string method, string msg) {
            ErrReport err = this.Validate(code, className, method);
            Assert.AreEqual(msg, err.Msg, "Message Mismatch");
        }

        /// <summary>
        /// Set level message needs before being logged
        /// </summary>
        /// <param name="level">Required level</param>
        public void SetVerbosity(MsgLevel level) {
            Log.SetVerbosity(level);
        }


        /// <summary>
        /// Set the threshold of messages needing to be logged before dump thread is woken
        /// </summary>
        /// <param name="numberOfMsgBeforeDumpToLog"></param>
        public void SetLogThreshold(int numberOfMsgBeforeDumpToLog) {
            Log.SetMsgNumberThreshold(numberOfMsgBeforeDumpToLog);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Grab the ErrReport from every log event
        /// </summary>
        /// <param name="level"></param>
        /// <param name="errReport"></param>
        void Log_OnLogMsgEvent(MsgLevel level, ErrReport errReport) {
            this.errors.Add(errReport);
        }

        #endregion

    }
}
