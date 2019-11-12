using SpStateMachine.Interfaces;
using SpStateMachine.States;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;

namespace SpStateMachineDemo.Net.States {

    public class DemoState : SpState<DemoMachineObj, DemoMsgId, DemoStateId, DemoMsgType> {

        public DemoState(DemoStateId id, DemoMachineObj machine) 
            : base(DummyDI.MsgFactoryInstance, id, machine) {
        }


        public DemoState(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine)
            : base(parent, DummyDI.MsgFactoryInstance, id, machine) {
        }


    }
}
