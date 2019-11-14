using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.States;

namespace SpStateMachineDemo.Net.SuperStates {

    /// <summary>Initial state with everything turned off</summary>
    public class SuperStateNotStarted : DemoSuperState {

        ISpState<DemoMsgId> recovery = null;
        ISpState<DemoMsgId> initialising = null;


        public SuperStateNotStarted(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine)
            : base(parent, id, machine) {

            // Create sub-states
            this.recovery = this.AddSubState(new StateSimpleRecovery(this, DemoStateId.SimpleRecovery, machine));
            this.initialising = this.AddSubState(new StateInitIO(this, machine));

            // Register events and internal result returns
            this.recovery.ToNextOnResult(DemoMsgId.RecoveryComplete, this.initialising);
            this.initialising.ToExitOnResult(DemoMsgId.InitComplete);

            this.SetEntryState(this.recovery);
        }

    }

}
