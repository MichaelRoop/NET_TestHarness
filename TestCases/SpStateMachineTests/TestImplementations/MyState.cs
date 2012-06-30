using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using LogUtils;
using System.Threading;
using SpStateMachine.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>
    /// Derived class to attach an object that the state machine represents
    /// </summary>
    /// <remarks>Note usage of enum to enforce strong typing at implementation level</remarks>
    public class MyState : SpState<MyDataClass> {

        string name = "";

        public MyState(MyStateID id, MyDataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        public MyState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, id.Int(), dataClass) {
        }

        public override string Name {
            get {
                if (this.name.Length == 0) {
                    StringBuilder sb = new StringBuilder(75);
                    this.IdChain.ForEach((item) => {
                        sb.Append(String.Format(".{0}", item.ToStateId().ToString()));
                    });
                    this.name = sb.Length > 0 ? sb.ToString(1, sb.Length - 1) : "NameSearchFailed";
                }
                return this.name;
            }
        }


        protected override ISpMessage ExecOnEntry(ISpMessage msg) {
            Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
            This.StrVal = "The message set on Entry";
            This.IntVal = 9876;
            return this.GetDefaultReturnMsg(msg);
        }

        protected override ISpMessage ExecOnTick(ISpMessage msg) {
            Thread.Sleep(200);
            Log.Info("MyState", "ExecOnTick", String.Format("Raised {0} StrVal:{1} IntVal:{2}", msg.EventId, This.StrVal, This.IntVal));
            return this.GetDefaultReturnMsg(msg);
        }


        protected override void ExecOnExit() {
            Log.Info("MyState", "ExecOnExit", "");
        }


        /// <summary>
        /// Provides the default return msg
        /// </summary>
        /// <param name="msg">The incomming message</param>
        protected override ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            
            // Temp
            return msg;

        }



        protected override ISpMessage GetReponseMsg(ISpMessage msg) {
            // will get it from a factory eventually
            int responseMsgTypeId = 22;
            return new BaseResponse(responseMsgTypeId, (BaseMsg)msg);
        }


    }
}
