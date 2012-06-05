using System;
using System.Collections;
using System.Xml;

namespace Ca.Roop.TestHarness.Xml {


/// <summary>
/// Helper class to initialise
/// </summary>
public class XmlInitHelper {

    /// <summary>
    /// Creates loads the XML document from a file with DTD validation.
    /// </summary>
    /// <param name="fileName">XML file to load.</param>
    /// <returns>The validated XML document.</returns>
    /// <exception cref="InputException with embedded XML errors if applicable" />
    public static XmlDocument LoadAndValidate(String fileName) {
        return XmlExceptionProcessor.WrapTry(delegate() {
            XmlReaderSettings settings  = new XmlReaderSettings();
            settings.ValidationType = ValidationType.DTD;
            settings.ProhibitDtd = false;
            XmlDocument doc             = new XmlDocument();
            doc.Load(XmlTextReader.Create(fileName, settings));
            return doc;
        });
    }


    /// <summary>Get the elements iterator by element name from an XmlDocument.</summary>
    /// <param name="name">Name of the Elements to retrieve.</param>
    /// <param name="doc">The XmlDocument.</param>
    /// <returns>An inumerator to a list of Elements.</returns>
    public static IEnumerator GetElementIterator(String name, XmlDocument doc) {
        return XmlExceptionProcessor.WrapTry(delegate() {
            return doc.GetElementsByTagName(name).GetEnumerator();
        });
    }


    /// <summary>Get the elements iterator by element name from an XmlElement.</summary>
    /// <param name="name">Name of the Elements to retrieve.</param>
    /// <param name="doc">The XmlElement parent.</param>
    /// <returns>An inumerator to a list of Elements.</returns>
    public static IEnumerator GetElementIterator(String name, XmlElement e) {
        return XmlExceptionProcessor.WrapTry(delegate() {
            return e.GetElementsByTagName(name).GetEnumerator();
        });
    }


}

}
