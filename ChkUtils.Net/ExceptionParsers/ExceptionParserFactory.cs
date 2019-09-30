using System;
using System.Diagnostics;
using System.Xml;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Factory to deliver specific IExceptionParser objects according to exception type
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class ExceptionParserFactory {

        /// <summary>
        /// Retrieve the appropriate parser by Exception type
        /// </summary>
        /// <param name="e">The exception that requires a parser</param>
        /// <returns>An exception parser</returns>
        public static IExceptionParser Get(Exception e) {
            if (e == null) {
                //Debug.WriteLine("ExceptionParserBase.AddStackFrames:Attempting to add stack frames from a null exception");
                //return new DefaultExceptionParser(new Exception("The Exception passed in to the exception parser factory is null"));
                return null;
            }

            if (e.GetType() == typeof(XmlException)) {
                return new XmlExceptionParser((XmlException)e);
            }
            else if (e.GetType() == typeof(Exception)) {
                return new DefaultExceptionParser(e);
            }
            return new DefaultExceptionParser(e);
        }


    }

}

