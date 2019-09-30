using ChkUtils.Net.ErrObjects;
using LogUtils.Net;
using LogUtils.Net.Interfaces;
using System;


namespace LogUtils.Net {

    /// <summary>Console writer specific to Windows</summary>
    public class OsConsoleWriter : I_OS_ConsoleWriter {

        public void WriteToConsole(string logLine) {
            Console.WriteLine(logLine);
        }

        public void WriteToConsole(MsgLevel level, ErrReport report) {
            if (report.StackTrace.Length > 0) {
                Console.WriteLine("{0} {1}\t{2}\t{3}.{4} - {5}{6}{7}", report.TimeStamp.ToString("h:mm:ss fff"), report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg, Environment.NewLine, report.StackTrace);
            }
            else {
                Console.WriteLine("{0} {1}\t{2}\t{3}.{4} - {5}", report.TimeStamp.ToString("h:mm:ss fff"), report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg);
            }
        }
    }
}
