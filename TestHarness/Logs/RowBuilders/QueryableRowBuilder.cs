using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Logs.RowBuilders {

/// <summary>
/// Builds a row of data from a queryable object's data.
/// </summary>
public class QueryableRowBuilder : RowBuilder {
    
	private IQueryable queryable;


    /// <summary>Constructor.</summary>
    /// <param name="queryable">The object to query.</param>
    /// <param name="info">The metadata used to build the row.</param>
	public QueryableRowBuilder(IQueryable queryable, LogInfo info) : base(info) {
		this.queryable = queryable;
	}


    /// <see cref="RowBuilder.BuildColumn"/>
    protected override void BuildColumn(ColumnDef columnDef) {
        this.rowString.Append(queryable.GetValue(columnDef, this.info));
    }
}

}
