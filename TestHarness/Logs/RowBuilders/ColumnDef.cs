using System;
using System.Text;

namespace Ca.Roop.TestHarness.Logs.RowBuilders {


/// <summary>Metadata object for one column of data.</summary>
public class ColumnDef {

    // Hidden default constructor.
    private ColumnDef() { }


    /// <summary>Constructor.</summary>
    /// <param name="name">Column name.</param>
    /// <param name="type">The SQL representation of the data type.</param>
    /// <param name="maxLength">The maximum data length for the column.</param>
    public ColumnDef(String name, String type, int maxLength) {
        this.Name               = name;
        this.SqlRepresentation  = type;
        this.MaxLength          = maxLength;
    }


    /// <summary>Column name.</summary>
    public String Name { get; private set; }


    /// <summary>SQL representation of column data type.</summary>
    public String SqlRepresentation { get; private set; }


    /// <summary>Maximum length for column data.</summary>
    public int MaxLength { get; private set; }


    /// <summary>Returns whether the data has a max length that may require truncation of 
    /// data before storage.
    /// </summary>
    public bool IsDataToBeTruncated() {
        return this.MaxLength >= 0;
    }


    public override string ToString() {
        return new StringBuilder()
            .Append("===== ColumnDef =====\n")
            .Append("Name:").Append(this.Name).Append("\n")
            .Append("type:").Append(this.SqlRepresentation).Append("\n")
            .Append("MaxLength:").Append(this.MaxLength).Append("\n")
            .Append("===== End ColumnDef =====\n")
            .ToString();
    }


}

}
