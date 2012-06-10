using System.Collections.Generic;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Interface for an execption parser that can be specialised to parse
    /// multipley types of exceptions
    /// </summary>
    /// <author>Michael Roop</author>
    public interface IExceptionParser {
        
        /// <summary>
        /// Retrieve the Exception Info object with basic information
        /// </summary>
        /// <returns>The ExceptionInfo object</returns>
        ExceptionInfo GetInfo();

        /// <summary>
        /// Retrieve a list of extra info objects for specialised exeptions
        /// </summary>
        /// <returns>A list of ExceptionExtraInfo objects</returns>
        List<ExceptionExtraInfo> GetExtraInfoInfo();


        /// <summary>
        /// Retrieve a list of strings representing the frames of a stack trace
        /// </summary>
        /// <param name="reversed">true if you want the order reversed to exception origine first</param>
        /// <returns>A list of strings with the stack frame information</returns>
        List<string> GetStackFrames(bool reversed);

    }
}
