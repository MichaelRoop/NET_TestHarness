
namespace Ca.Roop.TestHarness.Core.Test {

    /// <summary>Status indicator for the test case</summary>
    public enum TestStatus {
        NONE,		        /// No status. Default.
        SUCCESS,		    /// Test was successful.
        FAIL_INIT,	        /// Test failed when init was called.
        FAIL_SETUP,	        /// Test failed on setup.
        FAIL_TEST,	        /// Test failed.
        FAIL_CLEANUP,       /// Test failed on cleanup.
        FAIL_BY_EXCEPTION,  /// Test failed by uncaught exception.
        FAIL_BY_ERROR,      /// Test failed by uncaught error.
        NOT_EXISTS	        /// Test with unique id does not exist.
    }

}
