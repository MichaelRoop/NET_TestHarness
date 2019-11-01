using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyStateMachine : SpMachine<MyDataClass, MyMsgId> {

        public MyStateMachine(MyDataClass dataClass, ISpState<MyMsgId> state)
            : base(dataClass, state) {
        }

    }

}
