using System;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Definition of a class to wrap Enum to Int conversion. In this
    /// way you can pass the embedded Extension converters as an interface
    /// parameter
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpEnumToInt : ISpToInt {

        #region Data

        /// <summary>The enum passed in on construction
        /// 
        /// </summary>
        private Enum val;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpEnumToInt() { 
        }
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="val">The Enum to convert</param>
        public SpEnumToInt(Enum val) {
            this.val = val;
        }

        #endregion

        #region ISpToInt Members

        /// <summary>
        /// Convert the held Enum to Int
        /// </summary>
        /// <returns></returns>
        public int ToInt() {
            return SpConverter.EnumToInt(this.val);
        }

        #endregion
    }


}
