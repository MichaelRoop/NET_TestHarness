using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ErrObjects;

namespace LogUtils {

    public class ConsoleWriter {

        Action<MsgLevel, ErrReport> onMsgLogged = null;
        private bool connected = false;


        public ConsoleWriter() {
            this.onMsgLogged = new Action<MsgLevel, ErrReport>(this.LogToConsole);
        }

        ~ConsoleWriter() {
            this.StopLogging();
        }

        public void StartLogging() {
            if (!this.connected) {
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
        
        private void LogToConsole(MsgLevel level, ErrReport report) {
            if (report.StackTrace.Length > 0) {
                Console.WriteLine("{0}\t{1}\t{2}.{3} - {4}{5}{6}", report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg, Environment.NewLine, report.StackTrace);
            }
            else {
                Console.WriteLine("{0}\t{1}\t{2}.{3} - {4}", report.Code, level.ShortName(), report.AtClass, report.AtMethod, report.Msg);
            }
        }


    }
}
