using System;
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

        ISpState<MyMsgId> StateIdle = null;
        ISpState<MyMsgId> StateActive = null;

        #endregion

        public NotStartedSs(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.NotStarted, dataClass) {

            // Setup sub-states
            this.StateIdle = (MyState)this.AddSubState(new IdleSt(this, dataClass));
            this.StateActive = (MyState)this.AddSubState(new ActiveSt(this, dataClass));

            // Idle State event and results transitions
            this.StateIdle.ToNextOnEvent(MyMsgId.Start, this.StateActive);
            this.StateIdle.ToExitOnEvent(MyMsgId.Abort);
            this.StateIdle.ToNextOnResult(MyMsgId.Start, this.StateActive);

            // Active State event and results transitions
            this.StateActive.ToNextOnEvent(MyMsgId.Stop, this.StateIdle, new MyTickMsg());
            this.StateActive.ToDeferedOnEvent(MyMsgId.Abort);
            this.StateActive.ToNextOnResult(MyMsgId.Stop, this.StateIdle, new MyTickMsg());

            // Super state transitions
            this.ToNextOnResult(MyMsgId.Stop, this.StateIdle);

            this.SetEntryState(this.StateIdle);
        }

    }
}
