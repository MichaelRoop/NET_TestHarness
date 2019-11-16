using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {

    //public class EntryState : MyState {
    //    public EntryState(ISpState<MyMsgId> parent, MyStateID id, MyDataClass dataClass) 
    //        : base(parent, id, dataClass) {
    //    }
    //    protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
    //        return new MyDoneMsg();
    //    }
    //}

    public class SS_M2 : MySuperState {

        //ISpState<MyMsgId> firstState = null;
        ISpState<MyMsgId> ssADoneOnEntry = null;
        ISpState<MyMsgId> ssB = null;

        public SS_M2(MyDataClass dataClass)
            : base(MyStateID.SS_M2, dataClass) {

            //this.firstState = this.AddSubState(new EntryState(this, MyStateID.Level3, dataClass));
            this.ssADoneOnEntry = this.AddSubState(new SS_A2(this, MyStateID.SS_A1, dataClass));
            this.ssB = this.AddSubState(new SS_B(this, dataClass));

            // Add first state to just handle initial tick
            //this.firstState.ToNextOnResult(MyMsgId.RespDone, this.ssADoneOnEntry);

            this.ssADoneOnEntry.ToNextOnResult(MyMsgId.RespDone, this.ssB);

            //this.SetEntryState(this.firstState);
            this.SetEntryState(this.ssADoneOnEntry);
        }
    }
}
