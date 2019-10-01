using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LogUtils;
using ChkUtils.ErrObjects;
using System.Threading;


namespace TestCases.LogUtilsTests {

    [TestFixture]
    public class LogTests {

        #region Data

        MsgLevel currentLevel = MsgLevel.Off;
        ErrReport currentReport = new ErrReport();
        NUnitTraceWriter consoleWriter = new NUnitTraceWriter();

        #endregion

        #region Setup 

        // This is how they write to console with the VS test platform
        // https://xunit.net/docs/capturing-output
        //private readonly ITestOutputHelper output;


        //public MyTestClass(ITestOutputHelper output) {
        //    this.output = output;
        //}



        public LogTests() {
            Log.OnLogMsgEvent += new LogingMsgEventDelegate(Log_OnLogMsgEvent);

        }

        void Log_OnLogMsgEvent(MsgLevel level, ErrReport errReport) {
            this.currentLevel = level;
            this.currentReport = errReport;
        }

        [OneTimeSetUp]
        public void OneTime() {
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            //For.NET core 2.0, you need to use:
            //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        [OneTimeTearDown]
        public void TearDown() {
            System.Diagnostics.Trace.Flush();
        }


        [SetUp]
        public void Setup() {
            LogUtils.Log.SetVerbosity(MsgLevel.Info);
            LogUtils.Log.SetMsgNumberThreshold(1);
            this.consoleWriter.StartLogging();

            this.currentLevel = MsgLevel.Off;
            this.currentReport = new ErrReport();
        }
        
        #endregion

        #region Valid Calls

        [Test]
        public void Info_Valid() {
            Log.Info("LogTests", "Info_Valid", "This is my message");
            this.CheckLogValues(MsgLevel.Info, 0, "Info_Valid", "This is my message", "");
        }

        [Test]
        public void Debug_Valid() {
            Log.Debug("LogTests", "Debug_Valid", "This is my message");
            this.CheckLogValues(MsgLevel.Debug, 0, "Debug_Valid", "This is my message", "");
        }

        [Test]
        public void Warning_Valid() {
            Log.Warning(12345, "This is my warning string");
            this.CheckLogValues(MsgLevel.Warning, 12345, "Warning_Valid", "This is my warning string", "");
        }

        [Test]
        public void Error_Valid() {
            Log.Error(8744, "This is my error string");
            this.CheckLogValues(MsgLevel.Error, 8744, "Error_Valid", "This is my error string", "");
        }

        [Test]
        public void Critical_Valid() {
            Log.Critical(8989, "This is my critical error string");
            this.CheckLogValues(MsgLevel.Critical, 8989, "Critical_Valid", "This is my critical error string", "");
        }

        [Test]
        public void Exception_Valid() {
            try {
                throw new Exception("Blah blah exception");
            }
            catch (Exception e) {
                Log.Exception(1000, "This is my exception error string", e);
                this.CheckLogValues(MsgLevel.Exception, 1000, "Exception_Valid", "This is my exception error string", "Blah blah exception");
            }
        }

        #endregion

        #region Level Restricted Verbosity Calls

        [Test]
        public void Info_Restricted() {
            Log.SetVerbosity(MsgLevel.Debug);
            Log.Info("LogTests", "Info_Valid", "This is my message");
            this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");

            Log.SetVerbosity(MsgLevel.Info);
            this.Info_Valid();
        }

        [Test]
        public void Debug_Restricted() {
            Log.SetVerbosity(MsgLevel.Warning);
            Log.Debug("LogTests", "Debug_Valid", "This is my message");
            this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");
            
            Log.SetVerbosity(MsgLevel.Debug);
            this.Debug_Valid();
        }

        [Test]
        public void Warning_Restricted() {
            Log.SetVerbosity(MsgLevel.Error);
            Log.Warning(1233, "This is my message SHOULD NOT SHOW");
            this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");

            Log.SetVerbosity(MsgLevel.Warning);
            this.Warning_Valid();
        }

        [Test]
        public void Error_Restricted() {
            Log.SetVerbosity(MsgLevel.Critical);
            Log.Error(1233, "This is my message");
            this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");

            Log.SetVerbosity(MsgLevel.Error);
            this.Error_Valid();
        }


        [Test]
        public void Critical_Restricted() {
            Log.SetVerbosity(MsgLevel.Exception);
            Log.Critical(1233, "This is my message");
            this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");

            // Few tests
            // Does not show at all
            //Console.WriteLine("Console.WriteLine");
            //TestContext.WriteLine($"*** TestContext.WriteLine");
            //TestContext.Write("TestContext.Write");
            //TestContext.Out.WriteLine($"*** TestContext.Out.WriteLine");

            // Only show when you 'Debug Selected Tests'
            //System.Diagnostics.Debug.WriteLine("*** Debug.WriteLine");
            //System.Diagnostics.Trace.WriteLine("*** Trace.WriteLine");

            // Show in the Output window with [warning]
            //TestContext.Error.WriteLine($"*** TestContext.Error.WriteLine");
            // Does not show at all

            // To make everything work we use the 'debug all tests, but have it in release configuration 
            // to prevent the debugger from stopping the process on first error.  The new NUnit Output
            // Writter uses the System.Diagnostics.Trace.WriteLine that will write to Debug output


            Log.SetVerbosity(MsgLevel.Critical);
            this.Critical_Valid();

            //Assert.IsTrue(false, "Just to make it fail");

        }

        [Test]
        public void Exception_Restricted() {
            try {
                Log.SetVerbosity(MsgLevel.Off);
                throw new Exception("Blah blah restricted exception");
            }
            catch (Exception e) {
                Log.Exception(1000, "This is my restricted exception error stack", e);
                this.CheckLogValues(MsgLevel.Off, 0, "", "", "", "");
            }

            Log.SetVerbosity(MsgLevel.Exception);
            this.Exception_Valid();

        }

        #endregion



        #region Private Methods

        public void CheckLogValues(MsgLevel level, int code, string atMethod, string msg, string stack) {
            this.CheckLogValues(level, code, "LogTests", atMethod, msg, stack);
        }

        public void CheckLogValues(MsgLevel level, int code, string atClass, string atMethod, string msg, string stack) {
            Thread.Sleep(250);
            Assert.AreEqual(level, this.currentLevel, "Level Mismatch");
            Assert.AreEqual(code, this.currentReport.Code, "Error Code Mismatch");
            Assert.AreEqual(atClass, this.currentReport.AtClass, "Class Name Mismatch");
            Assert.AreEqual(atMethod, this.currentReport.AtMethod, "Method Name Mismatch");
            Assert.AreEqual(msg, this.currentReport.Msg, "Message Mismatch");

            if (stack.Length == 0) {
                Assert.AreEqual("", this.currentReport.StackTrace, "Stack trace supposed to be empty");
            }
            else {
                Assert.IsTrue(this.currentReport.StackTrace.Contains(stack), String.Format("Stack does not contain '{0}' - {1}", stack, this.currentReport.StackTrace));
            }

        }

        #endregion

        

    }
}
