using ChkUtils.Net.ErrObjects;
using LogUtils.Net.Interfaces;
using System;

namespace LogUtils.Net {

    /// <summary>
    /// Used as a simple writer of messages to console
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class ConsoleWriter {

        Action<MsgLevel, ErrReport> onMsgLogged = null;
        private bool connected = false;

        I_OS_ConsoleWriter writer = null;


        public ConsoleWriter(I_OS_ConsoleWriter writer) {
            this.writer = writer;
            this.onMsgLogged = new Action<MsgLevel, ErrReport>(this.LogToConsole);
        }

        ~ConsoleWriter() {
            this.StopLogging();
        }

        public void StartLogging() {
            this.writer.WriteToConsole("StartLogging");
            if (!this.connected) {
                this.writer.WriteToConsole("LogUtils.ConsoleWriter.StartLogging - event connected");
                Log.OnLogMsgEvent += new LogingMsgEventDelegate(this.onMsgLogged);
                this.connected = true;
            }
        }

        public void StopLogging() {
            if (this.connected) {
                Log.OnLogMsgEvent -= new LogingMsgEventDelegate(this.onMsgLogged);
                this.connected = false;
            }
        }

        public void LogToConsole(MsgLevel level, ErrReport report) {
            this.writer.WriteToConsole(level, report);

            //if (report.StackTrace.Length > 0) {
            //    writer.WriteToConsole(string.Format("{0} {1}\t{2}\t{3}.{4} - {5}{6}{7}", report.TimeStamp.ToString("h:mm:ss fff"), report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg, Environment.NewLine, report.StackTrace));
            //}
            //else {
            //    writer.WriteToConsole(string.Format("{0} {1}\t{2}\t{3}.{4} - {5}", report.TimeStamp.ToString("h:mm:ss fff"), report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg));
            //}
        }


    }
}
