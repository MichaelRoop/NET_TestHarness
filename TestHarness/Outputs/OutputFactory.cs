using Ca.Roop.TestHarness.Logs;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Outputs {

/// <summary>Creates the appropriate IOutputable derived object.</summary>
class OutputFactory {

    /// <summary>Creates the appropriate IOutputable derived object.</summary>
    /// <param name="info">The data necessary to create the object.</param>
    /// <returns>The IOutputable object.</returns>
    /// <exception cref="InputException"/>
    public static IOutputable Create(LogInfo info) {
        switch (info.OutputData.Type) {
        case OutputType.CONSOLE:
            return new ConsoleOutput();
        case OutputType.FILE:
            return new FileOutput(info);
        case OutputType.ODBC:
            return new OdbcOutput(info);
        case OutputType.EMAIL:
            return new EmailOutput(info);
        default:
            throw new InputException(InvalidEnumMessage.Get(info.OutputData.Type));
        }
    }


}

}
