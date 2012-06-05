
using System;
using System.Text.RegularExpressions;
using Ca.Roop.TestHarness.Logs.RowBuilders;

namespace Ca.Roop.TestHarness.Logs.Sql {

/// <summary>Builds a row of column names and type. Each unit is delimited in the row.</summary>
public class SqlColumnNameTypeListBuilder : RowBuilder {

    private static String typeSizeToken = "#";
        
    /// <summary>Constructor.</summary>
    /// <param name="info"></param>
    public SqlColumnNameTypeListBuilder(LogInfo info) : base(info) {
    }


    /// <summary>Called by the base class to build one column's information.</summary>
    /// <param name="columnDef">Metadat for one column.</param>
    protected override void BuildColumn(ColumnDef columnDef) {

        // columnDef.MaxLength

        this.rowString
            .Append(columnDef.Name)
            .Append(" ")
            .Append(this.BuildColumnType(columnDef));
    }


    /// <summary>Builds a SQL type string if # found and MaxLength larger than 0.</summary>
    /// <param name="columnDef">Column metadata object.</param>
    /// <returns>
    /// Built type string if constraints are present or the ColumnDef.SqlRepresentation.
    /// </returns>
    private String BuildColumnType(ColumnDef columnDef) { 
        Regex reg = new Regex("[" + typeSizeToken + "]");
        if (reg.IsMatch(columnDef.SqlRepresentation) && columnDef.MaxLength > 0) {
            return reg.Replace(columnDef.SqlRepresentation, columnDef.MaxLength.ToString());
        }
        return columnDef.SqlRepresentation;
    }



}

}
