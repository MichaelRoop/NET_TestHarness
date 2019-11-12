using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;

namespace SpStateMachineDemo.Net.Core {

    public class DemoStateMachine : SpMachine<DemoMachineObj, DemoMsgId> {
    
        public DemoStateMachine(DemoMachineObj wrappedObject, ISpState<DemoMsgId> entryState) 
            : base(wrappedObject, entryState) {
        }
    }
}
