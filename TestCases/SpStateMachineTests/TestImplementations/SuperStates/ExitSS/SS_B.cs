using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class SS_B : MySuperState {

        ISpState<MyMsgId> bSt = null;

        public SS_B(ISpState<MyMsgId> parent, MyDataClass dataClass) 
            : base(parent, MyStateID.SS_B1, dataClass) {
            this.bSt = this.AddSubState(new S_B1(this, dataClass));
            this.SetEntryState(this.bSt);
        }

    }
}
