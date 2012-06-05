using System;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {

public class ErrorEqualVerboseWriter : VerboseMsgWriter {

    protected String userMsg;


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    /// <param name="userMsg">A user message to add to the error message.</param>
    public ErrorEqualVerboseWriter(ITestable testable, String userMsg)
        : base(testable) {
        this.userMsg = userMsg;
    }


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    public ErrorEqualVerboseWriter(ITestable testable)
        : this(testable, "") {
    }


    /// <summary>
    /// On failure of operation this will write an error message.
    /// </summary>
    /// <typeparam name="T">First Generic type in comparison.</typeparam>
    /// <typeparam name="T2">Seconf Generic type in comparison.</typeparam>
    /// <param name="success">success condition of operation evaluated.</param>
    /// <param name="expected">Value that is expected.</param>
    /// <param name="actual">Actual value.</param>
    /// <returns>true if the operation is successful, otherwise false.</returns>
    public override bool WriteOnError<T, T2>(bool success, T expected, T2 actual) {
        if (!success) {
            if (this.GetTickCount() >= 1) {
                this.Write(this.userMsg);
            }

            if (testable.VerboseBuffer.Length > 0) {
                testable.VerboseBuffer.AppendLine("");
            }

            testable.VerboseBuffer
                .AppendFormat("On Iteration:{0,-5}",this.GetTickCount())
                .AppendFormat("Expected:{0,-10}", expected)
                .Append("  Actual:").Append(actual);
        }
        return success;
    }


}

}
