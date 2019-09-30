using ChkUtils.Net;
using ChkUtils.Net.ErrObjects;
using ChkUtils.Net.Interfaces;
using LogUtils.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogUtils.Net {

    // TO-DO - Shutdown and restart to take care of thread creation and teardown
    // TO-DO - Method to set the wakeup time

    /// <summary>
    /// Delegate signature for log message event
    /// </summary>
    /// <param name="errReport">The ErrReport object that contains information for the logger</param>
    /// <param name="level">The message log level</param>
    public delegate void LogingMsgEventDelegate(MsgLevel level, ErrReport errReport);


    /// <summary>
    /// Log message wrapper that will raise an event on valid level of message logged so 
    /// that the user can get the information to feed to their own log subsystem to write 
    /// out the values.
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class Log {

        #region Data

        /// <summary>Protects the verbosity level on change</summary>
        private static object levelLock = new object();

        /// <summary>The current verbosity level</summary>
        private static MsgLevel verbosity = MsgLevel.Off;

        /// <summary>The queue of log messages to push</summary>
        private static List<KeyValuePair<MsgLevel, ErrReport>> messages = new List<KeyValuePair<MsgLevel, ErrReport>>();

        /// <summary>Protects access to the message queue</summary>
        private static object msgQLock = new object();

        /// <summary>Thread that pulls messages off the queue and logs them</summary>
        private static Task pushThread = new Task(Log.MsgPushThread);

        /// <summary>flag for thread to terminate</summary>
        private static bool terminateThread = false;

        /// <summary>Event to wait on for processing interval</summary>
        private static AutoResetEvent msgWaitEvent = new AutoResetEvent(false);

        /// <summary>Threshold where the messages get logged even if timeout not expired</summary>
        private static int msgNumberThreshold = 100;

        /// <summary>Alows OS specific parsing of stack for warning and up class and method names</summary>
        private static IStackTools stackTools = null;

        #endregion

        #region Events

        /// <summary>
        /// The event that is raised on each message logged that is equal or greater
        /// than the set verbosity.
        /// </summary>
        public static event LogingMsgEventDelegate OnLogMsgEvent;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        static Log() {
            Log.pushThread.Start();
        }

        #endregion

        #region Info Calls

        /// <summary>
        /// Log an Info level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public static void Info(string atClass, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Info, 0, atClass, atMethod, msg);
        }


        /// <summary>
        /// Log a high performance Info level message with no message formating invoked unless logged
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public static void Info(string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Info, 0, atClass, atMethod, msgFunc);
        }


        /// <summary>
        /// Log a method entry Info level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        public static void InfoEntry(string atClass, string atMethod) {
            Log.LogMsg(MsgLevel.Info, 0, atClass, atMethod, "Entry");
        }


        /// <summary>
        /// Log a method exit Info level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        public static void InfoExit(string atClass, string atMethod) {
            Log.LogMsg(MsgLevel.Info, 0, atClass, atMethod, "Exit");
        }

        #endregion

        #region Debug Calls

        /// <summary>
        /// Log a Debug level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public static void Debug(string atClass, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Debug, 0, atClass, atMethod, msg);
        }


        /// <summary>
        /// Log a high performance Debug level message with no message formating invoked unless logged
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public static void Debug(string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Debug, 0, atClass, atMethod, msgFunc);
        }


        /// <summary>
        /// Log a method entry Debug level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        public static void DebugEntry(string atClass, string atMethod) {
            Log.LogMsg(MsgLevel.Debug, 0, atClass, atMethod, "Entry");
        }


        /// <summary>
        /// Log a method exit Debug level message
        /// </summary>
        /// <param name="atClass">The class of origine</param>
        /// <param name="atMethod">The method of origine</param>
        public static void DebugExit(string atClass, string atMethod) {
            Log.LogMsg(MsgLevel.Debug, 0, atClass, atMethod, "Exit");
        }

        #endregion

        #region Warning to Exception Calls

        /// <summary>
        /// Log a warning level message
        /// </summary>
        /// <param name="code">warning number code</param>
        /// <param name="msg">The message to log</param>
        public static void Warning(int code, string msg) {
            Log.LogWarningAndUp(MsgLevel.Warning, code, msg);
        }

        public static void Warning(int code, string atClass, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Warning, code, atClass, atMethod, msg);
        }


        /// <summary>
        /// Log an Error level message
        /// </summary>
        /// <param name="code">error number code</param>
        /// <param name="msg">The message to log</param>
        public static void Error(int code, string msg) {
            Log.LogWarningAndUp(MsgLevel.Error, code, msg);
        }


        public static void Error(int code, string atClass, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Error, code, atClass, atMethod, msg);
        }


        /// <summary>
        /// Log a Critical level message
        /// </summary>
        /// <param name="code">error number code</param>
        /// <param name="msg">The message to log</param>
        public static void Critical(int code, string msg) {
            Log.LogWarningAndUp(MsgLevel.Critical, code, msg);
        }


        public static void Critical(int code, string atClass, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Critical, code, atClass, atMethod, msg);
        }


        /// <summary>
        /// Log an exception level message
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="msg">The message to log</param>
        /// <param name="e">The exception to parse for information</param>
        public static void Exception(int code, string msg, Exception e) {
            Log.LogWarningAndUp(MsgLevel.Exception, code, msg, e);
        }


        public static void Exception(int code, string atClass, string atMethod, Exception e) {
            string msg = (e is ErrReportException) ? (e as ErrReportException).Report.Msg : e.Message;
            //Log.LogMsg(MsgLevel.Exception, code, atClass, atMethod, e.Message, e);
            Log.LogMsg(MsgLevel.Exception, code, atClass, atMethod, msg, e);
        }

        public static void Exception(int code, string atClass, string atMethod, string msg, Exception e) {
            Log.LogMsg(MsgLevel.Exception, code, atClass, atMethod, msg, e);
        }


        #region High Performance message format defered

        /// <summary>
        /// Log a warning level message with defered message formating
        /// </summary>
        /// <param name="code">warning number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public static void Warning(int code, Func<string> msgFunc) {
            Log.LogWarningAndUp(MsgLevel.Warning, code, msgFunc);
        }


        public static void Warning(int code, string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Warning, code, atClass, atMethod, msgFunc);
        }


        /// <summary>
        /// Log an Error level message with defered message formating
        /// </summary>
        /// <param name="code">error number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public static void Error(int code, Func<string> msgFunc) {
            Log.LogWarningAndUp(MsgLevel.Error, code, msgFunc);
        }


        public static void Error(int code, string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Error, code, atClass, atMethod, msgFunc);
        }


        /// <summary>
        /// Log a Critical level message with defered message formating
        /// </summary>
        /// <param name="code">error number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public static void Critical(int code, Func<string> msgFunc) {
            Log.LogWarningAndUp(MsgLevel.Critical, code, msgFunc);
        }

        public static void Critical(int code, string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Critical, code, atClass, atMethod, msgFunc);
        }


        /// <summary>
        /// Log an exception level message with defered message formating
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        /// <param name="e">The exception to parse for information</param>
        public static void Exception(int code, Func<string> msgFunc, Exception e) {
            Log.LogWarningAndUp(MsgLevel.Exception, code, msgFunc, e);
        }


        public static void Exception(int code, string atClass, string atMethod, Func<string> msgFunc, Exception e) {
            Log.LogMsg(MsgLevel.Exception, code, atClass, atMethod, msgFunc, e);
        }

        #endregion

        #endregion

        #region LogMsg

        /// <summary>
        /// Log the message
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message</param>
        public static void LogMsg(MsgLevel level, int code, string atClass, string atMethod, string msg) {
            Log.LogMsg(level, code, atClass, atMethod, msg, null);
        }

        #region High Performance message format defered

        /// <summary>
        /// Log the message using a function that defers execution of message formating unless at the 
        /// correct level to log
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message function</param>
        public static void LogMsg(MsgLevel level, int code, string atClass, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(level, code, atClass, atMethod, msgFunc, null);
        }


        /// <summary>
        /// Log the message using a function that defers execution of message formating unless at the 
        /// correct level to log
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message function</param>
        /// <param name="e">Exception to log</param>
        public static void LogMsg(MsgLevel level, int code, string atClass, string atMethod, Func<string> msgFunc, Exception e) {
            // You must only invoke the formater function if the level is high enough to ensure maximum
            // performance gains if the message is not high enough level to be logged
            if (Log.IsVerboseEnough(level)) {
                string msg = "";
                try {
                    msg = msgFunc.Invoke();
                }
                catch (Exception ex) {
                    msg = String.Format(
                        "Exception Thrown on invoking Msg Formater for Msg Number:{0} at {1}.{2} - {3}",
                        code, atClass, atMethod, ex.Message);
                    System.Diagnostics.Debug.WriteLine(msg);
                }
                Log.LogMsg(level, code, atClass, atMethod, msg, e);
            }
        }

        #endregion

        /// <summary>
        /// Log the message
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Class of origine</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message</param>
        /// <param name="e">Exception to log</param>
        public static void LogMsg(MsgLevel level, int code, string atClass, string atMethod, string msg, Exception e) {
            Log.LogMsg(level, new ErrReport(code, atClass, atMethod, msg, e));
        }


        /// <summary>
        /// Log the message
        /// </summary>
        /// <param name="level">The message level</param>
        /// <param name="msg">The message object</param>
        public static void LogMsg(MsgLevel level, ErrReport msg) {
            if (Log.IsVerboseEnough(level)) {
                lock (Log.msgQLock) {
                    Log.messages.Add(new KeyValuePair<MsgLevel, ErrReport>(level, msg));
                    if (Log.messages.Count >= Log.msgNumberThreshold) {
                        Log.msgWaitEvent.Set();
                    }
                }
            }
        }


        #endregion

        #region Other public Methods

        /// <summary>
        /// Conforms the the ChkUtils LogingMsgDelegate that can be passed into the 
        /// wrappers to push information to a logger when an exception is caught
        /// </summary>
        /// <param name="report">The object with the error information</param>
        public static void LogExceptionDelegate(ErrReport report) {
            Log.LogMsg(MsgLevel.Exception, report);
        }


        /// <summary>
        /// Set the minimum verbosity that the message need for logging
        /// </summary>
        /// <param name="minimumLevelForLogging">The minimum level required</param>
        public static void SetVerbosity(MsgLevel minimumLevelForLogging) {
            lock (Log.levelLock) {
                Log.verbosity = minimumLevelForLogging;
            }
        }


        /// <summary>Get the minimum verbosity that the message need for logging</summary>
        /// <return>The current logging verbosity</param>
        public static MsgLevel GetVerbosity() {
            lock (Log.levelLock) {
                return Log.verbosity;
            }
        }


        public static void SetMsgNumberThreshold(int number) {
            Log.msgNumberThreshold = number;
        }

        public static void Terminate() {
            Log.terminateThread = true;

            Log.msgNumberThreshold = 1;
        }

        /// <summary>Allows the log calls to derive class and method names as per OS</summary>
        /// <param name="stackTools">The stack tools to inject</param>
        public static void SetStackTools(IStackTools stackTools) {
            Log.stackTools = stackTools;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves the class and method names by reflection. Used for messages from warning to 
        /// Exception level with occur infrequently
        /// </summary>
        /// <param name="level">Log message level</param>
        /// <param name="code">Error code</param>
        /// <param name="msg">Error message</param>
        private static void LogWarningAndUp(MsgLevel level, int code, string msg) {
            LogWarningAndUp(level, code, msg, null);
        }


        /// <summary>
        /// Retrieves the class and method names by reflection. Used for messages from warning to 
        /// Exception level with occur infrequently
        /// </summary>
        /// <param name="level">Log message level</param>
        /// <param name="code">Error code</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">The exception to parse for information</param>
        private static void LogWarningAndUp(MsgLevel level, int code, string msg, Exception e) {
            // Do the verbosity check first to avoid the overhead of reflection if it is not being logged
            if (Log.IsVerboseEnough(level)) {
                if (Log.stackTools != null) {
                    try {
                        ErrorLocation location = Log.stackTools.FirstNonWrappedMethod(new Type[] {
                            typeof(LogUtils.Net.Log),
                            typeof(ChkUtils.Net.WrapErr),
                            typeof(ErrReport),
                            typeof(ClassLog),
                        });
                        Log.LogMsg(level, code, location.ClassName, location.MethodName, msg, e);
                    }
                    catch (Exception e2) {
                        Log.LogMsg(MsgLevel.Error, 0, "Log", "LogWarningAndUp(479)", "EXCEPTION GETTING LOCATION", e2);
                        Log.LogMsg(level, code, "NAClass", "NAMethod", msg, e);
                    }
                }
                else {
                    Log.LogMsg(MsgLevel.Error, 0, "Log", "LogWarningAndUp(484)", "NO STACK TRACE TOOLS", e);
                    Log.LogMsg(level, code, "NAClass", "NAMethod", msg, e);
                }
            }
        }

        #region High Performance message format defered

        /// <summary>
        /// Retrieves the class and method names by reflection. Used for messages from warning to 
        /// Exception level with occur infrequently. The message is delivered by delegate for 
        /// better performance when using formated error string.
        /// </summary>
        /// <param name="level">Log message level</param>
        /// <param name="code">Error code</param>
        /// <param name="msg">Error message</param>
        private static void LogWarningAndUp(MsgLevel level, int code, Func<string> msgFunc) {
            LogWarningAndUp(level, code, msgFunc, null);
        }


        /// <summary>
        /// Retrieves the class and method names by reflection. Used for messages from warning to 
        /// Exception level with occur infrequently. The message is delivered by delegate for better
        /// performance when using formated error string.
        /// </summary>
        /// <param name="level">Log message level</param>
        /// <param name="code">Error code</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">The exception to parse for information</param>
        private static void LogWarningAndUp(MsgLevel level, int code, Func<string> msgFunc, Exception e) {
            // Do the verbosity check first to avoid the overhead of reflection if it is not being logged
            if (Log.IsVerboseEnough(level)) {
                string msg = "";
                try {
                    msg = msgFunc.Invoke();
                }
                catch (Exception ex) {
                    msg = String.Format(
                        "Exception Thrown on invoking Msg Formater for Msg Number:{0} - {1}", code, ex.Message);
                    System.Diagnostics.Debug.WriteLine(msg);
                }

                if (Log.stackTools != null) {
                    try {
                        ErrorLocation location = Log.stackTools.FirstNonWrappedMethod(new Type[] {
                            typeof(LogUtils.Net.Log),
                            typeof(ChkUtils.Net.WrapErr),
                            typeof(ErrReport),
                        });
                        Log.LogMsg(level, code, location.ClassName, location.MethodName, msg, e);
                    }
                    catch (Exception e2) {
                        Log.LogMsg(MsgLevel.Error, 0, "Log", "LogWarningAndUp", "EXCEPTION GETTING LOCATION", e2);
                        Log.LogMsg(level, code, "NAClass", "NAMethod", msg, e);
                    }
                }
                else {
                    Log.LogMsg(MsgLevel.Error, 0, "Log", "LogWarningAndUp", "NO STACK TRACE TOOLS", e);
                    Log.LogMsg(level, code, "NAClass", "NAMethod", msg, e);
                }
            }
        }

        #endregion

        /// <summary>
        /// Checks if the message level is equal or greater than the set verbosity
        /// </summary>
        /// <param name="msgLevel">The message level</param>
        /// <returns></returns>
        private static bool IsVerboseEnough(MsgLevel msgLevel) {
            // Initial check for illegal usage of Off for a message level
            if (msgLevel == MsgLevel.Off) {
                return false;
            }
            return msgLevel.GreaterOrEqual(Log.verbosity);
        }


        /// <summary>
        /// The logging thread. Will take a copy of all collected messages and log them in a 
        /// separate thread
        /// </summary>
        /// <remarks>
        /// Wakes up either on a specified time interval or immediately if accumulated message
        /// threshold is attained
        /// </remarks>
        private static void MsgPushThread() {
            // New way. Terminate only at end of current push
            List<KeyValuePair<MsgLevel, ErrReport>> messageSet = new List<KeyValuePair<MsgLevel, ErrReport>>();
            while (!Log.terminateThread) {
                Log.msgWaitEvent.WaitOne(500);
                lock (Log.msgQLock) {
                    if (Log.messages.Count > 0) {
                        messageSet = Log.messages.GetRange(0, Log.messages.Count);
                        Log.messages.Clear();
                    }
                }

                foreach (KeyValuePair<MsgLevel, ErrReport> msg in messageSet) {
                    Log.RaiseEvent(msg.Key, msg.Value);
                }
                messageSet.Clear();
            }
        }


        /// <summary>
        /// Push the log message to the log message event subscribers
        /// </summary>
        /// <param name="eventData"></param>
        private static void RaiseEvent(MsgLevel level, ErrReport msg) {
            if (Log.OnLogMsgEvent != null) {
                WrapErr.SafeAction(() => Log.OnLogMsgEvent(level, msg));
            }
            else {
                System.Diagnostics.Debug.WriteLine("No subscribers to log message event");
            }
        }

        #endregion

    }
}
