using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class NotStartedSs : MySuperState {

        public NotStartedSs(MyDataClass dataClass)
            : base(MyStateID.NotStarted, dataClass) {

            ISpState idle = new IdleSt(this, dataClass);
            ISpState active = new ActiveSt(this, dataClass);

            this.AddSubState(idle);
            this.AddSubState(active);

            // Register Idle state transitions
            idle.RegisterOnEventTransition(MyEventType.Start.Int(), new SpStateTransition(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnEventTransition(MyEventType.Abort.Int(), new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            // Register active state transitions
            active.RegisterOnEventTransition(MyEventType.Stop.Int(), new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnEventTransition(MyEventType.Abort.Int(), new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            //this.RegisterOnEventTransition(MyEventType.

            this.SetEntryState(idle);

        }






    }
}
