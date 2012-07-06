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

        public NotStartedSs(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.NotStarted, dataClass) {

            MyState idle = new IdleSt(this, dataClass);
            MyState active = new ActiveSt(this, dataClass);

            this.AddSubState(idle);
            this.AddSubState(active);

            // Register Idle state transitions
            idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, new MyTickMsg()));


            // results
            idle.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition(SpStateTransitionType.NextState, active, null));
            //idle.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, new MyTickMsg()));


            // Register active state transitions
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, new MyTickMsg()));


            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));
            active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.Defered, null, null));


//            active.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, new MyTickMsg()));



            // Register my defered
            // If I get an abort handed to me by a state I will push the internal transition to idle


            // TODO - use this to provoque a check error
            //this.RegisterOnResultTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.NextState, idle, null));


            this.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));


            this.SetEntryState(idle);

        }






    }
}
