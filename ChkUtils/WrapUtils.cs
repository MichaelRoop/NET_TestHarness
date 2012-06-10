using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ChkUtils {

    /// <summary>
    /// Partial WrapErr class implementation with common utilities
    /// </summary>
    public static partial class WrapErr {

        /// <summary>
        /// Helper to create an error report object with the class and method names of calling method that
        /// had an exeption occur. Names are found by relection
        /// </summary>
        /// <param name="code">Unique error code</param>
        /// <param name="msg">Error message</param>
        /// <param name="e">Exception of origine</param>
        /// <returns>And error report object</returns>
        private static ErrReport GetErrReport (int code, string msg, Exception e) {
            try {
                // Always needs to be called directly from one of the wrap objects to get the right number of the stack frame
                MethodBase method = new StackTrace().GetFrame(2).GetMethod();
                return new ErrReport(code, method.DeclaringType.Name, method.Name, msg, e);
            }
            catch (Exception ex) {
                Debug.WriteLine("{0} on call to WrapErr.GetErrReport:{1} - {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                return new ErrReport(code, "UnknownClass", "UnknownMethod", msg, e);
            }
        }
        

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







    }
}
