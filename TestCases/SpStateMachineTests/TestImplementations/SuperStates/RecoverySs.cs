using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class RecoverySs : MySuperState {

        ISpState<MyEventType> idle = null;
        ISpState<MyEventType> active = null;


        public RecoverySs(ISpState<MyEventType> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Recovery, dataClass) {

            this.idle = new IdleSt(this, dataClass);
            this.active = new ActiveSt(this, dataClass);

            this.AddSubState(this.idle);
            this.AddSubState(this.active);

            // Register Idle state transitions
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, active, null));
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition<MyEventType>(SpStateTransitionType.ExitState, null, null));

            //idle.RegisterOnEventTransition(MyEventType.Start, new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, active, null));
            //idle.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition<MyEventType>(SpStateTransitionType.ExitState, null, null));
            this.idle.ToNextOnEvent(MyEventType.Start, this.active);
            this.idle.ToExitOnEvent(MyEventType.Abort);

            // Register active state transitions
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition<MyEventType>(SpStateTransitionType.ExitState, null, null));
            //active.RegisterOnEventTransition(MyEventType.Stop, new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition<MyEventType>(SpStateTransitionType.ExitState, null, null));

            this.active.ToNextOnEvent(MyEventType.Stop, this.idle);
            this.active.ToExitOnEvent(MyEventType.Abort);

            // Only on events registered.  On abor goes exit state


            this.SetEntryState(idle);

        }
    }
}
