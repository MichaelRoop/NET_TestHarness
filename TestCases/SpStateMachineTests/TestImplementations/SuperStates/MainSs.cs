using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class MainSs : MySuperState {

        public MainSs(MyDataClass dataClass)
            : base(MyStateID.Main, dataClass) {

            MySuperState notStarted = new NotStartedSs(this, dataClass);
            MySuperState recovery = new RecoverySs(this, dataClass);

            this.AddSubState(notStarted);
            this.AddSubState(recovery);

            // Register Started state transitions
            //notStarted.RegisterOnEventTransition(MyEventType.Start, new SpStateTransition(SpStateTransitionType.NextState, recovery, null));
            //notStarted.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // Register OnResult so that the Superstate can handle its state's ExitState transitions
            notStarted.RegisterOnResultTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.NextState, recovery, null));


            //// Register active state transitions
            //active.RegisterOnEventTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            //// Only on events registered.  On abor goes exit state


            this.SetEntryState(notStarted);

        }
    }
}