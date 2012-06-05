using System;
using Ca.Roop.TestHarness.Logs.Sql;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Logs {

/// <summary>
/// SQL log object. Wraps the data in SQL statements before writing them to the output.
/// </summary>
class SqlLog : Log {

    SqlBuilder builder = null;


    /// <summary>Constructor.</summary>
    /// <param name="info">Log metadata.</param>
    public SqlLog(LogInfo info): base(info) {
        builder = new SqlBuilder(info);
    }


    /// <see cref="ILogable.WriteSummaryEntry"/>
    public override bool WriteSummaryEntry(ILogable logable) {
        this.WriteHeader();
        bool success = this.output.Write(builder.GetInsertStatement(logable));
        output.CloseOutput();
        return success;
    }


    /// <see cref="ILogable.WriteHeader"/>
    public override bool WriteHeader() {
        output.InitOutput();
        if (this.logInfo.OutputData.IsOverwrite) {
            TryHelper.EatException<String,bool>(this.output.Write, builder.DropStatement);
        }

        //Now attempt to create.
        TryHelper.EatException<String, bool>(this.output.Write, builder.CreateStatement);
        return true;
    }


    /// <see cref="Log.WriteEntry"/>
    protected override bool WriteEntry(Ca.Roop.TestHarness.Core.ITestable testable) {
        return output.Write(builder.GetInsertStatement(testable));
    }
}

}
