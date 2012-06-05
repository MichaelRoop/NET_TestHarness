using System;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {

class ErrorNotEqualVerboseWriter : ErrorEqualVerboseWriter {

    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    /// <param name="userMsg">A user message to add to the error message.</param>
    public ErrorNotEqualVerboseWriter(ITestable testable, String userMsg) 
        : base(testable, userMsg) { 
    }


    /// <summary>Constructor.</summary>
    /// <param name="testable">The testable to hold error information.</param>
    public ErrorNotEqualVerboseWriter(ITestable testable)
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
            if (this.GetTickCount() <= 1) {
                this.Write(this.userMsg);
            }

            if (testable.VerboseBuffer.Length > 0) {
                testable.VerboseBuffer.AppendLine("");
            }

            if (this.GetTickCount() > 0) {
                testable.VerboseBuffer
                    .AppendFormat("On Iteration:{0,-5}", this.GetTickCount());
            }

            testable.VerboseBuffer
                .AppendFormat("Received not expected value:{0,-10}", actual);
        }
        return areEqual;
    }

}

}
