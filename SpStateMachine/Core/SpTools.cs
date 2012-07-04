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
            WrapErr.ChkParam(currentStrings, "currentStrings", 51000);
            WrapErr.ChkParam(converterFunc, "converterFunc", 51001);
            return WrapErr.ToErrorReportException(51002, () => {
                if (currentStrings.Keys.Contains(key)) {
                    return currentStrings[key];
                }

                // Do another wrap level to isolate the user defined conversion failure
                string ret =  WrapErr.ToErrorReportException(51003, "Error in Calling Id to String Converter Method", () => {
                    return converterFunc.Invoke(key);
                });
                currentStrings.Add(key, ret);
                return ret;
            });
        }


    }
}
