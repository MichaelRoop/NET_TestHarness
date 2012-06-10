using System;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Default exception parser
    /// </summary>
    /// <author>Michael Roop</author>
    public class DefaultExceptionParser : ExceptionParserBase {

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">The Exception to parse</param>
        public DefaultExceptionParser(Exception e)
            : base(e) {
        }

        #endregion

        #region Overrides

        protected override void AddExtraInfo(Exception e) {
            // Default one will not attempt to get any extra info
        }

        #endregion
    }
}
