using System;
using System.Text;

namespace Ca.Roop.TestHarness.Logs {

/// <summary>SQL Statement template metadata object.</summary>
public class StatementTemplate {

    // Hidden default constructor.
    private StatementTemplate() {
        this.Type = StatementTemplateType.UNDEFINED;
    }


    /// <summary>Constructor.</summary>
    /// <param name="type">The statement type</param>
    /// <param name="value">The statement string</param>
    public StatementTemplate(StatementTemplateType type, String value) {
        this.Type = type;
        this.Value = value;
    }


    /// <summary>Retrieve the template type.</summary>
    public StatementTemplateType Type { get; private set; }


    /// <summary>Retrieve the template string.</summary>
    public String Value { get; private set; }


    // Dev only TODO - remove
    public override string ToString() {
        return new StringBuilder()
        .Append("===== StatementTemplate =====\n")
        .Append("Type:").Append(this.Type.ToString()).Append("\n")
        .Append("Value:").Append(this.Value).Append("\n")
        .Append("===== End StatementTemplate =====\n")
        .ToString();
    }


}

}
