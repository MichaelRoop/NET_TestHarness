using System;
using System.Reflection;

namespace Ca.Roop.TestHarness.Core.Test {


/// <summary>
/// Parse out the value based on the string representation of the type and value as 
/// loaded from the test script.
/// </summary>
public class ArgParser {

    private const String ArgTypePropertyName        = "Type";
    private const String ArgValuePropertyName       = "Value";
    private const String ArgItemTypePropertyName    = "ItemType";


    /// <summary>
    /// Builds the object based on metadata loaded from the test script and held in the TestArg.
    /// </summary>
    /// <param name="testArg">The object that is a TestArg.</param>
    /// <returns>The object built from the TestArg metadata.</returns>
    /// <exception cref="ArgumentException">If the object passed in is not a TestArg.</exception>
    public static object GetValue(object testArg) {
        if (testArg is TestArg) {
            return ArgConverterFactory
                .GetConverter(GetPropertyStringValue(testArg, ArgTypePropertyName))
                    .Invoke(
                        GetPropertyStringValue(testArg, ArgValuePropertyName),
                        GetPropertyStringValue(testArg, ArgItemTypePropertyName)
                    );
        }
        throw new ArgumentException("Object "  + testArg.GetType().Name + " is not a TestArg");
    }


    private static String GetPropertyStringValue(object obj, String propertyName) {
        PropertyInfo pi = obj.GetType().GetProperty(propertyName);
        return (String)pi.GetValue(obj, new object[] { });
    }

}

}
