using System;
using System.Text;
using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Core.Test;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.Outputs;
using Ca.Roop.TestHarness.TestExceptions;

namespace Ca.Roop.TestHarness.Logs {


/// <summary>
/// Abstract class that implements the ILogable interface and provides base functionality for
/// its childres.
/// </summary>
public abstract class Log : ILogable {

    protected LogInfo       logInfo             = null;
	protected IOutputable	output              = null;	// logOutput pointer.
	protected ILogable		summaryLog          = null; // summary logger pointer.
	private int				successCount        = 0;	// Counter for successful tests.
	private int				failInitCount       = 0;	// Counter tests that failed on Init.
	private int				failSetupCount      = 0;	// Counter tests that failed on setup.
	private int				failTestCount       = 0;	// Counter tests that failed on test.
	private int				failCleanupCount    = 0;    // Counter tests that failed on cleanup.
	private int				notExistCount       = 0;	// Counter tests not found.
	private int				exceptionCount      = 0;	// Number of test fail to unchecked exceptions.
	private int				assertCount         = 0;	// Number of test fail to assertions.

	
	// hidden default constructor.
	private Log() {}


    /// <summary>Constructor.</summary>
    /// <param name="info">The metadata to create the data logging.</param>
	public Log(LogInfo info) {
        this.logInfo    = info;
        this.output     = OutputFactory.Create(this.logInfo);
        this.summaryLog = LogFactory.Create(this.logInfo.SummaryLogInfo);
	}


    /// <see cref="ILogable.WriteSummaryEntry"/>
    public abstract bool WriteSummaryEntry(ILogable logable);


    /// <see cref="Logs.WriteHeader"/>
    public abstract bool WriteHeader();


    /// <see cref="Logs.LogTestable"/>
    public bool LogTestable(ITestable testable) {
		switch(testable.Status)
		{
		case TestStatus.SUCCESS:
			++successCount;
            if (!this.logInfo.LogSuccessCases) {
                return true;
            }
			break;
        case TestStatus.FAIL_INIT:		
			++failInitCount;
			break;
        case TestStatus.FAIL_SETUP:		
			++failSetupCount;
			break;
        case TestStatus.FAIL_TEST:		
			++failTestCount;
			break;
        case TestStatus.FAIL_CLEANUP:	
			++failCleanupCount;
			break;
        case TestStatus.NOT_EXISTS:		
			++notExistCount;
			break;
        case TestStatus.FAIL_BY_EXCEPTION:
			++exceptionCount;
			break;
        case TestStatus.FAIL_BY_ERROR:
            ++assertCount;
            break;
		default:
            throw new System.ArgumentException("Invalid Status", testable.Status.ToString());
		}

		return this.WriteEntry(testable);
	}


    /// <see cref="Logs.Summarize"/>
    public bool Summarize() {
		if (summaryLog != null) {
			return summaryLog.WriteSummaryEntry(this);
		}
		return false;
	}


    /// <summary>Write the line entry for the log output.</summary>
    /// <param name="testable">The test case object to log.</param>
    /// <returns></returns>
	protected abstract bool WriteEntry(ITestable testable);


	/// <see cref="Logs.Close"/>
	public void Close() {
		if (this.output != null) {
			this.output.CloseOutput();
		}
		if (this.summaryLog != null) {
			this.summaryLog.Close();
		}
	}

	
    /// <summary>
    /// Implements the method from IQueryable. Allows the objects data to be querried.
    /// </summary>
    /// <param name="columnDef">Column metadata.</param>
    /// <param name="info">Log metadata.</param>
    /// <returns>The string format of the data indexed by the column's metadata.</returns>
	public String GetValue(ColumnDef columnDef, LogInfo info) {
		if (columnDef.Name.CompareTo("SuccessCount") == 0) {
			return Convert.ToString(this.successCount);
		}
        else if (columnDef.Name.CompareTo("FailInitCount") == 0) {
            return Convert.ToString(this.failInitCount);
		}
        else if (columnDef.Name.CompareTo("FailSetupCount") == 0) {
            return Convert.ToString(this.failSetupCount);
		}
        else if (columnDef.Name.CompareTo("FailTestCount") == 0) {
            return Convert.ToString(this.failTestCount);
		}
        else if (columnDef.Name.CompareTo("FailCleanupCount") == 0) {
            return Convert.ToString(this.failCleanupCount);
		}
        else if (columnDef.Name.CompareTo("NotExistCount") == 0) {
            return Convert.ToString(this.notExistCount);
		}
        else if (columnDef.Name.CompareTo("UncheckedExceptionCount") == 0) {
            return Convert.ToString(this.exceptionCount);
		}
        else if (columnDef.Name.CompareTo("JavaErrorCount") == 0) {
            return Convert.ToString(this.assertCount);
		}
        else if (columnDef.Name.CompareTo("TotalCount") == 0) {
			return Convert.ToString(
					this.failInitCount + this.failSetupCount + this.failCleanupCount + 
					this.failTestCount + this.successCount   + this.notExistCount + exceptionCount +
                    this.assertCount
				);
		}
        else if (columnDef.Name.CompareTo("RunId") == 0) {
            // Do NOT do a truncate check against the run id.  The output would become useless to identify.
            // Better let it to fail on length limit.
            return new StringBuilder()
                .Append(info.SyntaxData.StringDelimiter)
                .Append(TestEngine.GetInstance().GetRunId())
                .Append(info.SyntaxData.StringDelimiter)
                .ToString();
		}
		else 
		{
			StringBuilder sb = new StringBuilder( 200 );
			sb.Append( "Illegal column name:" ).Append(columnDef.Name);
			sb.Append( " - Allowable values are:" );
			sb.Append( "SuccessCount, FailInitCount, FailSetupCount, FailTestCount, " );
			sb.Append( "FailCleanupCount, NotExistCount, RunId, TotalCount" );
			throw new InputException( sb.ToString() );
		}
	}





}

}
