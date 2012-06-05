using System;
using System.Text;
using System.Xml;
using Ca.Roop.TestHarness.Compare;
using Ca.Roop.TestHarness.TestExceptions;


namespace Ca.Roop.TestHarness.Xml {

/// <summary>Wrappers for common XmlElement functionality.</summary>
public class XmlElementHelper {


    /// <summary>
    /// Retrieves the text portion of the element.  This would be when the first node is text.
    /// </summary>
    /// <param name="e">The XmlElement to query.</param>
    /// <param name="validateOnEmpty">Throw an exception if the TEXT node is empty.</param>
    /// <returns>The element's TEXT.</returns>
    /// <exception cref="InputException" />
    public static String GetText(XmlElement e, bool validateOnEmpty) {
        // There should only be the text value. Constrained by DTD.
        XmlNodeList nodes = e.ChildNodes;
        switch (nodes.Count) {
        case 0:
            throw new InputException("Empty XML Element:" + e.Name);
        case 1:
            if (nodes.Item(0).NodeType != XmlNodeType.Text) {
                throw new InputException("XML Element:" + e.Name + " is not a TEXT type");
            }
            else if (validateOnEmpty && nodes.Item(0).Value.Length == 0) {
                throw new InputException(
                    "XML Element:" + e.Name + " not expecting an empty TEXT node");
            }
            return nodes.Item(0).Value;
        default:
            throw new InputException("XML Element malformed:" + e.Name);
        }
    }


    /// <summary>Get the attribute value.</summary>
    /// <typeparam name="T">The generic type to convert the xml string.</typeparam>
    /// <param name="e">The element.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>The XML attribute converted to the type specified in the generic parameter.</returns>
    public static T GetAttributeValue<T>(XmlElement e, String attributeName) {
        XmlAttribute a = e.GetAttributeNode(attributeName);
        if (a == null) {
            throw new InputException("<" + e.Name + "> " + attributeName + "=   Could not be found");
        }
        T tmp = default(T);
        return ConvertAttributeValue<T>(a, false, tmp);
    }


    /// <summary>Get the optional attribute value or default value provided.</summary>
    /// <typeparam name="T">The generic type to convert the xml string.</typeparam>
    /// <param name="e">The element.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="defaultValue">The generic default value.</param>
    /// <returns>The XML attribute converted to the type specified in the generic parameter or the default.</returns>
    public static T GetOptionalAttributeValue<T>(XmlElement e, String attributeName, T defaultValue) {
        XmlAttribute a = e.GetAttributeNode(attributeName);
        if (a == null) {
            return defaultValue;
        }
        return ConvertAttributeValue<T>(a, true, defaultValue);
    }


    private static String GetValueOrDefault<T>(XmlAttribute a, bool optional, T defaultValue) {
        if (!optional) {
            return a.Value;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(defaultValue);
        return a.Value.Length > 0 ? a.Value : sb.ToString();
    }


    private static T ConvertAttributeValue<T>(XmlAttribute a, bool optional, T defaultValue) {
        if (PrimitiveTypeCheck.IsEnum(default(T))) {
            return (T)Enum.Parse(typeof(T), a.Value, true);
        }

        try {
            return (T)Convert.ChangeType(GetValueOrDefault(a, optional, defaultValue), typeof(T));
        }
        catch (Exception) {
            throw new InputException("Failed to convert " + a.Value);
        }
    }

}

}
