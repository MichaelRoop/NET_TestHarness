using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Logs.Sql;
using Ca.Roop.TestHarness.Outputs;


namespace Ca.Roop.TestHarness.Logs.RowBuilders {


/// <summary>Abstracts the creation of the derived IRowBuilder objects.</summary>
public class RowBuilderFactory {

    /// <summary>Get the builder for SQL CREATE column name/type details.</summary>
    /// <param name="info">The metadata used to build the row.</param>
    /// <returns>An IRowBuilder to create a row of column name and data types.</returns>
    public static IRowBuilder SqlNameAndTypeColumns(LogInfo info) {
        return new SqlColumnNameTypeListBuilder(info);
    }


    /// <summary>Builder for Header row.</summary>
    /// <param name="info">The metadata used to build the row.</param>
    /// <returns>A builder for the header row.</returns>
    public static IRowBuilder GetHeaderBuilder(LogInfo info) {
        return new HeaderRowBuilder(info);
    }


    /// <summary>Retrieve the builder for row value columns.</summary>
    /// <param name="queryable"></param>
    /// <param name="info">The metadata used to build the row.</param>
    /// <returns>The builder for row value columns.</returns>
    public static IRowBuilder GetRowBuilder(IQueryable queryable, LogInfo info) {
        return new QueryableRowBuilder(queryable, info);
    }


    /// <summary>Retrieve the summary row builder for the Log object.</summary>
    /// <param name="log">The logable item that provides the data.</param>
    /// <param name="info">The metadata used to build the row.</param>
    /// <returns></returns>
    public static IRowBuilder GetSummaryBuilder(ILogable log, LogInfo info) {
        // TODO Try to find a better way to do this.
        return (info.OutputData.Type == OutputType.CONSOLE || info.OutputData.Type == OutputType.EMAIL)
                    ? new ConsoleLogRowBuilder(log, info)
                    : GetRowBuilder(log, info);
    }

}

}
