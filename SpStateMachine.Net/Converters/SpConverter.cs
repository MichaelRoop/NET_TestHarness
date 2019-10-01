using System;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Static converter using static Enum Extension Methods
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class SpConverter {


        /// <summary>
        /// Convert an int to an enum of type T
        /// </summary>
        /// <typeparam name="T">The Enum type</typeparam>
        /// <param name="id">The int id to convert to enum of type T</param>
        /// <returns>An enum of type T</returns>
        public static T IntToEnum<T>(int id) where T : struct {
            return id.ToEnum<T>();
        }


        /// <summary>
        /// Convert an enum to its int value
        /// </summary>
        /// <param name="value">The enum to convert to int</param>
        /// <returns>The corresponding int of the enum</returns>
        public static int EnumToInt(Enum value) {
            return value.ToInt();
        }


    }
}
