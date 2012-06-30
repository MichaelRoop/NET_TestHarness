using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyStateMachine : SpStateMachine<MyDataClass> {
        public MyStateMachine(MyDataClass dataClass, ISpState state)
            : base(dataClass, state) {
        }
    }

}
