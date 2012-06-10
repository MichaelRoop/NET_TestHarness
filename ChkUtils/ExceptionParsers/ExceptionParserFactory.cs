using System;
using System.Diagnostics;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Factory to deliver specific IExceptionParser objects according to exception type
    /// </summary>
    /// <author>Michael Roop</author>
    public static class ExceptionParserFactory {

        /// <summary>
        /// Retrieve the appropriate parser by Exception type
        /// </summary>
        /// <param name="e">The exception that requires a parser</param>
        /// <returns>An exception parser</returns>
        public static IExceptionParser Get(Exception e) {
            if (e == null) {
                Debug.WriteLine("ExceptionParserBase.AddStackFrames:Attempting to add stack frames from a null exception");
            }

            if (e.GetType() == typeof(Exception)) {
                return new DefaultExceptionParser(e);
            }
            return new DefaultExceptionParser(e);
        }


    }

}

