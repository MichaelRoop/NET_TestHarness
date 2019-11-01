using LogUtils;
using SpStateMachine.Interfaces;
using SpStateMachine.States;
using System;
using System.Threading;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>
    /// Derived test state class to resolve generic parameters which represent the State Machine common 
    /// data class, the event type enum, and the state id enum 
    /// </summary>
    public class MyState : SpState<MyDataClass,MyMsgId,MyStateID,MyMsgType> {

        #region Constructors

        // Note: Singletons are test shortcuts. Should pass in Interfaces via DI

        /// <summary>Constructor</summary>
        /// <param name="id">The identifier for the state instance</param>
        /// <param name="dataClass">
        /// Object represented by state machine containing common data and methods
        /// </param>
        public MyState(MyStateID id, MyDataClass dataClass)
            : base(MyDummyDI.MsgFactoryInstance, id, dataClass) {
        }


        public MyState(ISpState<MyMsgId> parent, MyStateID id, MyDataClass dataClass)
            : base(parent, MyDummyDI.MsgFactoryInstance, id, dataClass) {
        }

        #endregion

        #region Overrides

        // Overrides only for sending out logs and values for tests

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
            This.StrVal = "The message set on Entry";
            This.IntVal = 9876;
            return this.MsgFactory.GetDefaultResponse(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            Thread.Sleep(200);
            Log.Info("MyState", "ExecOnTick", String.Format("Raised {0} StrVal:{1} IntVal:{2}", msg.EventId, This.StrVal, This.IntVal));
            //return this.GetDefaultReturnMsg(msg);
            return msg;
        }


        protected override void ExecOnExit() {
            Log.Info("MyState", "ExecOnExit", this.FullName);
        }

        #endregion

    }
}
