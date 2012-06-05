using System;
using System.Text;

namespace Ca.Roop.TestHarness.Outputs {

/// <summary>
/// Output credential metadata object.  This metadata is used to establish connections.
/// </summary>
public class OutputCredential {

    private OutputCredential() { }


    /// <summary>Constructor.</summary>
    /// <param name="type">The credential type.</param>
    /// <param name="value">The credential string.</param>
    public OutputCredential(CredentialType type, String value) {
        this.Type  = type;
        this.Value = value;
    }

    /// <summary>The credential type./// </summary>
    public CredentialType Type { get; private set; }


    /// <summary>The credential string./// </summary>
    public String Value { get; private set; }


    // dev only - TODO remove.
    public override String ToString() {
        return new StringBuilder()
        .Append("===== OutputCredential =====\n")
        .Append("Type:").Append(this.Type.ToString()).Append("\n")
        .Append("Value:").Append(this.Value).Append("\n")
        .ToString();
    }


}

}
