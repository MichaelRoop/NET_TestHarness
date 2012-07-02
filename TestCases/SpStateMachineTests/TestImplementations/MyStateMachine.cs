using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyStateMachine : SpMachine<MyDataClass> {
        public MyStateMachine(MyDataClass dataClass, ISpState state)
            : base(dataClass, state) {
        }
    }

}
