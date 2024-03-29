﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Core;
using SpStateMachine.Converters;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class NotStartedSs : MySuperState {

        #region Data

        MyState StateIdle = null;
        MyState StateActive = null;

        #endregion

        public NotStartedSs(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.NotStarted, dataClass) {

            // Setup sub-states
            this.StateIdle = (MyState)this.AddSubState(new IdleSt(this, dataClass));
            this.StateActive = (MyState)this.AddSubState(new ActiveSt(this, dataClass));

            // Idle State event and results transitions
            this.StateIdle.ToNextOnEvent(MyEventType.Start, this.StateActive);
            this.StateIdle.ToExitOnEvent(MyEventType.Abort);
            this.StateIdle.ToNextOnResult(MyEventType.Start, this.StateActive);

            // Active State event and results transitions
            this.StateActive.ToNextOnEvent(MyEventType.Stop, this.StateIdle, new MyTickMsg());
            this.StateActive.ToDeferedOnEvent(MyEventType.Abort);
            this.StateActive.ToNextOnResult(MyEventType.Stop, this.StateIdle, new MyTickMsg());

            // Super state transitions
            this.ToNextOnResult(MyEventType.Stop, this.StateIdle);

            this.SetEntryState(StateIdle);
        }

    }
}
