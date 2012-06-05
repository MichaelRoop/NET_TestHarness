using Ca.Roop.TestHarness.Compare;

namespace Ca.Roop.TestHarness.Core {

public class LogCompare {

    /// <summary>
    /// Compares two generic values and write a message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <param name="msg">An extra user message</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestEqual<T, T2>(T expected, T2 actual, ITestable testable, string msg) {
        return Comparator.IsEqualTo(expected, actual, new ErrorEqualWriter(testable, msg));
    }


    /// <summary>
    /// Compares two generic values and write a message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestEqual<T, T2>(T expected, T2 actual, ITestable testable) {
        return TestEqual(expected, actual, testable, "");
    }


    /// <summary>
    /// Compares two generic values for NOT equal and write a message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <param name="msg">An extra user message</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestNotEqual<T, T2>(T notExpected, T2 actual, ITestable testable, string msg) {
        return Comparator.IsNotEqualTo(notExpected, actual, new ErrorNotEqualWriter(testable, msg));
    }


    /// <summary>
    /// Compares two generic values for NOT equal and write a message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestNotEqual<T, T2>(T notExpected, T2 actual, ITestable testable) {
        return TestNotEqual(notExpected, actual, testable, "");
    }


    /// <summary>
    /// Compares two generic values and write a verbose message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <param name="msg">An extra user message</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestEqualVerbose<T, T2>(T expected, T2 actual, ITestable testable, string msg) {
        return Comparator.IsEqualTo(expected, actual, new ErrorEqualVerboseWriter(testable, msg));
    }


    /// <summary>
    /// Compares two generic values and write a verbose message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestEqualVerbose<T, T2>(T expected, T2 actual, ITestable testable) {
        return TestEqualVerbose(expected, actual, testable, "");
    }


    /// <summary>
    /// Compares two generic values for NOT equal and write a verbose message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <param name="msg">An extra user message</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestNotEqualVerbose<T, T2>(T expected, T2 actual, ITestable testable, string msg) {
        return Comparator.IsNotEqualTo(expected, actual, new ErrorNotEqualVerboseWriter(testable, msg));
    }


    /// <summary>
    /// Compares two generic values for NOT equal and write a verbose message when values are different.
    /// </summary>
    /// <typeparam name="T">The generic first type.</typeparam>
    /// <typeparam name="T2">The generic second type.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testable">The testable object</param>
    /// <returns>true if the test succeeded, otherwise false.</returns>
    public static bool TestNotEqualVerbose<T, T2>(T expected, T2 actual, ITestable testable) {
        return TestNotEqualVerbose(expected, actual, testable, "");
    }

}

}
