using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Utils;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Definition of a class to wrap Int to Enum conversion. In this
    /// way you can pass the embedded Extension converters as an interface
    /// parameter
    /// </summary>
    /// <typeparam name="T">The type of Enum to convert</typeparam>
    public class SpToEnum<T> : ISpToEnum<T> where T : struct {

        #region Data

        /// <summary>The integer value preserved from construtor</summary>
        private int intVal = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpToEnum() { 
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="intVal">The held int value to convert to an enum</param>
        public SpToEnum(int intVal) { 
            this.intVal = intVal; 
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert to Enum of type T
        /// </summary>
        /// <returns>Returns a type T enum</returns>
        public T ToEnum() {
            //return SpEnumConverterExtensions.ToEnum<T>(this.intVal);
            //return SpToEnum<T>.ToEnum(this.intVal);
            //return this.intVal.ToEnum<T>();

            return this.ToEnum(this.intVal);
        }


        /// <summary>
        /// Convert an int value into an enum
        /// </summary>
        /// <param name="id">The int value to convert</param>
        /// <returns>An enum</returns>
        public T ToEnum(int id) {
            return id.ToEnum<T>();
            //return SpEnumConverterExtensions.ToEnum<T>(id);
        }


        #endregion

        #region Public Static Methods

        public static T ToAnyEnum(int id) {
            return id.ToEnum<T>();
            //return SpEnumConverterExtensions.ToEnum<T>(id);
        }

        #endregion
    }

}
