using System;
using System.Collections.Generic;
using System.Text;
using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.Outputs;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Logs {

/// <summary>The main metadata object for a log.</summary>
public class LogInfo {

    /// <summary>Constructor to create a new LogInfo metadata oject.</summary>
    /// <param name="name">The name of the Log.</param>
    /// <param name="type">The type of the Log.</param>
    /// <param name="LogSuccessCases">Flag to indicate if successful test cases are logged.</param>
    /// <param name="syntax">The Log output syntax. Like SQL or CVS.</param>
    /// <param name="columnDefs">List of Definitions information for each field logged.</param>
    /// <param name="outputInfo">Creation information for the Log's Output object.</param>
    public LogInfo(
        String name, 
        LogType type, 
        bool logSuccessCases,
        LogSyntaxInfo syntax, 
        List<ColumnDef> columnDefs,
        OutputInfo outputInfo,
        LogInfo summaryLogInfo) {

        this.Name = name;
        this.Type = type;
        this.LogSuccessCases = logSuccessCases;
        this.SyntaxData = syntax;
        this.ColumnDefs = columnDefs;
        this.OutputData = outputInfo;
        this.SummaryLogInfo = summaryLogInfo;
    }


    /// <summary>
    /// Default constructor creates an invalid LogInfo to be used as end of list indicator.
    /// </summary>
    public LogInfo() : this("", LogType.UNDEFINED, false, null, null, null,null )   {
    }


    /// <summary>Log name.</summary>
    public String Name { get; private set; }


    /// <summary>Log type.</summary>
    public LogType Type { get; private set; }


    /// <summary>Flag to indicate if successful test cases are to be logged.</summary>
    public bool LogSuccessCases { get; private set; }


    /// <summary>Log syntax metadata object.</summary>
    public LogSyntaxInfo SyntaxData { get; private set; }


    /// <summary>List of column metadata objects.</summary>
    public List<ColumnDef> ColumnDefs { get; private set; }


    /// <summary>Output metadata object.</summary>
    public OutputInfo OutputData { get; private set; }


    /// <summary>Summary log metadata object.</summary>
    public LogInfo SummaryLogInfo { get; private set; }


    /// <summary>Indicates if this object holds valid summary log metadata.</summary>
    /// <returns>true if a summary log info, otherwise false.</returns>
    public bool IsSummaryLogInfo() {
        return this.SummaryLogInfo.Type == LogType.UNDEFINED;
    }


    /// <summary>Indicates if this is a valid metadat object or an end of list indicator.</summary>
    /// <returns></returns>
    public bool IsValid() {
        return this.Type != LogType.UNDEFINED;
    }

    // for dev only TODO - remove later.
    public override string ToString() {

        StringBuilder sbDefs = new StringBuilder();
        if (this.ColumnDefs == null) {
            sbDefs.Append("null");
        }
        else {
            foreach (ColumnDef def in this.ColumnDefs) {
                sbDefs.Append(def.ToString());
            }
        }

        return new StringBuilder()
        .Append("===== LogInfo =====\n")
        .Append("Name:").Append(this.Name).Append("\n")
        .Append("Type:").Append(this.Type.ToString()).Append("\n")
        .Append("LogSyntaxInfo:").Append(Str.SafeToString(this.SyntaxData)).Append("\n")
        .Append("ColumnDefs:").Append(sbDefs.ToString()).Append("\n")
        .Append("OutputData:").Append(Str.SafeToString(this.OutputData)).Append("\n")
        .Append("Summary:").Append(this.SummaryLogInfo).Append("\n")
        .Append("===== End LogInfo =====\n")
        .ToString();
    }


}

}
