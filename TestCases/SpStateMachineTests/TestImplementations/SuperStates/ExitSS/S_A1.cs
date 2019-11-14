using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtils.Net;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class S_A1 : MyState {

        int counter = 0;
        ClassLog log = new ClassLog("** S_A1 **");

        public S_A1(ISpState<MyMsgId> parent, MyDataClass dataClass) 
            : base(parent, MyStateID.SS_A1, dataClass) {
        }

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            this.log.InfoEntry("ExecOnEntry");
            this.counter = 0;
            return base.ExecOnEntry(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            this.counter++;
            this.log.Info("ExecOnTick", ()=> string.Format("Count:{0}", this.counter));
            if (this.counter > 2) {
                this.log.Info("ExecOnTick", "RETURN DONE");
                return new MyDoneMsg();
            }
            return base.ExecOnTick(msg);
        }

        protected override void ExecOnExit() {
            this.log.InfoEntry("ExecOnExit");
            base.ExecOnExit();
        }



    }
}
