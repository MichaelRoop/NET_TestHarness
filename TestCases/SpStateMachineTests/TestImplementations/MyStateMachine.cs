using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyStateMachine : SpMachine<MyDataClass> {

        public MyStateMachine(MyDataClass dataClass, ISpState state)
            : base(dataClass, state) {
        }

    }

}
