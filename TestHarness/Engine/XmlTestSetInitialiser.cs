using System;
using System.Xml;
using Ca.Roop.TestHarness.Xml;

namespace Ca.Roop.TestHarness.Engine {


/// <summary>Retrieves test set information from a data source.</summary>
public class XmlTestSetInitialiser : ITestSetInitialiser {

    private XmlDocument     document    = null;
    System.Collections.IEnumerator testIterator = null;

    static String testSetTag		= "TestSet";
    static String assemblyNameTag	= "Assembly";
    static String classNameTag	 	= "Class";
    static String configFileTag		= "ConfigFile";
    static String scriptFileTag		= "ScriptFile";
    static String attrName          = "name";

    // Default constructor in private scope to prevent usage.
    private XmlTestSetInitialiser() {}


    /// <summary>Constructor.</summary>
    /// <param name="fileName">File to parse for metadata.</param>
    public XmlTestSetInitialiser(String fileName){
        this.document       = XmlInitHelper.LoadAndValidate(fileName);
        this.testIterator   = XmlInitHelper.GetElementIterator(testSetTag, document);
    }


    /// <summary>Retrive the next Test Set metadata object.</summary>
    /// <returns>A TestSetInfo.IsValid true if valid, otherwise it is end of list.</returns>
    public TestSetInfo GetNextSet() {
        return XmlExceptionProcessor.WrapTry( delegate() {
            if (this.testIterator.MoveNext()) {
                return this.ProcessTestSetElement((XmlElement)this.testIterator.Current);
            }
            return new TestSetInfo();
        });
    }


    private TestSetInfo ProcessTestSetElement(XmlElement e) {
        return new TestSetInfo(
            this.AttrValue(e,assemblyNameTag),
            this.AttrValue(e,classNameTag),
            this.AttrValue(e,configFileTag),
            this.AttrValue(e,scriptFileTag)
        );
    }


    private String AttrValue(XmlElement e, String childName) {
        return ((XmlElement)e.SelectSingleNode(childName)).GetAttributeNode(attrName).Value;
    }


}

}
