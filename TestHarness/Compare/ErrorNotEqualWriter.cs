using System;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {


/// <summary>Formats and writes not equal error message to buffer.</summary>
public class ErrorNotEqualWriter : ErrorEqualWriter {


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    /// <param name="userMsg">A user message to add to the error message.</param>
    public ErrorNotEqualWriter(ITestable testable, String userMsg) 
        : base(testable, userMsg) { 
    }


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    public ErrorNotEqualWriter(ITestable testable)
        : base(testable) {
    }


    /// <summary>
    /// On equality of operation this will write a not expected error message.
    /// </summary>
    /// <typeparam name="T">First Generic type in comparison.</typeparam>
    /// <typeparam name="T2">Seconf Generic type in comparison.</typeparam>
    /// <param name="areEqual">Equal condition of operation evaluated.</param>
    /// <param name="notExpected">Value that is not expected.</param>
    /// <param name="actual">Actual value.</param>
    /// <returns>true if the operation is NOT equal, otherwise false.</returns>
    public override bool WriteOnError<T, T2>(bool areEqual, T notExpected, T2 actual) {
        // We loged the error from a common set of evaluation calls by flipping the 
        // equal success.
        if (areEqual) {
            testable.MsgBuffer.Append("Received not expected value:").Append(actual);
            this.Write(userMsg);
        }
        return areEqual;
    }

}

}
