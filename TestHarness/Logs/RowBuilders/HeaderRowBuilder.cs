
namespace Ca.Roop.TestHarness.Logs.RowBuilders {


/// <summary>Builds a data row with only the delimited column names.</summary>
public class HeaderRowBuilder : RowBuilder {

    /// <summary>Constructor.</summary>
    /// <param name="info">The metadata used to build the row.</param>
    public HeaderRowBuilder(LogInfo info)
        : base(info) {
    }


    /// <summary>Adds only the column name.</summary>
    /// <see cref="RowBuilder.BuildColumn"/>
    protected override void BuildColumn(ColumnDef columnDef) {
        this.rowString.Append(columnDef.Name);
    }
}

}

