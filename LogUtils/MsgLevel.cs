
namespace LogUtils {

    /// <summary>
    /// Used to set the message level as well as the verbosity level to log
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public enum MsgLevel : int {

        /// <summary>
        /// Most verbose message only logs if level set to Info
        /// </summary>
        Info,

        /// <summary>
        /// Second most verbose message only logs if logger level set to 
        /// Debug, Info
        /// </summary>
        Debug,

        /// <summary>
        /// Warning message only logs if logger level set to Debug, Info, 
        /// Warning
        /// </summary>
        Warning,

        /// <summary>
        /// Error message only logs if logger level set to Debug, Info, 
        /// Warning, Error
        /// </summary>
        Error,

        /// <summary>
        /// Error message only logs if logger level set to Debug, Info, 
        /// Warning, Error, Critical
        /// </summary>
        Critical,

        /// <summary>
        /// Error message only logs if logger level set to Debug, Info, 
        /// Warning, Error, Critical, Exception
        /// </summary>
        Exception,

        /// <summary>
        /// Used only for verbosity levels not log messages. If logger set to 
        /// Off, no messages will log
        /// </summary>
        Off,

    }


    /// <summary>
    /// Extension class for MsgLevel enum
    /// </summary>
    public static class MsgLevelExtensions {

        /// <summary>
        /// Short form of the above levels. Warning, it must match order and number
        /// </summary>
        private static string[] shortName = new string[] { "I", "D", "W", "E", "C", "X", "O" };

        /// <summary>
        /// Returns the one letter equivalent of the level name
        /// </summary>
        /// <param name="self">The MsgLevel enum</param>
        /// <returns>A one character string name</returns>
        public static string ShortName(this MsgLevel self) {
            return MsgLevelExtensions.shortName[(int)self];
        }


        /// <summary>
        /// Do a check that the enum value is at least as high as compared to another to 
        /// determine if the logging can proceed
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool GreaterOrEqual(this MsgLevel self, MsgLevel other) {
            return self >= other;
        }

    }


}
