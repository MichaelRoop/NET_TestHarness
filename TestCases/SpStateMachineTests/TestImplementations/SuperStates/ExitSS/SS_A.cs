using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class SS_A : MySuperState {

        ISpState<MyMsgId> doneSt = null;

        public SS_A(ISpState<MyMsgId> parent, MyDataClass dataClass) 
            : base(parent, MyStateID.SS_A1, dataClass) {

            this.doneSt = this.AddSubState(new S_A1(this, dataClass));
            
            // The Done state will trigger the exit on 4th ticks. The first tick causes the entry
            this.doneSt.ToExitOnResult(MyMsgId.RespDone);

            this.SetEntryState(this.doneSt);
        }



    }
}
