using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ca.Roop.TestHarness.Core.Test {

    public class TestHarnessConsole {

        private bool useWaitOnKey = false;



        public TestHarnessConsole()
            : this(false) { 
        }


        public TestHarnessConsole(bool useWaitOnkey) {
            this.useWaitOnKey = useWaitOnkey;
        }


        public void UseWaitOnKey(bool useWaitOnKey) {
            this.useWaitOnKey = useWaitOnKey;
        }


        public void Execute(string[] args) {

            if (ValidateArgs(args)) {
                try {
                    TestSetProcessor p = new TestSetProcessor(args[0]);
                    p.ProcessSets();
                }
                catch (Exception e) {
                    System.Console.WriteLine("Caught Exception" + e.GetType().Name);
                    System.Console.WriteLine(e.Message);
                    System.Console.WriteLine("==============================");
                    DumpStackTrace(e);
                }
            }


            if (this.useWaitOnKey) {
                System.Console.WriteLine("Hit any key to terminate");
                System.Console.ReadKey();
            }

        }


        /// <summary>Print stack trace by depth first traversal.</summary>
        /// <param name="e">The current level of exception to check.</param>
        private void DumpStackTrace(Exception e) {
            if (e.InnerException != null) {
                DumpStackTrace(e.InnerException);
            }
            System.Console.WriteLine(e.StackTrace);
        }


        private bool ValidateArgs(string[] args) {
            if (args.Length < 1 || args[0].Length == 0) {
                System.Console.WriteLine(
                    "Invalid command. Required Format: 'ConsoleTester TestSetXmlFile.xml'");
                return false;
            }
            return true;
        }

    }

}
