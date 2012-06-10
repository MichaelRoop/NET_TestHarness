﻿using System;
using System.Diagnostics;
using System.Reflection;

namespace ChkUtils {

    /// <summary>
    /// Partial WrapErr class implementation with common utilities
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {
        
        #region Safe Action Methods

        /// <summary>
        /// Safely executes an action. All exceptions caught and ignored
        /// </summary>
        /// <param name="action">The action to invoke</param>
        public static void SafeAction (Action action) {
            try {
                action.Invoke();
            }
            catch (Exception e) {
                Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
                // At this point we do not want to report on any error back to the application
            }
        }


        /// <summary>
        /// Safely executes a function that returns a value . All exceptions caught and ignored. If an exception
        /// is thrown, the default for the type T will be returned so it has to be checked
        /// </summary>
        /// <typeparam name="T">The function return type</typeparam>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public static T SafeAction<T>(Func<T> func) {
            try {
                return func.Invoke();
            }
            catch (Exception e) {
                Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
                // At this point we do not want to report on any error back to the application
                return default(T);
            }
        }


        /// <summary>
        /// Safely executes a function that returns a value . All exceptions caught and ignored. If an exception
        /// is thrown, the default for the type T will be returned so it has to be checked
        /// </summary>
        /// <typeparam name="T">The function return type</typeparam>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public static string SafeAction(Func<string> func) {
            try {
                return func.Invoke();
            }
            catch (Exception e) {
                Debug.WriteLine("{0} on call to WrapErr.SafeAction:{1} - {2}", e.GetType().Name, e.Message, e.StackTrace);
                // At this point we do not want to report on any error back to the application
                return "Exception trying to format error string:" + e.Message;
            }
        }



        #endregion

        #region Stack Frame Methods

        /// <summary>
        /// Get the file name with the path stripped off from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The file name</returns>
        public static string FileName(StackFrame frame) {
            if (frame == null) {
                Debug.WriteLine("WrapErr.GetFileName : Null frame");
                return "NoFileName";
            }
            
            string name = frame.GetFileName();
            if (name == null) {
                Debug.WriteLine("WrapErr.GetFileName : Null name in frame");
                return "NoFileName";
            }

            // Strip off the path
            if (name.Length > 0) {
                int pos = name.LastIndexOf('\\');
                if (name.Length > 0 && pos != -1 && pos < (name.Length - 1)) {
                    name = name.Substring(pos + 1);
                }
            }
            return name;
        }


        /// <summary>
        /// Get the method name from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The method name</returns>
        public static string MethodName(StackFrame frame) {
            try {
                return frame.GetMethod().Name;
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetMethodName : Exception {0} getting method name:{1}", e.GetType().Name, e.Message));
                return "NoMethodName";
            }
        }


        /// <summary>
        /// Get the line number from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The method name</returns>
        public static int Line(StackFrame frame) {
            try {
                return frame.GetFileLineNumber();
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetLine : Exception {0} getting line number:{1}", e.GetType().Name, e.Message));
                return 0;
            }
        }
        

        /// <summary>
        /// Get the class name from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The class name</returns>
        public static string ClassName(StackFrame frame) {
            try {
                return frame.GetMethod().DeclaringType.Name;
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetClassName : Exception {0} getting class name:{1}", e.GetType().Name, e.Message));
                return "NoClassName";
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Helper to create an error report object with the class and method names of calling method that
        /// had an exeption occur. Names are found by relection
        /// </summary>
        /// <param name="code">Unique error code</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">Exception of origine</param>
        /// <returns>And error report object</returns>
        private static ErrReport GetErrReport(int code, string msg, Exception e) {
            try {
                // Always needs to be called directly from one of the wrap objects to get the right number of the stack frame
                // This will give us the class and method names in which the wrap method is used
                MethodBase method = new StackTrace().GetFrame(2).GetMethod();
                return new ErrReport(code, method.DeclaringType.Name, method.Name, msg, e);
            }
            catch (Exception ex) {
                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                return new ErrReport(code, "UnknownClass", "UnknownMethod", msg, e);
            }
        }

        #endregion
        
    }
}
