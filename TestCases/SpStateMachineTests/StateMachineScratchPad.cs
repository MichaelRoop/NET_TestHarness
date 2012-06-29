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
using SpStateMachine.Behaviours;

namespace TestCases.SpStateMachineTests {

    #region Test Sample enum state Ids and extension methods

    public enum MyStateID : int {
        Default,
        Active,
    }

    public static class MyStateIdExtensions {
        public static int Int(this MyStateID self) {
            return (int)self;
        }
    }

    #endregion

    #region Test Sample generic T to pass to SpState as the 'This' member

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

    #endregion

    #region Test Sample SpState derived implementation

    /// <summary>
    /// Derived class to attach an object that the state machine represents
    /// </summary>
    /// <remarks>Note usage of enum to enforce strong typing at implementation level</remarks>
    public class MyState : SpState<DataClass> {
        public MyState(MyStateID id, DataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        protected override ISpMessage ExecOnEntry(ISpMessage msg) {
            Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
            This.StrVal = "The message set on Entry";
            This.IntVal = 9876;
            return msg;
        }

        protected override ISpMessage ExecOnTick(ISpMessage msg) {
            Thread.Sleep(200);
            Log.Info("MyState", "ExecOnTick", String.Format("Raised {0} StrVal:{1} IntVal:{2}", msg.EventId, This.StrVal, This.IntVal));
            return msg;
        }


        protected override void ExecOnExit() {
            Log.Info("MyState", "ExecOnExit", "");
        }

    }

    #endregion

    #region Test Sample SpStateMachine derived Implementation

    public class MyStateMachine : SpStateMachine<DataClass> {
        public MyStateMachine(DataClass dataClass, ISpState state)
            : base(dataClass, state) {
        }
    }

    #endregion

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


        [Test, Explicit]
        public void TestInitialGenericSpState() {
            DataClass dataClass = new DataClass();
            // This would normally be the main superstate which includes all other states cascading within it's and it's children's constructors
            ISpState s = new MyState(MyStateID.Active, dataClass); 
            ISpStateMachine sm = new MyStateMachine(dataClass, s);
            ISpMessage defaultTickMsg = new BaseMsg(0, 0);
            ISpEventStore store = new SimpleDequeEventStore(defaultTickMsg);
            ISpBehaviorOnEvent behavior = new SpPeriodicWakeupOnly();
            ISpPeriodicTimer timer = new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 500));
            ISpEventListner listner = new SimpleEventListner();

            listner.ResponseReceived += new Action<ISpMessage>((msg) => { });


            // TODO - Need a default response msg

            // Simulates DI
            SpStateMachineEngine engine = 
                new SpStateMachineEngine(listner, store, behavior, sm, timer);

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
