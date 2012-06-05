
namespace Ca.Roop.TestHarness.Compare {

/// <summary>Encapsulates different writing to buffers.</summary>
public interface IBufferWriter {


    /// <summary>On failure of operation write an error message.</summary>
    /// <typeparam name="T">First Generic type in comparison.</typeparam>
    /// <typeparam name="T2">Second Generic type in comparison.</typeparam>
    /// <param name="success">success condition of operation evaluated.</param>
    /// <param name="expected">Value that is expected.</param>
    /// <param name="actual">Actual value.</param>
    /// <returns>true if the operation is successful, otherwise false.</returns>
    bool WriteOnError<T, T2>(bool success, T expected, T2 actual);


    /// <summary>Write to the buffer.</summary>
    /// <param name="msg">Message string to write.</param>
    void Write(string msg);


    /// <summary>Flag whether you stop writing after first error.</summary>
    /// <returns>true is only one error is to be written, otherwise false.</returns>
    bool StopOnFirstError();


    /// <summary>
    /// Users can tick the writer. Useful in derived classes when tracking multiple iterations.
    /// </summary>
    void Tick();
}

}
