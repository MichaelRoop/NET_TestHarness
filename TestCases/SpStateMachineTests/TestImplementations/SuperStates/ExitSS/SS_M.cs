using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class SS_M : MySuperState {

        ISpState<MyMsgId> ssA = null;
        ISpState<MyMsgId> ssB = null;

        public SS_M(MyDataClass dataClass) 
            : base(MyStateID.SS_M, dataClass) {

            this.ssA = this.AddSubState(new SS_A(this, dataClass));
            this.ssB = this.AddSubState(new SS_B(this, dataClass));

            this.ssA.ToNextOnResult(MyMsgId.RespDone, this.ssB);

            this.SetEntryState(this.ssA);
        }
    }
}
