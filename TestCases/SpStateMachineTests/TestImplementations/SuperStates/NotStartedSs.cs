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

            MyState idle = new IdleSt(this, dataClass);
            MyState active = new ActiveSt(this, dataClass);

            this.AddSubState(idle);
            this.AddSubState(active);

            // Register Idle state transitions
            idle.RegisterOnEventTransition(MyEventType.Start, new SpStateTransition(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // results
            idle.RegisterOnResultTransition(MyEventType.Start, new SpStateTransition(SpStateTransitionType.NextState, active, null));


            // Register active state transitions
            active.RegisterOnEventTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));
            active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.Defered, null, null));

            active.RegisterOnResultTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));


            // Register my defered
            // If I get an abort handed to me by a state I will push the internal transition to idle


            // TODO - use this to provoque a check error
            //this.RegisterOnResultTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.NextState, idle, null));


            this.RegisterOnResultTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));


            this.SetEntryState(idle);

        }






    }
}
