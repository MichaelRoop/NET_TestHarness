using System.Text;

namespace Ca.Roop.TestHarness.Logs.RowBuilders {

/// <summary>
/// Abstract base class implements the IRowBuilder and provides base functionality to the 
/// child objects.  The main thrust of this object is to iterate through a List of ColumnDef
/// objects found in the LogInfo and applying the abstract BuildColumn to each Columnd def. The 
/// children will override the BuildColumn to set out the column's data in a particular format.
/// </summary>
public abstract class RowBuilder : IRowBuilder {

    protected   StringBuilder   rowString;
    protected   int             count;
    protected   LogInfo         info;


    /// <summary>Default constructor hidden in private scope.</summary>
    private RowBuilder() { }


    /// <summary>Constructor.</summary>
    /// <param name="info">The metadata used to build the row.</param>
    public RowBuilder(LogInfo info) {
        rowString = new StringBuilder();
        count = 0;
        this.info = info;
    }

    /// <summary>Builds the row string from the List of ColumnDef data.</summary>
    /// <returns>The build data row.</returns>
    public string BuildRow() {
        foreach (ColumnDef d in this.info.ColumnDefs) {
            this.AddColumn(d);
        }
        return this.GetRowString();
    }


    /// <summary>
    /// Builds a row column based on the column data.
    /// </summary>
    /// <param name="columnData">Information on how to build one column of the row.</param>
    protected virtual void AddColumn(ColumnDef columnData) {
        if ((++count) > 1) {
            rowString.Append(this.info.SyntaxData.ColumnDelimiter);
        }
        this.BuildColumn(columnData); 
    }


    /// <summary>
    /// Get the row that has been assembled into a string
    /// </summary>
    /// <returns>The completed row string.</returns>
    protected string GetRowString() {
        return this.rowString.ToString();
    }


    /// <summary>
    /// Override to format each column's data before it is added to the row string.
    /// </summary>
    /// <param name="columnDef">The metadata for the column's data.</param>
    protected abstract void BuildColumn(ColumnDef columnDef);

}

}
