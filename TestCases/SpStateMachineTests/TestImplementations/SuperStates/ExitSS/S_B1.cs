using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class S_B1 : MyState {
        public S_B1(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.S_B1, dataClass) {
        }
    }
}
