using ChkUtils.Net.ErrObjects;
using System;

namespace LogUtils.Net {

    /// <summary>Log wrapper to use in one class</summary>
    public class ClassLog {

        #region Data

        private string loggedClass = "NAClass";

        #endregion

        #region Constructors

        public ClassLog(string className) {
            this.loggedClass = string.IsNullOrWhiteSpace(className) ? "_invalid_" : className;
        }

        #endregion

        #region Info Calls

        /// <summary>Log an Info level message</summary>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public void Info(string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Info, 0, this.loggedClass, atMethod, msg);
        }


        /// <summary>Log a high performance Info level message with no message formating invoked unless logged</summary>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public void Info(string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Info, 0, this.loggedClass, atMethod, msgFunc);
        }


        /// <summary>Log a method entry Info level message</summary>
        /// <param name="atMethod">The method of origine</param>
        public void InfoEntry(string atMethod) {
            Log.LogMsg(MsgLevel.Info, 0, this.loggedClass, atMethod, "Entry");
        }


        /// <summary>Log a method exit Info level message</summary>
        /// <param name="atMethod">The method of origine</param>
        public void InfoExit(string atMethod) {
            Log.LogMsg(MsgLevel.Info, 0, this.loggedClass, atMethod, "Exit");
        }

        #endregion

        #region Debug Calls

        /// <summary>Log a Debug level message</summary>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public void Debug(string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Debug, 0, this.loggedClass, atMethod, msg);
        }


        /// <summary>Log a high performance Debug level message with no message formating invoked unless logged</summary>
        /// <param name="atMethod">The method of origine</param>
        /// <param name="msg">The message to log</param>
        public void Debug(string atMethod, Func<string> msgFunc) {
            Log.LogMsg(MsgLevel.Debug, 0, this.loggedClass, atMethod, msgFunc);
        }


        /// <summary>Log a method entry Debug level message</summary>
        /// <param name="atMethod">The method of origine</param>
        public void DebugEntry(string atMethod) {
            Log.LogMsg(MsgLevel.Debug, 0, this.loggedClass, atMethod, "Entry");
        }


        /// <summary>Log a method exit Debug level message</summary>
        /// <param name="atMethod">The method of origine</param>
        public void DebugExit(string atMethod) {
            Log.LogMsg(MsgLevel.Debug, 0, this.loggedClass, atMethod, "Exit");
        }

        #endregion

        #region Warning

        /// <summary>Log a warning level message</summary>
        /// <param name="code">warning number code</param>
        /// <param name="msg">The message to log</param>
        public void Warning(int code, string msg) {
            Log.Warning(code, msg);
        }

        public void Warning(int code, string atMethod, string msg) {
            Log.Warning(code, this.loggedClass, atMethod, msg);
        }

        /// <summary>Log a warning level message with defered message formating</summary>
        /// <param name="code">warning number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public void Warning(int code, Func<string> msgFunc) {
            Log.Warning(code, msgFunc);
        }


        public void Warning(int code, string atMethod, Func<string> msgFunc) {
            Log.Warning(code, this.loggedClass, atMethod, msgFunc);
        }

        #endregion

        #region Error

        /// <summary>Log an Error level message</summary>
        /// <param name="code">error number code</param>
        /// <param name="msg">The message to log</param>
        public void Error(int code, string msg) {
            Log.Error(code, msg);
        }


        public void Error(int code, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Error, code, this.loggedClass, atMethod, msg);
        }

        /// <summary>Log an Error level message with defered message formating</summary>
        /// <param name="code">error number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public void Error(int code, Func<string> msgFunc) {
            Log.Error(code, msgFunc);
        }


        public void Error(int code, string atMethod, Func<string> msgFunc) {
            Log.Error(code, this.loggedClass, atMethod, msgFunc);
        }

        #endregion

        #region Critical

        /// <summary>Log a Critical level message</summary>
        /// <param name="code">error number code</param>
        /// <param name="msg">The message to log</param>
        public void Critical(int code, string msg) {
            Log.Critical(code, msg);
        }


        public void Critical(int code, string atMethod, string msg) {
            Log.LogMsg(MsgLevel.Critical, code, this.loggedClass, atMethod, msg);
        }


        /// <summary>Log a Critical level message with defered message formating</summary>
        /// <param name="code">error number code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        public void Critical(int code, Func<string> msgFunc) {
            Log.Critical(code, msgFunc);
        }


        public void Critical(int code, string atMethod, Func<string> msgFunc) {
            Log.Critical(code, this.loggedClass, atMethod, msgFunc);
        }

        #endregion

        #region Exception

        /// <summary>Log an exception level message</summary>
        /// <param name="code">Error code</param>
        /// <param name="msg">The message to log</param>
        /// <param name="e">The exception to parse for information</param>
        public void Exception(int code, string msg, Exception e) {
            Log.Exception(code, msg, e);
        }


        //public void Exception(int code, string atMethod, Exception e) {
        //    Log.Exception(code, this.loggedClass, atMethod, e);
        //}

        public void Exception(int code, string atMethod, string msg, Exception e) {
            Log.Exception(code, this.loggedClass, atMethod, msg, e);
        }


        /// <summary>
        /// Log an exception level message with defered message formating
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="msgFunc">The message formater method to produce log string</param>
        /// <param name="e">The exception to parse for information</param>
        public void Exception(int code, Func<string> msgFunc, Exception e) {
            Log.Exception(code, msgFunc, e);
        }


        public void Exception(int code, string atMethod, Func<string> msgFunc, Exception e) {
            Log.Exception(code, this.loggedClass, atMethod, msgFunc, e);
        }

        #endregion

        #region LogMsg

        /// <summary>Log the message</summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message</param>
        public void LogMsg(MsgLevel level, int code, string atMethod, string msg) {
            Log.LogMsg(level, code, this.loggedClass, atMethod, msg);
        }


        /// <summary>Log using defered execution of message formating unless at thecorrect level to log</summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message function</param>
        public void LogMsg(MsgLevel level, int code, string atMethod, Func<string> msgFunc) {
            Log.LogMsg(level, code, this.loggedClass, atMethod, msgFunc);
        }


        /// <summary>Log using function that defers execution of message formating unless at the correct level to log</summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message function</param>
        /// <param name="e">Exception to log</param>
        public void LogMsg(MsgLevel level, int code, string atMethod, Func<string> msgFunc, Exception e) {
            Log.LogMsg(level, code, this.loggedClass, atMethod, msgFunc, e);
        }


        /// <summary>Log the message</summary>
        /// <param name="level">Message level</param>
        /// <param name="code">Error code</param>
        /// <param name="atMethod">Method of origine</param>
        /// <param name="msg">Message</param>
        /// <param name="e">Exception to log</param>
        public void LogMsg(MsgLevel level, int code, string atMethod, string msg, Exception e) {
            Log.LogMsg(level, code, this.loggedClass, atMethod, msg, e);
        }


        /// <summary>Log the message</summary>
        /// <param name="level">The message level</param>
        /// <param name="msg">The message object</param>
        public void LogMsg(MsgLevel level, ErrReport msg) {
            Log.LogMsg(level, msg);
        }

        #endregion

    }
}
