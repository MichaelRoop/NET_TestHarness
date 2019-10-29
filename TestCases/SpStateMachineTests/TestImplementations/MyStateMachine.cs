using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyStateMachine : SpMachine<MyDataClass, MyEventType> {

        public MyStateMachine(MyDataClass dataClass, ISpState<MyEventType> state)
            : base(dataClass, state) {
        }

    }

}
