using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.Utilities;

namespace Ca.Roop.TestHarness.Logs {


/// <summary>Constructs the appropriate derived Log according to Log type.</summary>
public class LogFactory {

    /// <summary>
    /// Creates the appropriate ILoggable derived object.
    /// </summary>
    /// <param name="info">The data required to build the object.</param>
    /// <returns></returns>
    public static ILogable Create(LogInfo info) {
        switch (info.Type) {
        case LogType.SQL:
            return new SqlLog(info);
        case LogType.TEXT:
            return new TextLog(info);
        case LogType.UNDEFINED:
            return null;
        default:
            throw new InputException(InvalidEnumMessage.Get(info.Type));
        }
    }
}


}
