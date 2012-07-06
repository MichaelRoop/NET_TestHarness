using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using LogUtils;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.States {

    public class IdleSt : MyState {

        private string className = "IdleState";
        private int triggerCount = 0;


        public IdleSt(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.Idle, dataClass) {
        }

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnEntry", this.FullName);
            //return base.ExecOnEntry(msg);
            return this.MsgFactory.GetDefaultResponse(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnTick", this.FullName + " ********************************************** ");
            if (This.DoIFlipStates) {
                // TODO - rework msg to allow creation of a msg with another msg to transfer correlation GUID
                Log.Info(this.className, "ExecOnTick", "Exceeded trigger count, ** changing msg to Start");
                MyBaseMsg newMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start);
                newMsg.Uid = msg.Uid;
                //return base.ExecOnTick(newMsg);
            }
            return msg;
            //return base.ExecOnEntry(msg);
        }

        protected override void ExecOnExit() {
            Log.Info(this.className, "ExecOnExit", "");
            //base.ExecOnExit();
        }


        //override onde


    }
}
