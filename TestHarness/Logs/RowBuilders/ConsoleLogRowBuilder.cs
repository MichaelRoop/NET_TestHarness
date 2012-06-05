
namespace Ca.Roop.TestHarness.Logs.RowBuilders {

/// <summary>Build a list of values for a data row from an ILogable object.</summary>
public class ConsoleLogRowBuilder : QueryableRowBuilder {


    /// <summary>Constructor.</summary>
    /// <param name="logable">The object that constains the row of data.</param>
    /// <param name="info">The metadata used to build the row.</param>
    public ConsoleLogRowBuilder(ILogable logable, LogInfo info)
        : base(logable, info) {
    }


    /// <summary>
    /// Overrides the AddColumn to change the way data is formatted. It is called to present a 
    /// left padded column name and data, one per line.  Used for the summary in the logger.
    /// </summary>
    /// <param name="columnDef"></param>
    protected override void AddColumn(ColumnDef columnDef) {
        if ((++count) > 1) {
            rowString.Append(info.OutputData.NewLineSequence);
        }

        this.rowString.Append(columnDef.Name.PadLeft(30)).Append(": ");
        this.BuildColumn(columnDef);
    }

}

}
