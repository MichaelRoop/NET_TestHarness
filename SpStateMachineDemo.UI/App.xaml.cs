using ChkUtils.Net;
using ChkUtils.Net.ErrObjects;
using LogUtils.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpStateMachineDemo.UI {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        log4net.ILog loggerImpl = null;


        public App() {

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            this.loggerImpl = log4net.LogManager.GetLogger(typeof(App));

            Log.SetStackTools(new StackTools());
            WrapErr.SetStackTools(new StackTools());
            Log.SetVerbosity(MsgLevel.Info);
            Log.SetMsgNumberThreshold(5);
            Log.OnLogMsgEvent += new LogingMsgEventDelegate(this.Log_OnLogMsgEvent);


            Log.Warning(0, "---------------------Started-----------------------------");
        }


        #region Log Event Handlers

        /// <summary>
        /// Safely pass a Log message to the logger implementation
        /// </summary>
        /// <param name="level">The loging level of the message</param>
        /// <param name="err">The error report object with the information</param>
        void Log_OnLogMsgEvent(MsgLevel level, ErrReport err) {
            if (this.loggerImpl != null) {
                try {
                    string msg = "NO MSG";
                    if (err.StackTrace.Length > 0) {
                        msg = String.Format(
                            "{0}\t{1}\t{2}\t{3}.{4} {5}{6}{7}",
                            err.TimeStamp.ToString("h:mm:ss fff"), this.LogLevelShort(level), err.Code, err.AtClass, err.AtMethod, err.Msg, Environment.NewLine, err.StackTrace);
                    }
                    else {
                        msg = String.Format(
                            "{0}\t{1}\t{2}\t{3}.{4} {5}",
                            err.TimeStamp.ToString("h:mm:ss fff"), this.LogLevelShort(level), err.Code, err.AtClass, err.AtMethod, err.Msg);
                    }

                    switch (level) {
                        case MsgLevel.Info:
                            loggerImpl.Info(msg);
                            break;
                        case MsgLevel.Debug:
                            loggerImpl.Debug(msg);
                            break;
                        case MsgLevel.Warning:
                            loggerImpl.Warn(msg);
                            break;
                        case MsgLevel.Error:
                        case MsgLevel.Exception:
                            loggerImpl.Error(msg);
                            break;
                    }

#if SEND_LOG_TO_DEBUG
                    Debug.WriteLine(msg);
#endif

                }
                catch (Exception e) {
                    WrapErr.SafeAction(() => Debug.WriteLine(String.Format("Exception on logging out message:{0}", e.Message)));
                }
            }
        }


        /// <summary>
        /// Translate log level to single char
        /// </summary>
        /// <param name="level">The log level</param>
        /// <returns>One char level or 'U' is not found</returns>
        private string LogLevelShort(MsgLevel level) {
            switch (level) {
                case MsgLevel.Info: return "I";
                case MsgLevel.Debug: return "D";
                case MsgLevel.Warning: return "W";
                case MsgLevel.Error: return "E";
                case MsgLevel.Exception: return "X";
                default: return "U";
            }
        }

        #endregion




    }
}
