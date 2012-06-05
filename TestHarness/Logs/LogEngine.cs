//--------------------------------------------------------------------------------------
/// @file	LogEngine.h
/// @brief	Engine to drive the logging activity.
///
/// @author		Michael Roop
/// @date		2010
/// @version	1.0
///
/// Copyright 2010 Michael Roop
//--------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs.Initialisers;

namespace Ca.Roop.TestHarness.Logs
{
    /// <summary>
    /// Drives the logging of testCase information across a variable number of derived Loggers.
    /// </summary>
    public class LogEngine {

        protected List<ILogable> logs;
        
        
        /// <summary>Constructor.</summary>
        public LogEngine() {
            logs = new List<ILogable>();
        }


        /// <summary>Write the header information across all loggers.</summary>
        /// <returns>true if successful, otherwise false.</returns>
        public bool WriteHeaders() {
            foreach (ILogable log in this.logs) {
                log.WriteHeader();
            }
            return true;
        }


        /// <summary>Write the summary information across all loggers.</summary>
        /// <returns>true if successful, otherwise false.</returns>
        public bool WriteSummaries() {
            foreach (ILogable log in this.logs) {
                log.Summarize();
            }
            return true;
        }


        /// <summary>Log the test case information across all loggers.</summary>
        /// <param name="testable">The testable object to run and log.</param>
        /// <returns>true if successful, otherwise false.</returns>
        public bool Log(ITestable testable) {
            if (testable == null) {
                throw new ArgumentNullException("testable", "theCase cannot be null");
            }

            foreach (ILogable log in this.logs) {
                log.LogTestable(testable);
            }
            return true;
        }


        /// <summary>Add a logger object to the logger group.</summary>
        /// <param name="log">The logable object to add.</param>
        public void AddLog(ILogable log) {
            if (log == null) {
                throw new ArgumentNullException("log", "logger cannot be null");
            }
            logs.Add(log);
        }


        /// <summary>Load test sets from an initialisation file.
        /// 
        /// </summary>
        /// <param name="info">The test sets metadata.</param>
	    public void loadLoggers(TestSetInfo info) {
            ILogInitialiser init = InitialiserFactory.Create(info);
            init.Load();

            LogInfo logInfo = init.GetNextLog();
            while (logInfo.IsValid()) {
                this.AddLog(LogFactory.Create(logInfo));
                logInfo = init.GetNextLog();
            }
        }

    	
        /// <summary>Cleanup method for all the held log objects.</summary>
        /// <exception cref="InputException"/>
	    public void CloseAll() {
            foreach (ILogable log in logs) {
                log.Close();
            }
        }
    	
    	
        /// <summary>Purge all held outputs after calling close on them.</summary>
        /// <exception cref="InputException"/>
	    public void PurgeAll() {
            this.CloseAll();
            logs.Clear();
        }


    };

}//end namespace
