using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using LogUtils;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.States {

    public class ActiveSt : MyState {

        private string className = "ActiveSt";
//        private int triggerCount = 0;


        public ActiveSt(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Active, dataClass) {
        }

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnEntry", this.FullName);
            //return base.ExecOnEntry(msg);
            return this.MsgFactory.GetDefaultResponse(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnTick", "");
            if (This.DoIFlipStates) {
                // TODO rework msg to allow creation of a msg with another msg to transfer correlation GUID
                Log.Info(this.className, "ExecOnTick", "Exceeded trigger count, ** changing msg to Stop");
                MyBaseMsg newMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop);
                newMsg.Uid = msg.Uid;
                //return base.ExecOnTick(newMsg);
            }
            //return base.ExecOnEntry(msg);

            return msg;
        }

        protected override void ExecOnExit() {
            Log.Info(this.className, "ExecOnExit", "");
            //base.ExecOnExit();
        }

    }
}
