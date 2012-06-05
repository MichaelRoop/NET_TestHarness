using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.Outputs;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.TestHarness.Xml;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Logs.Initialisers {


/// <summary>Initialises the metadata objects with values from XML.</summary>
public class XmlLogInitialiser : ILogInitialiser {

    private String      fileName        = "";
    private IEnumerator logsIterator    = null;
    private XmlDocument document        = null;

    private static String logTag        = "Log";
    private static String stmtTemplateTag = "StmtTemplate";
    private static String outputTag     = "Output";
    private static String summaryTag    = "Summary";
    private static String credentialTag = "Credential";

    private static String attrOverwrite     = "overwrite";
    private static String attrIsUniqueName  = "uniqueName";
    private static String attrName      = "name";
    private static String attrValue     = "value";
    private static String attrType      = "type";
    private static String attrLogSuccess = "logSuccessCases";
    private static String attrnNewLineSequence = "newLineSequence";
    private static String attrSyntax    = "syntax";
    private static String attrfields    = "fields";
    private static String attrCreate    = "create";
    private static String attrLength    = "length";
    private static String attrStringDelimiter = "stringDelimiter";
    private static String attrColumnDelimiter = "fieldDelimiter";

    private static String nullLogName   = "nullLog";


    private XmlLogInitialiser() { }


    public XmlLogInitialiser(TestSetInfo info) {
        this.fileName = info.ConfigFile;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Load() {
        document     = XmlInitHelper.LoadAndValidate(fileName);
        logsIterator = XmlInitHelper.GetElementIterator(
            logTag,(XmlElement)document.SelectSingleNode("LogConfig/Logs")
            );
        return true;
    }


    /// <summary>
    /// Retrieve the next log info data until you reach the end.  If the returned LogInfo.IsValid
    /// is false then you are done.
    /// </summary>
    /// <returns></returns>
    public LogInfo GetNextLog() {
        return XmlExceptionProcessor.WrapTry(delegate() {
            if (this.logsIterator.MoveNext()) {
                LogInfo info = this.GetLog((XmlElement)this.logsIterator.Current);

                // We need to skip over the nullLog Log.  It is a place holder necessary to 
                // satisfy DTD attribute ID validation.
                if (info.Name == nullLogName) {
                    return this.GetNextLog();
                }
                return info;
            }
            return new LogInfo();
        });
    }


    private LogInfo GetLog(XmlElement e) {
        return new LogInfo(
            XmlElementHelper.GetAttributeValue<string>(e, attrName),
            XmlElementHelper.GetAttributeValue<LogType>(e, attrType),
            XmlElementHelper.GetAttributeValue<bool>(e, attrLogSuccess),
            this.GetSyntax(XmlElementHelper.GetAttributeValue<string>(e, attrSyntax)),
            this.GetFields(XmlElementHelper.GetAttributeValue<string>(e, attrfields)),
            this.GetOutput(e),
            this.GetSummary(e)
        );
    }


    private List<ColumnDef> GetFields(String fieldSet) {
        List<ColumnDef> columnDefs = new List<ColumnDef>();

        XmlElement e = document.GetElementById(fieldSet);
        foreach (XmlNode n in e.GetElementsByTagName(e.FirstChild.Name)) {
            XmlElement child = (XmlElement)n;
            columnDefs.Add(
                new ColumnDef(
                    XmlElementHelper.GetAttributeValue<string>(child, attrName),
                    XmlElementHelper.GetAttributeValue<string>(child, attrType),
                    XmlElementHelper.GetOptionalAttributeValue<int>(child, attrLength, 0)
                )
            );
        }
        return columnDefs;
    }


    private LogSyntaxInfo GetSyntax(String name)  {

        XmlElement e = document.GetElementById(name);
        if (e != null) {
            return new
                LogSyntaxInfo(
                    XmlElementHelper.GetAttributeValue<string>(e, attrName),
                    XmlElementHelper.GetAttributeValue<LogSyntaxType>(e, attrType),
                    XmlElementHelper.GetAttributeValue<string>(e, attrStringDelimiter),
                    XmlElementHelper.GetAttributeValue<string>(e, attrColumnDelimiter),
                    this.GetStatementTemplates(e)
                );
        }
        // should never happen DTD protected.
        throw new InputException("Did not find the Syntax element with name=" + name);
    }


    private List<StatementTemplate> GetStatementTemplates(XmlElement parent) {
        List<StatementTemplate> statements = new List<StatementTemplate>();

        IEnumerator it = XmlInitHelper.GetElementIterator(stmtTemplateTag, parent);
        while (it.MoveNext()) {
            XmlElement child = (XmlElement)it.Current;
            statements.Add( 
                new StatementTemplate(
                    XmlElementHelper.GetAttributeValue<StatementTemplateType>(child, attrType),
                    XmlElementHelper.GetAttributeValue<string>(child, attrValue)
                )
             );
        }
        return statements;
    }


    private OutputInfo GetOutput(XmlElement parent) {
        XmlElement e = (XmlElement)parent.SelectSingleNode(outputTag);
        return new OutputInfo(
                XmlElementHelper.GetAttributeValue<string>(e, attrName),
                XmlElementHelper.GetAttributeValue<OutputType>(e, attrType),
                XmlElementHelper.GetAttributeValue<bool>(e, attrOverwrite),
                XmlElementHelper.GetAttributeValue<bool>(e, attrIsUniqueName),
                XmlElementHelper.GetOptionalAttributeValue(e, attrnNewLineSequence, Environment.NewLine),
                this.GetCredentials(e)
            );
    }


    private LogInfo GetSummary(XmlElement parent) {
        XmlElement child = (XmlElement)parent.SelectSingleNode(summaryTag);
        if (XmlElementHelper.GetAttributeValue<bool>(child, attrCreate)) {
            return this.GetLog(
                (XmlElement)document.GetElementById(child.GetAttributeNode(attrName).Value));
        }
        return new LogInfo();
    }


    private Dictionary<CredentialType, OutputCredential> GetCredentials(XmlElement parent) {
        Dictionary<CredentialType,OutputCredential> credentials = 
            new Dictionary<CredentialType, OutputCredential>();

        IEnumerator it = XmlInitHelper.GetElementIterator(credentialTag, parent);
        while (it.MoveNext()) {
            XmlElement child = (XmlElement)it.Current;
            CredentialType key = XmlElementHelper.GetAttributeValue<CredentialType>(child, attrType);
            credentials.Add(
                key,
                new OutputCredential(key, XmlElementHelper.GetAttributeValue<string>(child, attrValue)));
        }
        return credentials;
    }


}
    
}
