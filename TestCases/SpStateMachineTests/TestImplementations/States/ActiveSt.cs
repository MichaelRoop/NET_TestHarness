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
        private int triggerCount = 0;


        public ActiveSt(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.Active, dataClass) {
        }

        protected override ISpMessage ExecOnEntry(ISpMessage msg) {
            Log.Info(this.className, "ExecOnEntry", "");
            return base.ExecOnEntry(msg);
        }

        protected override ISpMessage ExecOnTick(ISpMessage msg) {
            Log.Info(this.className, "ExecOnTick", "");

            //if (this.triggerCount++ < 2) {
            //    return base.ExecOnEntry(msg);
            //}
            //Log.Info(this.className, "ExecOnTick", "Exceeded trigger count, changing msg to Stop");
            //triggerCount = 0;

            ////// rework msg to allow creation of a msg with another msg to transfer correlation GUID
            ////MyBaseResponse ret = new MyBaseResponse(MyMsgType.SimpleResponse, (MyBaseMsg)msg, MyReturnCode.Success, "");
            ////ret.EventId = MyEventType.Stop.Int();
            ////return base.ExecOnEntry(ret);

            //MyBaseMsg newMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop);
            //newMsg.Uid = msg.Uid;
            //return base.ExecOnTick(newMsg);

            if (This.DoIFlipStates) {
                // rework msg to allow creation of a msg with another msg to transfer correlation GUID
                Log.Info(this.className, "ExecOnTick", "Exceeded trigger count, ** changing msg to Stop");
                MyBaseMsg newMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop);
                newMsg.Uid = msg.Uid;
                return base.ExecOnTick(newMsg);
            }
            return base.ExecOnEntry(msg);

        }

        protected override void ExecOnExit() {
            Log.Info(this.className, "ExecOnExit", "");
            base.ExecOnExit();
        }

    }
}
