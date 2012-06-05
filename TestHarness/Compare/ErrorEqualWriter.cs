using System;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {


/// <summary>Formats and writes equal error message to buffer.</summary>
public class ErrorEqualWriter : MsgBufferWriter {

    protected String userMsg;


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    /// <param name="userMsg">A user message to add to the error message.</param>
    public ErrorEqualWriter(ITestable testable, String userMsg) : base(testable) {
        this.userMsg = userMsg;
    }


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    public ErrorEqualWriter(ITestable testable)
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
            testable.MsgBuffer
                .Append("Expected:").Append(expected).Append("  Actual:").Append(actual);
            this.Write(this.userMsg);
        }
        return success;
    }

}

}
