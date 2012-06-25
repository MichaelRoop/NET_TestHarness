using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ExceptionParsers;

namespace ChkUtils.ExceptionFormating {

    /// <summary>
    /// Formats the output of the exception to string
    /// </summary>
    public interface IExceptionOutputFormater {

        void FormatException(IExceptionParser parser, StringBuilder target);


    }
}
