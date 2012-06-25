using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Base class for Exception parsers to error reporting
    /// </summary>
    /// <author>Michael Roop</author>
    public abstract class ExceptionParserBase : IExceptionParser {

        #region Data

        private ExceptionInfo info = null;

        List<ExceptionExtraInfo> extraInfo = new List<ExceptionExtraInfo>();

        List<string> stackFrames = new List<string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private ExceptionParserBase() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">The Exception to parse</param>
        public ExceptionParserBase(Exception e) {
            this.info = new ExceptionInfo(e);
            this.AddExtraInfo(e);
            this.AddStackFrames(e);
        }

        #endregion

        #region IExceptionParser

        /// <summary>
        /// Retrieve the Exception Info object with basic information
        /// </summary>
        /// <returns>The ExceptionInfo object</returns>
        public ExceptionInfo GetInfo() {
            return this.info;
        }


        /// <summary>
        /// Retrieve a list of extra info objects for specialised exeptions
        /// </summary>
        /// <returns>A list of ExceptionExtraInfo objects</returns>
        public List<ExceptionExtraInfo> GetExtraInfoInfo() {
            return this.extraInfo;
        }


        /// <summary>
        /// Retrieve a list of strings representing the frames of a stack trace
        /// </summary>
        /// <param name="reversed">true if you want the order reversed to exception origine first</param>
        /// <returns>A list of strings with the stack frame information</returns>
        public List<string> GetStackFrames(bool reversed) {
            if (reversed) {
                // Clone it since we do not want to reverse the original
                List<string> tmp = new List<string>();
                this.stackFrames.ForEach(item => tmp.Add(item));
                tmp.Reverse();
                return tmp;
            }
            return this.stackFrames;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Add the extra info elements depending on the exception type
        /// </summary>
        /// <param name="e"></param>
        protected abstract void AddExtraInfo(Exception e);

        #endregion

        #region Private Methods

        /// <summary>
        /// Add a stack frames as a list of formatted strings
        /// </summary>
        /// <param name="e">The exception to parse</param>
        private void AddStackFrames(Exception e) {
            try {
                if (e == null) {
                    Debug.WriteLine("ExceptionParserBase.AddStackFrames:Attempting to add stack frames from a null exception");
                }
                else {
                    StackTrace trace = new StackTrace(e, true);
                    for (int i =0; i < trace.FrameCount; i++) {
                        StackFrame sf = trace.GetFrame(i);
                        this.stackFrames.Add(
                            String.Format("\t{0} : Line:{1} - {2}.{3}", StackFrameTools.FileName(sf), StackFrameTools.Line(sf), StackFrameTools.ClassName(sf), StackFrameTools.MethodName(sf)));
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(String.Format("ExceptionParserBase.AddStackFrames : Exception {0} :{1}", ex.GetType().Name, ex.Message));
            }
        }

        #endregion

    }
}
