using System;
using System.Collections.Generic;

namespace Ca.Roop.TestHarness.Core.Test {


/// <summary>
/// Abstracts the conversion of the metadata held in the TestArgument into an
/// object by presenting the correct converter based on a type name key.  This 
/// is the same type name that is loaded from the test script.
/// </summary>
public class ArgConverterFactory {

    private static readonly Dictionary<String, Func<String, String, object>>
        conversionMethods = new Dictionary<string, Func<String, String, object>> {
            {"byte", ParseToByte},
            {"sbyte", ParseToSByte},
            {"short", ParseToShort},
            {"ushort", ParseToUShort},
            {"int", ParseToInt},
            {"uint", ParseToUInt},
            {"long", ParseToLong},
            {"ulong", ParseToULong},
            {"float", ParseToFloat},
            {"double", ParseToDouble},
            {"decimal", ParseToDecimal},
            {"bool", ParseToBool},
            {"String", ParseToString},
            {"List", ParseToList}
        };


    /// <summary>
    /// Returns the appropriate converter for the type. The return is in the generic object format.
    /// </summary>
    /// <param name="key">The type name lookup key.</param>
    /// <returns>An converted type downcast to an object.</returns>
    /// <exception cref="ArgumentException">On invalid type name key.</exception>
    public static Func<String, String, object> GetConverter(String key) {
        Func<String, String, object> func;
        if (conversionMethods.TryGetValue(key, out func)) {
            return func;
        }
        throw new ArgumentException("Conversion from String to " + key + " not supported.");
    }


    // Series of adaptors to force the common signature on the conversion functions.
    private static object ParseToByte(String value, String Item) {
        return (object)Byte.Parse(value);
    }

    private static object ParseToSByte(String value, String Item) {
        return (object)SByte.Parse(value);
    }

    private static object ParseToShort(String value, String Item) {
        return (object)Int16.Parse(value);
    }

    private static object ParseToUShort(String value, String Item) {
        return (object)UInt16.Parse(value);
    }

    private static object ParseToInt(String value, String Item) {
        return (object)Int32.Parse(value);
    }

    private static object ParseToUInt(String value, String Item) {
        return (object)UInt32.Parse(value);
    }

    private static object ParseToLong(String value, String Item) {
        return (object)Int64.Parse(value);
    }

    private static object ParseToULong(String value, String Item) {
        return (object)UInt64.Parse(value);
    }

    private static object ParseToFloat(String value, String Item) {
        return (object)Single.Parse(value);
    }

    private static object ParseToDouble(String value, String Item) {
        return (object)Double.Parse(value);
    }

    private static object ParseToDecimal(String value, String Item) {
        return (object)Decimal.Parse(value);
    }

    private static object ParseToBool(String value, String Item) {
        return (object)Boolean.Parse(value);
    }

    private static object ParseToString(String value, String Item) {
        return (object)value;
    }

    private static object ParseToList(String value, String item) {
        return ArgListParser.Parse(value, item);
    }



}

}
