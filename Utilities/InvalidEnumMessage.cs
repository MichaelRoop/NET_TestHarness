using System;
using System.Text;

namespace Ca.Roop.Utilities {

/// <summary>
/// Automates the building of an error message for a non allowable string representation of
/// an enum.  After the error message it appends a list of allowable string 
/// representations for a particular enum type.
/// </summary>
class InvalidEnumMessage {

    /// <summary>
    /// An error message with the bad enum string and a list of allowabe enums in 
    /// format "Type1,Type2,Type3".
    /// </summary>
    /// <typeparam name="T">The enum type to list.</typeparam>
    /// <param name="invalid">The enum string that was rejected.</param>
    /// <returns>A built error message.</returns>
    public static String Get<T>(T invalid) {
        int i = 1;

        StringBuilder sb = new StringBuilder();
        sb.Append(invalid.GetType().Name)
            .Append(":")
            .Append(invalid.ToString())
            .Append(" not supported. ")
            .Append(" Allowed:");

        foreach (T t in System.Enum.GetValues(typeof(T))) {
            if (i++ > 1) {
                sb.Append(',');
            }
            sb.Append(t.ToString());
        }
        return sb.ToString();
    }

}









}
