using System;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Default exception parser
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
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

        /// <summary>
        /// There is no further information fields
        /// </summary>
        /// <param name="e"></param>
        protected override void AddExtraInfo(Exception e) {
            // Default one will not attempt to get any extra info
        }

        #endregion
    }
}
