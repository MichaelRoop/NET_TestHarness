using System;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Central parser for displayable common exception iformation
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class ExceptionInfo {

        #region Data

        private string name = "";

        private string msg = "";

        #endregion

        #region Properties

        /// <summary>
        /// The exception type name
        /// </summary>
        public string Name {
            get {
                return this.name;
            }
        }

        /// <summary>
        /// The exception message
        /// </summary>
        public string Msg {
            get {
                return this.msg;
            }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private ExceptionInfo() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">The exception to parse out information</param>
        public ExceptionInfo(Exception e) {
            this.name = e.GetType().Name;
            this.msg = e.Message;
        }
        
        #endregion

    }

}
