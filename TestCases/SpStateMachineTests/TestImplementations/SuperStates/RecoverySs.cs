﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class RecoverySs : MySuperState {

        public RecoverySs(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.Recovery, dataClass) {

            MyState idle = new IdleSt(this, dataClass);
            MyState active = new ActiveSt(this, dataClass);

            this.AddSubState(idle);
            this.AddSubState(active);

            // Register Idle state transitions
            idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // Register active state transitions
            active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            // Only on events registered.  On abor goes exit state


            this.SetEntryState(idle);

        }
    }
}
