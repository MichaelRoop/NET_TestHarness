using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using LogUtils;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using LogUtils.Net;

namespace TestCases.SpStateMachineTests.TestImplementations.States {

    public class IdleSt : MyState {

        //        private int triggerCount = 0;
        ClassLog log = new ClassLog("IdleSt");


        public IdleSt(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Idle, dataClass) {
        }

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            this.log.Info("ExecOnEntry", this.FullName);
            //return base.ExecOnEntry(msg);
            return this.MsgFactory.GetDefaultResponse(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            this.log.Info("ExecOnTick", this.FullName + " ********************************************** ");
            if (This.DoIFlipStates) {
                // TODO - rework msg to allow creation of a msg with another msg to transfer correlation GUID
                this.log.Info("ExecOnTick", "Exceeded trigger count, ** changing msg to Start");
                MyBaseMsg newMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start);
                newMsg.Uid = msg.Uid;
                //return base.ExecOnTick(newMsg);
            }
            return msg;
            //return base.ExecOnEntry(msg);
        }

        protected override void ExecOnExit() {
            this.log.Info("ExecOnExit", "");
            //base.ExecOnExit();
        }


        //override onde


    }
}
