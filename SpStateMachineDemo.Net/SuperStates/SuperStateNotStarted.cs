using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.SuperStates {

    /// <summary>Initial state with everything turned off</summary>
    public class SuperStateNotStarted : DemoSuperState {

        ISpState<DemoMsgId> recovery = null;
        ISpState<DemoMsgId> initialising = null;


        public SuperStateNotStarted(DemoMachineObj machine)
            : base(DemoStateId.Initial, machine) {

            // Create sub-states
            this.recovery = new StateSimpleRecovery(this, DemoStateId.SimpleRecovery, machine);
            this.initialising = new StateInitIO(this, machine);

            this.recovery.ToNextOnResult(DemoMsgId.RecoveryComplete, this.initialising);
            // TODO - needs ToExitOnResult ?
            this.initialising.ToExitOnEvent(DemoMsgId.InitComplete);

            this.SetEntryState(this.recovery);
        }




    }
}
