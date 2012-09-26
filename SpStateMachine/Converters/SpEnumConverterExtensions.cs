using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Extension tools to enable developers to use enums for strong typing in generating
    /// and interpreting integer ids used in the State Machine Architechture.
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class SpEnumConverterExtensions {

        /// <summary>
        /// Convert an enum to its int value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value) {
            return WrapErr.ToErrorReportException(9999, "Unexpected Error Converting Enum to Int", () => {
                return Convert.ToInt32(value);
            });
        }


        /// <summary>
        /// Convert an integer to a generic enum
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The integer value to convert from</param>
        /// <returns>The Enum or an exception on failure to convert</returns>
        public static T ToEnum<T>(this int value) where T : struct {
            T enumType = default(T);
            T ret = 
            WrapErr.ToErrorReportException(9999,
                () => { return String.Format(
                    "Enum Conversion Failed Attempting to Convert to Type '{0}' with Value '{1}'", 
                    enumType.GetType().Name, value); }, 
                () => {
                    // This will throw on non enum but not out of range
                    return (T)Enum.Parse(typeof(T), value.ToString());
                });

            // Do the enum range check
            WrapErr.ChkTrue(Enum.IsDefined(typeof(T), ret), 9999, () => {
                return String.Format(
                    "Enum Conversion Out of Range Attempting to Convert to Type '{0}' with Value '{1}'",
                    enumType.GetType().Name, value);
            });
            return ret;
        }

    }
}
