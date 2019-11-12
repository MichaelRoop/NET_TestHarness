using SpStateMachine.Interfaces;
using SpStateMachine.States;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.States;

namespace SpStateMachineDemo.Net.SuperStates {

    /// <summary>Base class to define the template parameters to base SpSuperState</summary>
    public class DemoSuperState : SpSuperState<DemoMachineObj, DemoMsgId, DemoStateId, DemoMsgType> {

        public DemoSuperState(DemoStateId id, DemoMachineObj machine) 
            : base(DummyDI.MsgFactoryInstance, id, machine) {
        }


        public DemoSuperState(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine)
            : base(parent, DummyDI.MsgFactoryInstance, id, machine) {
        }

    }
}
