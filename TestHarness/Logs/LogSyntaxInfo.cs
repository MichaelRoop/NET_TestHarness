using System;
using System.Collections.Generic;
using System.Text;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Logs {

/// <summary>Log Syntax metadata object.</summary>
public class LogSyntaxInfo {

    private LogSyntaxInfo() { }

    /// <summary>Constuctor.</summary>
    /// <param name="name">Syntax object name.</param>
    /// <param name="type">Syntax type.</param>
    /// <param name="stringDelimiter">Character that delimits a string like quotes.</param>
    /// <param name="columnDelimiter">Character that delimits data between columns.</param>
    /// <param name="templates">List of SQL statement template metadata.</param>
    public LogSyntaxInfo(
        String                  name, 
        LogSyntaxType           type, 
        String                  stringDelimiter, 
        String                  columnDelimiter, 
        List<StatementTemplate> templates) {

        this.Name = name;
        this.Type = type;
        this.StringDelimiter = stringDelimiter;
        this.ColumnDelimiter = columnDelimiter;
        this.StatementTemplates = templates;
    }

    /// <summary>Syntax name.</summary>
    public String Name { get; private set; }


    /// <summary>Syntax type.</summary>
    public LogSyntaxType Type { get; private set; }


    /// <summary>String delimiter.</summary>
    public String StringDelimiter { get; private set; }


    /// <summary>Column delimiter.</summary>
    public String ColumnDelimiter { get; private set; }


    /// <summary>SQL Statement template metadata.</summary>
    public List<StatementTemplate> StatementTemplates { get; private set; }


    // DEV only TODO - remove.
    public override string ToString() {
        // temp dump for dev

        StringBuilder sb = new StringBuilder();
        if (this.StatementTemplates != null) {
            foreach (StatementTemplate t in this.StatementTemplates) {
                sb.Append(t.ToString());
            }
        }

        return new StringBuilder()
        .Append("===== LogSyntaxInfo =====\n")
        .Append("Name:").Append(this.Name).Append("\n")
        .Append("Type:").Append(this.Type.ToString()).Append("\n")
        .Append("StringDelimiter:").Append(this.StringDelimiter).Append("\n")
        .Append("ColumnDelimiter:").Append(this.ColumnDelimiter).Append("\n")
        .Append("StatementTemplates:").Append(this.StatementTemplates == null 
            ? Str.SafeToString(this.StatementTemplates)
            : sb.ToString()).Append("\n")
        .Append("===== End LogSyntaxInfo =====\n")
        .ToString();
    }

}

}
