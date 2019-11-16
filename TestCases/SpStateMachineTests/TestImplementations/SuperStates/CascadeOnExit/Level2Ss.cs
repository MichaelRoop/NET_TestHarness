using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Converters;
using SpStateMachine.Core;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.CascadeOnExit {

    public class Level2Ss : MySuperState {

        public Level2Ss(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Level2, dataClass) {

            MySuperState level3Ss = new Level3Ss(this, dataClass);
            MyState active = new ActiveSt(this, dataClass);

            this.AddSubState(level3Ss);
            //this.AddSubState(active);


            //// Register Idle state transitions
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition(SpStateTransitionType.NextState, active, null));
            ////idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // results - this idle class just returns whatever msg we send in. So send Start and it will return it as its return value and provok this transition

            // Register as the result event from previous state comming from abort to it
            //level3Ss.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            //level3Ss.RegisterOnResultTransition(MyMsgId.Abort, new SpStateTransition<MyMsgId>(SpStateTransitionType.ExitState, null, new MyTickMsg()));
            level3Ss.RegisterOnResultTransition(MyMsgId.Abort, new SpStateTransition<MyMsgId>(SpStateTransitionType.ExitState, null, null));


            //// Register active state transitions
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            //// results - this class just returns whatever msg we send in. So send Stop and it will return it as its return value and provok this transition
            //active.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition(SpStateTransitionType.NextState, idle, null));

            this.SetEntryState(level3Ss);

        }

    }
}
