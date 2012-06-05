using System;
using System.Collections.Generic;

namespace Ca.Roop.TestHarness.Core.Test {


    /// <summary>Test case metadata object. Identifies the test and holds its arguments.</summary>
    public class TestInfo {

        /// <summary>The status of the TestInfo. If not valid it indicates end of list.</summary>
        public bool IsValid { get; private set; }


        /// <summary>Unique identifier for the test case.</summary>
        public string Id    { get; private set; }


        /// <summary>List of test case arguments.</summary>
        public List<TestArg> Arguments { get; private set; }


        /// <summary>
        /// Default constructor creates an invalid test case metadata to act as end of 
        /// list indicatory.
        /// </summary>
        public TestInfo() {
            this.Id         = "";
            this.IsValid    = false;
            this.Arguments = null;
        }


        /// <summary>Constructor.</summary>
        /// <param name="id">Unique identifier for test.</param>
        /// <param name="args">The test case arguments.</param>
        public TestInfo(String id, List<TestArg> args) {
            this.Id         = id;
            this.IsValid    = true;
            this.Arguments  = args;
        }
    };

}
