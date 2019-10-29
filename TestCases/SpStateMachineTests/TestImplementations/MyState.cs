using LogUtils;
using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachine.States;
using System;
using System.Threading;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>Derived class to attach an object that the state machine represents</summary>
    public class MyState : SpState<MyDataClass,MyEventType> {

        #region Constructors

        // Use MsgFactory and MyIdConverter Singletons rather than interface for test shortcut

        public MyState(MyStateID id, MyDataClass dataClass)
            : base(MyMsgFactory.Instance, MyIdConverter.Instance, new SpEnumToInt(id), dataClass) {
        }

        public MyState(ISpState<MyEventType> parent, MyStateID id, MyDataClass dataClass)
            : base(parent, MyMsgFactory.Instance, MyIdConverter.Instance, new SpEnumToInt(id), dataClass) {
        }

        #endregion

        #region Overrides

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
