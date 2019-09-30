using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ExceptionParsers;

namespace ChkUtils.ExceptionFormating {

    /// <summary>
    /// Formats the output of the exception to string
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface IExceptionOutputFormater {

        /// <summary>
        /// Use the IExceptionParser to parse out all of the exception and
        /// nested exceptions stack information to a string
        /// </summary>
        /// <param name="parser">The parser to break down the exception</param>
        /// <param name="target">The target string builder for the stack string</param>
        void FormatException(IExceptionParser parser, StringBuilder target);

    }
}
