using ChkUtils.Net.ErrObjects;
using System;

namespace LogUtils.Net {

    /// <summary>Used as a writer to NUnit Output window in VS</summary>
    /// <author>Michael Roop</author>
    /// <copyright>August 2019 Michael Roop Used by permission</copyright> 
    public class NUnitTraceWriter {

        #region Data

        Action<MsgLevel, ErrReport> onMsgLogged = null;
        private bool connected = false;

        #endregion

        #region Constructors destructors

        public NUnitTraceWriter() {
            this.onMsgLogged = new Action<MsgLevel, ErrReport>(this.LogToConsole);
        }

        ~NUnitTraceWriter() {
            this.StopLogging();
        }

        #endregion

        #region Public

        /// <summary>Start the logging if not already started</summary>
        public void StartLogging() {
            if (!this.connected) {
                Log.OnLogMsgEvent += new LogingMsgEventDelegate(this.onMsgLogged);
                this.connected = true;
            }
        }


        /// <summary>Stop the logging if not already stoped</summary>
        public void StopLogging() {
            if (this.connected) {
                Log.OnLogMsgEvent -= new LogingMsgEventDelegate(this.onMsgLogged);
                this.connected = false;
            }
        }

        #endregion

        #region Private

        private void LogToConsole(MsgLevel level, ErrReport report) {
            if (report.StackTrace.Length > 0) {
                System.Diagnostics.Trace.WriteLine(String.Format("{0:00000}\t{1}\t{2}.{3} - {4}{5}{6}", report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg, Environment.NewLine, report.StackTrace));
            }
            else {
                System.Diagnostics.Trace.WriteLine(String.Format("{0:00000}\t{1}\t{2}.{3} - {4}", report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg));
            }
        }

        #endregion

    }
}
