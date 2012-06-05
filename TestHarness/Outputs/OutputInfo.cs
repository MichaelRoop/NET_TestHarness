using System;
using System.Collections.Generic;
using System.Text;
using Ca.Roop.Utilities;
using Ca.Roop.TestHarness.TestExceptions;

namespace Ca.Roop.TestHarness.Outputs {

/// <summary>The output metadata object.</summary>
public class OutputInfo {

    private Dictionary<CredentialType,OutputCredential> credentials;


    /// <summary>Default constructor creates an invalid object to denote end of list.</summary>
    public OutputInfo() : this( "", OutputType.UNDEFINED, false, false, "", null) {
    }


    /// <summary>Constructor.</summary>
    /// <param name="name">Output artifact name like a file or SQL table.</param>
    /// <param name="type">Output type.</param>
    /// <param name="isOverwrite">Flag to overwrite existing output artifact.</param>
    /// <param name="isUniqueName">Flag to generate a unique output artifact name.</param>
    /// <param name="newLineSequence">New line character sequence.</param>
    /// <param name="credentials">List of output connection credentials.</param>
    public OutputInfo(
        String name, 
        OutputType type, 
        bool isOverwrite, 
        bool isUniqueName,
        String newLineSequence,
        Dictionary<CredentialType,OutputCredential> credentials) {

        this.Name           = name;
        this.Type           = type;
        this.IsOverwrite    = isOverwrite;
        this.IsUniqueName   = isUniqueName;
        this.NewLineSequence = newLineSequence;
        this.credentials    = credentials;
    }


    /// <summary>
    /// Name of the output artifact if applicable. For example, a file output would have 
    /// 'filename.txt' as the name.  For a JDBC output it would be a table name.
    /// </summary>
    public String Name { get; private set; }


    /// <summary>The output type from OutputType.</summary>
    public OutputType Type { get; private set; }


    /// <summary>Determines if an output artifact is to be overwritten if applicable.</summary>
    public bool IsOverwrite { get; private set; }


    /// <summary>
    /// Determines if an output artifact is created with a unique name. To create a unique name, 
    /// the run id is appended to the existing name before creation of the artifact.
    /// </summary>
    public bool IsUniqueName { get; private set; }


    /// <summary>New Line character sequence.</summary>
    public String NewLineSequence { get; private set; }


    /// <summary>
    /// Retrieve credential information such as connection string, user, etc by key. 
    /// </summary>
    /// <param name="type">The lookup key.</param>
    /// <returns>The credential as a string.</returns>
    /// <exception cref="InputException">If the key is not found or the value is empty.</exception>
    public String GetCredential(CredentialType type) {
        OutputCredential ret = null;
        if (this.credentials.TryGetValue(type, out ret)) {
            if (ret.Value.Length > 0) {
                return ret.Value;
            }
            throw new InputException("Zero length " + type.ToString() + " credential.");
        }
        throw new InputException("No " + type.ToString() + " credential found.");
    }


    /// <summary> Retrieves credential if present, otherwise returns the default value.</summary>
    /// <param name="type">The lookup key.</param>
    /// <param name="defaultValue">The value to return if key lookup is unsuccessful.</param>
    /// <returns>The value corresponding to the key or the defaultValue if not found.</returns>
    public String GetOptionalCredential(CredentialType type, String defaultValue) {
        OutputCredential ret = null;
        if (this.credentials.TryGetValue(type, out ret)) {
            return ret.Value;
        }
        return defaultValue;
    }


    // Dev only TODO remove
    public override string ToString() {
        // temp dump for dev

        StringBuilder sb = new StringBuilder();
        if (this.credentials != null) {
            foreach (KeyValuePair<CredentialType, OutputCredential> c in this.credentials) {
                sb.Append(c.Value.ToString());
            }
        }

        return new StringBuilder()
        .Append("===== OutputInfo =====\n")
        .Append("Name:").Append(this.Name).Append("\n")
        .Append("Type:").Append(this.Type.ToString()).Append("\n")
        .Append("IsOverwrite:").Append(Convert.ToString(this.IsOverwrite)).Append("\n")
        .Append("IsUniqueName:").Append(Convert.ToString(this.IsUniqueName)).Append("\n")
        //.Append("Credentials:").Append(this.Credentials == null 
        //    ? Str.SafeToString(this.Credentials)
        //    : sb.ToString()).Append("\n")
        .Append("Credentials:").Append(sb.ToString()).Append("\n")
        .Append("===== End OutputInfo =====\n")
        .ToString();
    }



}


}
