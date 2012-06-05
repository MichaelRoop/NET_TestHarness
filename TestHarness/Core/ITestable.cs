using System;
using System.Collections.Generic;
using System.Text;
using Ca.Roop.TestHarness.Core.Test;

namespace Ca.Roop.TestHarness.Core
{
    /// <summary>
    /// Interface for all test cases processes by the TestEngine. 
    /// <p>
    /// The derived objects are processed by the TestEngine and queried by ILogable derived 
    /// objects to assemble the test case information.
    /// <p>
    /// Test case objects are constructed with a unique id string which is used to identify
    /// and retrieve the test.  It is also constructed with a description string.
    /// <p>
    /// When the test case is retrieved, the arguments can be fed to it by means of the 
    /// executeInit method.
    /// <p>
    /// There are three stages to a test case, the setup, the test, and the cleanup.
    /// </summary>
    public interface ITestable : IQueryable {

        /// <summary>Unique identifier for test.</summary>
        String Id { get; }


        /// <summary>Test status.</summary>
        TestStatus Status { get; }


        /// <summary>Buffer to hold messages to log from test cases.</summary>
        StringBuilder MsgBuffer { get; }


        /// <summary>Buffer to hold vorbose messages to log from test cases.</summary>
        StringBuilder VerboseBuffer { get; }


        /// <summary>Called by the engine when the test is selected.</summary>
        /// <remarks>
        /// Allows the test to be created before the arguments for the test are known. In this
        /// way a test case code can be used in different ways depending on the arguments that
        /// are read in at select time.  Part of the arguments can also be expected return value.
        /// <p>
        /// The test is instantiated and stored.  When it is selected, the arguments are know from
        /// the same script from which is was selected.
        /// </remarks>
        /// <param name="args">A list of Test arguments.</param>
        /// <returns>true if sucessful, otherwise false.</returns>
        bool ExecuteInit(List<TestArg> args);


        /// <summary>Executes and times the test setup. </summary>
        /// <returns>true if successful, otherwise false.</returns>
        bool ExecuteSetup();


        /// <summary>Executes and times the test itself. </summary>
        /// <returns>true if successful, otherwise false.</returns>
        bool ExecuteTest();

        /// <summary>Executes and times the test cleanup. </summary>
        /// <returns>true if successful, otherwise false.</returns>
        bool ExecuteCleanup();

    }
}
