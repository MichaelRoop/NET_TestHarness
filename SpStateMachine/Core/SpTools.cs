using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace SpStateMachine.Core {

    /// <summary>
    /// Collection of tools useful for factoring out common functionality
    /// </summary>
    public static class SpTools {
        
        /// <summary>
        /// Search through a dictionary for a string equivalent and add if not found
        /// </summary>
        /// <param name="key">The key for the string requested</param>
        /// <param name="currentStrings">The dictionary of strings to query and add to</param>
        /// <param name="converterFunc">The converted to convert the key to a string value if not in the Dictionary</param>
        /// <returns></returns>
        public static string GetIdString(int key, Dictionary<int, string> currentStrings, Func<int, string> converterFunc) {
            WrapErr.ChkParam(currentStrings, "currentStrings", 9999);
            WrapErr.ChkParam(converterFunc, "converterFunc", 9999);
            return WrapErr.ToErrorReportException(9999, () => {
                if (currentStrings.Keys.Contains(key)) {
                    return currentStrings[key];
                }
                string val = converterFunc.Invoke(key);
                currentStrings.Add(key, val);
                return val;
            });
        }


    }
}
