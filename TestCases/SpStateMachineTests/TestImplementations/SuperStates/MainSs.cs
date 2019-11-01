using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;
using SpStateMachine.Converters;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using LogUtils.Net;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates {

    public class MainSs : MySuperState {

        #region Data

        ISpState<MyMsgId> notStarted = null;
        ISpState<MyMsgId> recovery = null;

        #endregion

        public MainSs(MyDataClass dataClass)
            : base(MyStateID.Main, dataClass) {

            // Create and add the sub-states of this super state
            this.notStarted = new NotStartedSs(this, dataClass);
            this.recovery = new RecoverySs(this, dataClass);
            this.AddSubState(this.notStarted);
            this.AddSubState(this.recovery);

            // Register Started state transitions


            //notStarted.RegisterOnEventTransition(MyEventType.Start, new SpStateTransition(SpStateTransitionType.NextState, recovery, null));
            //notStarted.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            // Register OnResult so that the Superstate can handle its state's ExitState transitions
            //            notStarted.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.NextState, recovery, null));

            // Register a transition based on on internal processing
            //notStarted.RegisterOnResultTransition(
            //    new SpEnumToInt(MyEventType.Abort), 
            //    new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, 
            //    this.recovery, new MyTickMsg()));

            //notStarted.RegisterOnResultTransition(
            //    MyEventType.Abort,
            //    new SpStateTransition<MyEventType>(SpStateTransitionType.NextState,
            //    this.recovery, new MyTickMsg()));

            notStarted.ToNextOnResult(MyMsgId.Abort, this.recovery, new MyTickMsg());


            //// Register active state transitions 
            //active.RegisterOnEventTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            //// Only on events registered.  On abor goes exit state


            //this.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.NextState, recovery, new MyTickMsg()));

            // Set the entry state as the not Started SS
            this.SetEntryState(this.notStarted);

        }
    }
}