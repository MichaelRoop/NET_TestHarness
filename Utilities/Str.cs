using System;

namespace Ca.Roop.Utilities {

class Str {

    /// <summary>
    /// Does a safe call to an object's ToString method. If the object is null, "null" is 
    /// returned as a string.
    /// </summary>
    /// <param name="o">The object on which to test and call ToString.</param>
    /// <returns>The value of Ojbect.ToString if object is valid, otherwise "null".</returns>
    public static String SafeToString(Object o) {
        if (o == null) {
            return "null";
        }
        return o.ToString();
    }


    /// <summary>
    /// Convert a string to an int with a more suitable exception. Zero length string 
    /// is returned as 0.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The string converted into an int</returns>
    /// <exception cref="Exception">If the string is not convertable.</exception>
    public static int ConvertToInt(String str) {
        if (str.Length > 0) {
            try {
                return Convert.ToInt32(str);
            }
            catch (FormatException e) {
                throw new Exception("Invalid number:" + str, e);
            }
        }
        return 0;
    }



}

}
