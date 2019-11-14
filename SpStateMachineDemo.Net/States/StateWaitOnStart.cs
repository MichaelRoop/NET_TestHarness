using System;
using System.Collections.Generic;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;

namespace SpStateMachineDemo.Net.States {
    public class StateWaitOnStart : DemoState {
        public StateWaitOnStart(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine) 
            : base(parent, id, machine) {

        }

        // override events later

    }
}
