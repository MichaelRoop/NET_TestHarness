
namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Contains key value of an extra piece of exception information for particular exceptions
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class ExceptionExtraInfo {

        #region Data

        private string name = "";

        private string value = "";

        #endregion

        #region Properties

        /// <summary>
        /// The extra exception value name
        /// </summary>
        public string Name {
            get { 
                return this.name; 
            }
        }

        /// <summary>
        /// The extra exception value
        /// </summary>
        public string Value {
            get {
                return this.value;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private ExceptionExtraInfo() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The exception extra value key name</param>
        /// <param name="value">The value</param>
        public ExceptionExtraInfo(string name, string value) {
            this.name = name;
            this.value = value;
        }

        #endregion
        
    }
}
