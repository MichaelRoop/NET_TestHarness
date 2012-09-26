using SpStateMachine.Interfaces;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Definition of a class to wrap Int to Enum conversion. In this
    /// way you can pass the embedded Extension converters as an interface
    /// parameter
    /// </summary>
    /// <typeparam name="T">The type of Enum to convert</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpIntToEnum<T> : ISpToEnum<T> where T : struct {

        #region Data

        /// <summary>The integer value preserved from construtor</summary>
        private int intVal = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpIntToEnum() { 
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="intVal">The held int value to convert to an enum</param>
        public SpIntToEnum(int intVal) { 
            this.intVal = intVal; 
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert held int to Enum of type T
        /// </summary>
        /// <returns>Returns a type T enum</returns>
        public T ToEnum() {
            return SpConverter.IntToEnum<T>(this.intVal);
        }

        #endregion
    }

}
