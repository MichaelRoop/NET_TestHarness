using LogUtils.Net;
using NUnit.Framework;
using System;
using System.Diagnostics;
using TestCases.TestToolSet.Net;

namespace TestCases.LogUtilsTests.Net {

    [TestFixture]
    public class LogPerformanceTests {

        #region Data

        HelperLogReaderNet logReader = new HelperLogReaderNet();

        #endregion

        #region Setup

        [SetUp]
        public void TestSetup() {
            this.logReader.StartLogging();
        }

        [TearDown]
        public void TestTeardown() {
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion
        
        #region Info message injection

        [Test]
        public void Info_Compare_MessageInjection() {
            this.logReader.SetVerbosity(MsgLevel.Error);

            Stopwatch sw = new Stopwatch();
            double dbl = 32.98;
            int i = 99;
            string str1 = "str1";
            string str2 = "str2";

            double ts1 = 0.0;
            double ts2 = 0.0;

            sw.Start();
            for (int j = 0; j < 1000; j++) {
                Log.Info("ThisClass", "ThisMethod",
                    String.Format("This a string made up of a double:{0} an int:{1} and two strings '{2}' and '{3}'",
                    dbl, i, str1, str2));
            }
            sw.Stop();
            //Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);
            ts1 = sw.Elapsed.TotalMilliseconds;
            sw.Reset();

            // - formater version
            sw.Start();
            for (int j = 0; j < 1000; j++) {
                Log.Info("ThisClass", "ThisMethod", () => {
                    return
                        String.Format("This a string made up of a double:{0} an int:{1} and two strings '{2}' and '{3}'", dbl, i, str1, str2);
                });
            }
            sw.Stop();
            ts2 = sw.Elapsed.TotalMilliseconds;
            //Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);


            Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", ts1);
            Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", ts2);
            Console.WriteLine("{0}x faster", (ts1 / ts2));



            // Using the Func injector is 27.7x faster
            // Straight formatted time for message when not verbose enough to actually be logged:4.4265
            // Func Msg formatted time for message when not verbose enough to actually be logged:0.1598

        }


        [Test]
        public void Info_Compare_MessageInjectionInMethods() {
            this.logReader.SetVerbosity(MsgLevel.Error);

            double ts1 = 0.0;
            double ts2 = 0.0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int j = 0; j < 1000; j++) {
                this.Method_With_MsgFormating();
            }
            sw.Stop();
            ts1 = sw.Elapsed.TotalMilliseconds;
            //Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);
            sw.Reset();

            sw.Start();
            for (int j = 0; j < 1000; j++) {
                this.Method_With_MsgFormatingInjected();
            }
            sw.Stop();
            ts2 = sw.Elapsed.TotalMilliseconds;
            //Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);

            Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", ts1);
            Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", ts2);
            Console.WriteLine("{0}x faster", (ts1 / ts2));



            // 52.8x Faster ** More significant difference when they are in separate methods.
            // Straight formatted time for message when not verbose enough to actually be logged:1418.6824
            // Func Msg formatted time for message when not verbose enough to actually be logged:26.891
        }




        [Test]
        public void Info_Compare_MessageInjectionNoFormating() {
            this.logReader.SetVerbosity(MsgLevel.Error);

            double ts1 = 0.0;
            double ts2 = 0.0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int j = 0; j < 1000; j++) {
                Log.Info("ThisClass", "ThisMethod", "This a simple fixed length string with no formating whatever");
            }
            sw.Stop();
            ts1 = sw.Elapsed.TotalMilliseconds;
            //Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);

            sw.Reset();

            sw.Start();
            for (int j = 0; j < 1000; j++) {
                Log.Info("ThisClass", "ThisMethod", () => {
                    return "This a string made up of a double:{0} an int:{1} and two strings '{2}' and '{3}'";
                });
            }
            sw.Stop();
            ts2 = sw.Elapsed.TotalMilliseconds;
            //Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", sw.Elapsed.TotalMilliseconds);

            Console.WriteLine("Straight formatted time for message when not verbose enough to actually be logged:{0}", ts1);
            Console.WriteLine("Func Msg formatted time for message when not verbose enough to actually be logged:{0}", ts2);
            Console.WriteLine("{0}x faster", (ts1 / ts2));

        }

        // Even with fixed strings, using the Func injector is 3.4x faster
        // Straight formatted time for message when not verbose enough to actually be logged:0.5243
        // Func Msg formatted time for message when not verbose enough to actually be logged:0.154

        #endregion


        private void Method_With_MsgFormating() {
            this.logReader.SetVerbosity(MsgLevel.Error);

            double dbl = 32.98;
            int i = 99;
            string str1 = "str1";
            string str2 = "str2";

            Log.Info("ThisClass", "ThisMethod",
                String.Format("This a string made up of a double:{0} an int:{1} and two strings '{2}' and '{3}'",
                dbl, i, str1, str2));
        }


        private void Method_With_MsgFormatingInjected() {
            this.logReader.SetVerbosity(MsgLevel.Error);

            double dbl = 32.98;
            int i = 99;
            string str1 = "str1";
            string str2 = "str2";

            Log.Info("ThisClass", "ThisMethod", () => {
                return String.Format(
                    "This a string made up of a double:{0} an int:{1} and two strings '{2}' and '{3}'",
                    dbl, i, str1, str2);
            });
        }





    }
}
