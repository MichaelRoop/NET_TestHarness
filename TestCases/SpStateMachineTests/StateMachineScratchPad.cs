using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LogUtils;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using SpStateMachine.EventListners;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using SpStateMachine.EventStores;
using SpStateMachine.PeriodicTimers;
using System.Threading;

namespace TestCases.SpStateMachineTests {

    /// <summary>
    /// To drive development of framework
    /// </summary>
    [TestFixture]
    public class StateMachineScratchPad {

        #region Setup 

        ConsoleWriter consoleWriter = new ConsoleWriter();

        [TestFixtureSetUp]
        public void Setup() {
            Log.SetVerbosity(MsgLevel.Info);

            Log.SetMsgNumberThreshold(1);
            this.consoleWriter.StartLogging();
        }

        [TestFixtureTearDown]
        public void Teardown() {
            this.consoleWriter.StopLogging();
        }

        #endregion

        #region SpState generic Implementation classes

        /// <summary>
        /// The shared class that is represented by the state machine. The 
        /// properties and methods are accessible to the states
        /// by all states
        /// </summary>
        public class DataClass : IDisposable {

            #region Data

            private int intVal = 0;
            private string strVal = "Original value";

            #endregion

            #region Properties

            public int IntVal { get { return this.intVal; } set { this.intVal = value; } }
            public string StrVal { get { return this.strVal; } set { this.strVal = value; } }

            #endregion

            #region Public Methods

            public string GetMessage() {
                return "This is a message";
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                // Nothing to do. Provided for compatibility
            }

            #endregion
        }

        /// <summary>
        /// Derived class to attach an object that the state machine represents
        /// </summary>
        public class MyState : SpState<DataClass> {
            public MyState(DataClass dataClass)
                : base(dataClass) {
            }

            protected override ISpMessage ExecOnEntry(ISpMessage msg) {
                Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
                This.StrVal = "The message set on Entry";
                This.IntVal = 9876;
                return msg;
            }

            protected override ISpMessage ExecOnTick(ISpMessage msg) {
                Thread.Sleep(300);
                Log.Info("MyState", "ExecOnTick", String.Format("Raised {0} StrVal:{1} IntVal:{2}", msg.EventId, This.StrVal, This.IntVal));
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

        #endregion


        [Test, Explicit]
        public void TestInitialGenericSpState() {
            DataClass dataClass = new DataClass();
            ISpState s = new MyState(dataClass);
            ISpStateMachine sm = new MyStateMachine(dataClass, s);

            // sm.Tick(new BaseMsg(99, 456));

            ISpEventListner listner = new SimpleEventListner();
            listner.ResponseReceived += new Action<ISpMessage>((msg) => { });
            ISpMessage defaultTickMsg = new BaseMsg(0, 0);

            // TODO - Need a default response msg

            SpStateMachineEngine engine = 
                new SpStateMachineEngine(
                    listner,
                    new SimpleDequeEventStore(defaultTickMsg),
                    sm,
                    new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 500)));

            engine.Start();

            Thread.Sleep(3000);

            //sm.Tick(new BaseMsg(99, 456));

            listner.PostMessage(new BaseMsg(777, 12345));

            Thread.Sleep(3000);
            engine.Stop();

            Console.WriteLine("Disposing Engine - thread should not output while I wait 3 seconds");
            engine.Dispose();

            Thread.Sleep(3000);
            Console.WriteLine("Done");

            // Multi call test
            engine.Dispose();
            engine.Dispose();
            engine.Dispose();
            Console.WriteLine("Engine Disposed");

            //SpState<DataClass> state = new SpState<DataClass>(dataClass);
            //SpStateMachine<DataClass> stateMachine = new SpStateMachine<DataClass>(dataClass, state);

        }



    }
}
