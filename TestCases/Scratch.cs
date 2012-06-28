using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using System.Xml;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using SpStateMachine.EventListners;
using SpStateMachine.EventStores;
using SpStateMachine.PeriodicTimers;
using System.Threading;
using LogUtils;

namespace TestCases {

    [TestFixture, Explicit]
    public class Scratch {

        //bool onEntryRaised = false;
        //bool onTickRaised = false;
        //bool onExitRaised = false;

        ConsoleWriter consoleWriter = new ConsoleWriter();

        [TestFixtureSetUp]
        public void Setup() {
            Log.SetVerbosity(MsgLevel.Info);

            Log.SetMsgNumberThreshold(1);
            this.consoleWriter.StartLogging();
        }

        //[TearDown]
        //public void Teardown() {
        //    this.consoleWriter.StopLogging();
        //}


        #region Cast from Interface

        public interface ITestInt {
            int Id { get; set; }
        }

        public class ChildBase : ITestInt {
            private int id = 1;

            public int Id {
                get { return id; }
                set { id = value; }
            }

            public ChildBase(int id) {
                this.id = id;
            }
        }

        public class Child1 : ChildBase {
            private int intProp = 99;
            public Child1(int id) : base(id) { }
            public int IntProperty { get { return this.intProp; } set { this.intProp = value; } }
        }

        public class Child2 : ChildBase {
            string stringProp = "Hi";
            public Child2(int id) : base(id) { }
            public string StringProperty { get { return this.stringProp; } set { this.stringProp = value; } }
        }


        [Test, Explicit]
        public void TestCastFromInterface() {

            ITestInt intProp = new Child1(22);
            ITestInt strProp = new Child2(33);
            Console.WriteLine("Child1 cast id:{0} - int prop:{1}", ((Child1)intProp).Id, ((Child1)intProp).IntProperty);
            Console.WriteLine("Child2 cast id:{0} - str prop:{1}", ((Child2)strProp).Id, ((Child2)strProp).StringProperty);

        }


        #endregion

        #region SpState generic

        public class DataClass {
            private int intVal = 0;
            private string strVal = "";

            public int IntVal { get { return this.intVal; } set { this.intVal = value; } }
            public string StrVal { get { return this.strVal; } set { this.strVal = value; } }
        }

        public class MyState : SpState<DataClass> {
            public MyState(DataClass dataClass) : base(dataClass) {
            }

            protected override ISpMessage ExecOnEntry(ISpMessage msg) {
                Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
                return msg;
            }

            protected override ISpMessage ExecOnTick(ISpMessage msg) {
                Log.Info("MyState", "ExecOnTick", String.Format("Raised {0}", msg.EventId));
                return msg;
            }


            protected override ISpMessage ExecOnExit(ISpMessage msg) {
                Log.Info("MyState", "ExecOnExit", String.Format("Raised {0}", msg.EventId));
                return msg;
            }
        
        }

        public class MyStateMachine : SpStateMachine<DataClass> {
            public MyStateMachine(DataClass dataClass, ISpState state)
                : base(dataClass, state) {
            }
        }


        [Test, Explicit]
        public void TestInitialGenericSpState() {
            DataClass dataClass = new DataClass();
            ISpState s = new MyState(dataClass);
            ISpStateMachine sm = new MyStateMachine(dataClass, s);

           // sm.Tick(new BaseMsg(99, 456));

            ISpEventListner listner = new SimpleEventListner();
            listner.ResponseReceived+=new Action<ISpMessage>((msg) => {});
            ISpMessage defaultTickMsg = new BaseMsg(0, 0);

            // TODO - Need a default response msg
            
            SpStateMachineEngine engine = 
                new SpStateMachineEngine(
                    listner,
                    new SimpleDequeEventStore(defaultTickMsg),
                    sm,
                    new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 1000)));

            engine.Start();

            Thread.Sleep(3000);

            //sm.Tick(new BaseMsg(99, 456));

            listner.PostMessage(new BaseMsg(777, 12345));

            Thread.Sleep(3000);
            engine.Stop();

            Console.WriteLine("Disposing Engine - thread should cease while I want");
            engine.Dispose();

            Thread.Sleep(3000);
            Console.WriteLine("Done");


            //SpState<DataClass> state = new SpState<DataClass>(dataClass);
            //SpStateMachine<DataClass> stateMachine = new SpStateMachine<DataClass>(dataClass, state);

        }

        #endregion

        #region Do Scratch

        [Test, Explicit]
        public void DoScratch() {

            try {

              //  int i = Int32.Parse("22r");

                XmlException x = new XmlException("Blah error", null, 25, 100);
                throw x;
            }
            catch (XmlException e) {
                Console.WriteLine("Line Number {0}", e.LineNumber);
                Console.WriteLine("Line Position {0}", e.LinePosition);
                Console.WriteLine("Source URI {0}", e.SourceUri);
                Console.WriteLine("Message {0}", e.Message);
                Console.WriteLine("Stack Trace {0}", e.StackTrace);

            }

            catch (FormatException e) {
                Console.WriteLine("Caught Format Exception");

                Console.WriteLine("Helplink {0}", e.HelpLink);
                Console.WriteLine("Message {0}", e.Message);
                Console.WriteLine("Source {0}", e.Source);
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");
                //Console.WriteLine(" {0}");

                if (e.Data != null) {
                    Console.WriteLine("Getting extra details");
                    foreach (DictionaryEntry item in e.Data) {
                        Console.WriteLine("The key is '{0}' and the value is: {1}", item.Key, item.Value);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Caught Exception");
                if (e.Data != null) {
                    foreach (DictionaryEntry item in e.Data) {
                        Console.WriteLine("The key is '{0}' and the value is: {1}", item.Key, item.Value);
                    }
                }
            }


        }

        #endregion



    }
}
