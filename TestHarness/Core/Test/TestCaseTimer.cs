using System;
using System.Diagnostics;
using Ca.Roop.TestHarness.Compare;

namespace Ca.Roop.TestHarness.Core.Test {


/// <summary>Time test segments and log exception failures.</summary>
class TestCaseTimer {

    /// <summary>
    /// Times the action passed in and sets status in case of exceptions.
    /// </summary>
    /// <param name="action">The method that is being timed.</param>
    /// <param name="timeVal">The holder for the timing</param>
    /// <param name="status">The status to set in the event of an exception.</param>
    /// <param name="testable">The testable object that provides message buffers.</param>
    /// <returns></returns>
    public static bool Time(
        Func<bool> action, 
        ref long timeVal, 
        ref TestStatus status,
        ITestable testable) {

        try {
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool success        = action.Invoke();
            timeVal             = stopwatch.Elapsed.Seconds; //TODO - log Milliseconds; or seconds switch.
            return success;
        }
        catch (Exception e) {
            new MsgBufferWriter(testable).Write(e.Message);
            VerboseMsgWriter writer = new VerboseMsgWriter(testable);
            writer.Write(e.GetType().Name);
            DumpStackTrace(e, writer);
            status = TestStatus.FAIL_BY_EXCEPTION;
            return false;
        }
    }


    /// <summary>Recursive method to print out the full stack trace of inner exceptions.</summary>
    /// <remarks>
    /// The method uses reverse recursion. The most inner will start to write first.
    /// </remarks>
    /// <param name="e">The exception.</param>
    /// <param name="writer">The buffer writer to receive the stack trace.</param>
    private static void DumpStackTrace(Exception e, IBufferWriter writer) {
        if (e.InnerException != null) {
            DumpStackTrace(e.InnerException, writer);
        }
        writer.Write(e.StackTrace.ToString());
    }

}

}
