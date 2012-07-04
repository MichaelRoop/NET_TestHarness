using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface defining an int to Enum Converter
    /// </summary>
    /// <typeparam name="T">The type of Enum to convert</typeparam>
    public interface ISpToEnum<T> where T : struct {

        /// <summary>
        /// Convert to Enum of type T
        /// </summary>
        /// <returns>Returns a type T enum</returns>
        T ToEnum();


        /// <summary>
        /// Convert an int value into an enum
        /// </summary>
        /// <param name="id">The int value to convert</param>
        /// <returns>An enum</returns>
        T ToEnum(int id);

    }
}
