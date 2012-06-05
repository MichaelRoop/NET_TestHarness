using System;
using System.Collections.Generic;
using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Core.Test;
using Ca.Roop.TestHarness.Inputs;
using Ca.Roop.TestHarness.Logs;

namespace Ca.Roop.TestHarness.Engine
{

/// <summary>Dummy testCase to carry the not found designation if lookup fails.</summary>
public class NonExistantTest : TestCase {

    
    /// <summary>Constructor.</summary>
    /// <param name="id">Unique identifier for the test.</param>
    public NonExistantTest(string id) : base(id, "Test not found") {
		this.Status = TestStatus.NOT_EXISTS;
	}

    /// <see cref="TestCase.Test"/>
    public override bool Test()	{ return true; }
};
//-------------------------------------------------------


/// <summary>A Singleton engine to drive the test case architecture.</summary>
    public sealed class TestEngine 
{

    private Dictionary<String, ITestable>   testCases;
    private LogEngine                       logEngine; 
    private String                          runId;


    // Unique instance of test engine.
    private static readonly TestEngine instance = new TestEngine();


    /// <summary>Static method to retrieve the unique instance of this class.</summary>
    /// <returns>The unique instance of the test engine.</returns>
    public static TestEngine GetInstance()
    {
        return instance;
    }


    // Hidden default contructor.
    private TestEngine() {
        this.testCases  = new Dictionary<String, ITestable>();
        this.logEngine  = new LogEngine();
        this.runId      = "";
    }


    /// <summary>Register a test case to the system for later recall.</summary>
    /// <param name="testable">The testable to register.</param>
    /// <exception cref="ArgumentNullException">On null testable.</exception>
    public void RegisterCase(ITestable testable)
    {
        // TODO Trace - if any of these exceptions are thrown we get an error that exception
        // thrown by target of an invokation.  
        // The exception is being thrown in the same assembly and not the loaded dll so I 
        // am not sure what is going on.
        // However, it is being thrown out of the TestHarness.dll and caught in the ConsoleTester.exe
        // so there may be something there.
        if (testable == null) {
            throw new ArgumentNullException( "newCase", "Param cannot be null" );
        }
        else if (this.testCases.ContainsKey(testable.Id)) {
            throw new ArgumentException("Cannot register duplicate test ID", testable.Id);
        }
        this.testCases.Add(testable.Id, testable);
    }


    /// <summary>Process the test cases using the TextScriptReader object.</summary>
    /// <param name="theReader">Object that parses the script that drives test selection.</param>
    public void ProcessScript(IScriptReader theReader)
    {
        this.logEngine.WriteHeaders();
        TestInfo info = theReader.GetNextTest();
        while (info.IsValid) {
            this.ProcessCase(info);
            info = theReader.GetNextTest();
        } 
        this.logEngine.WriteSummaries();
    }


    /// <summary>Retrieve the results logging engine.</summary>
    /// <returns>The results logging engine.</returns>
	public LogEngine GetLogEngine()
    {
        return this.logEngine;
    }


    /// <summary>The unique identifier for the test run.</summary>
    /// <returns>The run set unique identifier.</returns>
    public String GetRunId() {
        if (this.runId.Length == 0) {
            this.runId = DateTime.Now.ToString("yyyyMMddHHmmss"); 
        }
        return this.runId;
    }


    /// <summary>Resets the TestEngine to be able to run a new test set with fresh data.</summary>
    public void PurgeAll() {
        this.testCases.Clear();
        this.GetLogEngine().CloseAll();
		this.GetLogEngine().PurgeAll();
        this.runId = "";
	}


    /// <summary>Finds the test in the list or returns a special NonExistantTest object.</summary>
    /// <param name="id">Unique identifier for the test case.</param>
    /// <returns>A valid test case if found, otherwise a NonExistantTest instance.</returns>
    private ITestable GetTest(String id) {
        ITestable t = null;
        if (testCases.TryGetValue(id, out t)) {
            return t;
        }
		return new NonExistantTest(id);
	}


    /// <summary>Process one testCase based on information contained in the Info.</summary>
    /// <param name="info">The test case metadata.</param>
    private void ProcessCase(TestInfo info) {
        ITestable t = this.GetTest(info.Id);
        t.ExecuteInit(info.Arguments);
        t.ExecuteSetup();
        t.ExecuteTest();
        t.ExecuteCleanup();
        this.LogResults(t);
        // remove it from the list.
        //m_cases.Remove(tc);
    }


    /// <summary>Log the results of one test case across log outputs.</summary>
    /// <param name="testable">The test case to be logged.</param>
    private void LogResults(ITestable testable) {
        if (testable == null) {
            throw new ArgumentNullException("theCase", "theCase cannot be null");
        }
        this.logEngine.Log(testable);
    }


}


} 
  
// end namespace
