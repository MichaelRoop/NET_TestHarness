using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ca.Roop.TestHarness.Core.Test {

public class ArgListParser {

    // This will strip off anything before { and after }.
    private const String bracketPattern = @"\{(.*)\}";


    public static object Parse(String value, String itemType) {

        String[] items = new Regex(",").Split(StripOuterBraces(value));
        if (items.Length == 0) {
            throw new ArgumentException("No values for List", value);
        }

        //Convert first item to type to find generic type from script alias name.
       object o = ArgConverterFactory.GetConverter(itemType).Invoke(items[0], "");

        // Get the generic type.
        Type genericListType = typeof(List<>);
        Type[] itemArgs = { o.GetType() };
        Type constructed = genericListType.MakeGenericType(itemArgs);
        object listObject = Activator.CreateInstance(constructed);

        // Adding each converted item to the new List object.
        foreach (String s in items) {
            object itemObj = ArgConverterFactory.GetConverter(itemType).Invoke(s, "");
            listObject.GetType().InvokeMember(
                "Add",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                listObject,
                new object[] {itemObj}
            );
        }

        //// Code to iterate through and read and print out the values.
        //foreach (object i in (IEnumerable)listObject) {
        //    System.Console.WriteLine("*** Reflective list value:{0}, type:{1}", i, i.GetType().Name);
        //}

        return listObject;
    }


    private static bool HasOuterBraces(String text) {
        return new Regex(bracketPattern).IsMatch(text);
    }


    private static String StripOuterBraces(String text) {
        if (!HasOuterBraces(text)) {
            throw new ArgumentException("Invalid format:" + text);
        }

        Match m = new Regex(bracketPattern).Match(text);
        return text.Substring(m.Index + 1, m.Length - 2);
    }


    private static void CheckNoMoreBrackets(String text) {
        if (new Regex(@"{").IsMatch(text) || new Regex(@"}").IsMatch(text)) {
            throw new ArgumentException("Invalid format:" + text);
        }
    }

}

}
