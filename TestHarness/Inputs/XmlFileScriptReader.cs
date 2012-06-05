using System;
using System.Xml;
using Ca.Roop.TestHarness.Core.Test;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.TestHarness.Xml;
using System.Collections.Generic;


namespace Ca.Roop.TestHarness.Inputs {

/// <summary>
/// Reads in the XML lists of test cases to execute along with their arguments.
/// </summary>
public class XmlFileScriptReader : IScriptReader {

    private String          fileName    = null;
    private XmlDocument     document    = null;
    System.Collections.IEnumerator testIterator;

    private static String   testTag     = "Test";
    private static String   argTag      = "Arg";
    private static String   nameAttr    = "name";
    private static String   valueAttr   = "value";
    private static String   typeAttr    = "type";
    private static String   itemTypeAttr= "itemtype";


    // Default constructor in private scope to prevent access. 
    private XmlFileScriptReader() {
    }
     
    
    /// <summary>Constructor.</summary>
    /// <param name="init">Initializer to access script information.</param>
    /// <param name="setName">The name of the test set.</param>
    public XmlFileScriptReader(TestSetInfo info) {
        this.fileName = info.ScriptFile;
    }


    /// <summary>Constructor.</summary>
    /// <param name="fileName">Name of the test script XML file</param>
    public XmlFileScriptReader(String fileName) {
        this.fileName = fileName;
    }


    /// <summary>Open the XML file and validate.</summary>
    public void Open() {
        document     = XmlInitHelper.LoadAndValidate(fileName);
        testIterator = XmlInitHelper.GetElementIterator(testTag, document);
    }

    
    /// <summary>
    /// Retrieve the next test information from the script. Check Info.IsValid() to find if you 
    /// have reached the end of the tests.
    /// </summary>
    /// <exception cref="InputException" />
    /// <returns>An Info object. Check Info.IsNull() to find if you have reached the end.</returns>
    public TestInfo GetNextTest() {
        return XmlExceptionProcessor.WrapTry(delegate() {
            if (this.testIterator.MoveNext()) {
                return this.GetTestInfo((XmlElement)this.testIterator.Current);
            }
            return new TestInfo();
        });
    }


    private TestInfo GetTestInfo(XmlElement e) {
        return new TestInfo(AttrValue(e, nameAttr), this.GetArgs(e));
    }


    private List<TestArg> GetArgs(XmlElement e) {
        List<TestArg> args = new List<TestArg>();
        foreach (XmlNode n in e.GetElementsByTagName(argTag)) {
            args.Add(this.GetArg((XmlElement)n));
        }
        return args;
    }


    private TestArg GetArg(XmlElement e) {
        return new TestArg(
            AttrValue(e, nameAttr), 
            AttrValue(e, valueAttr), 
            AttrValue(e, typeAttr), 
            OptionalAttrValue(e,itemTypeAttr));
    }


    private String AttrValue(XmlElement e, String name) {
        return e.GetAttributeNode(name).Value;
    }


    private String OptionalAttrValue(XmlElement e, String name) {
        if (!e.HasAttribute(name)) {
            return "";
        }
        return this.AttrValue(e, name);
    }


}

}// end namespace.
