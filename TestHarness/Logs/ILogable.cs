
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Logs {

/// <summary>
/// Interface for a loggable object. The derived objects will log the test results of ITestable
/// and ILogable derived objects.
/// </summary>
public interface ILogable : IQueryable {


    /// <summary>Write the header for the log output.</summary>
    /// <returns>true if successful, otherwise false.</returns>
	bool WriteHeader();


    /// <summary>Log the testCase information.</summary>
    /// <param name="testable">The testCase object to be logged.</param>
    /// <returns>true if successful, otherwise false.</returns>
    /// <exception cref="InputException"
	bool LogTestable(ITestable testable);


    /// <summary>Write the summary of the logging to the summary log output.</summary>
    /// <returns>true if successful, otherwise false.</returns>
	bool Summarize();


    /// <summary>
    /// Write a summary entry from an ILogable object. The loggable objects contain the 
    /// accumulated totals of test results.
    /// </summary>
    /// <param name="logable">true if successful, otherwise false .</param>
    /// <returns>true if successful, otherwise false.</returns>
	bool WriteSummaryEntry(ILogable logable);

	
    /// <summary>Closes down the log to cleanup loose ends.</summary>
	void Close();


}

}
