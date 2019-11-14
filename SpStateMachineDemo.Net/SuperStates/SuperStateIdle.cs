using System;
using System.Collections.Generic;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.States;

namespace SpStateMachineDemo.Net.SuperStates {
    public class SuperStateIdle : DemoSuperState {

        ISpState<DemoMsgId> waitOnStart = null;

        public SuperStateIdle(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine) 
            : base(parent, id, machine) {

            this.waitOnStart = this.AddSubState(new StateWaitOnStart(this, DemoStateId.WaitOnStart, machine));

            this.SetEntryState(this.waitOnStart);
        }
    }
}
