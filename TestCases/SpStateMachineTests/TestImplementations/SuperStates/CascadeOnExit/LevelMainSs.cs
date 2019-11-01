using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Converters;
using SpStateMachine.Core;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.CascadeOnExit {

    public class LevelMainSs : MySuperState {

        public LevelMainSs(MyDataClass dataClass)
            : base(MyStateID.Main, dataClass) {

            MySuperState level2 = new Level2Ss(this, dataClass);
            MySuperState recovery = new RecoverySs(this, dataClass);

            this.AddSubState(level2);
            this.AddSubState(recovery);

            // Register Started state transitions
            //notStarted.RegisterOnEventTransition(MyEventType.Start, new SpStateTransition(SpStateTransitionType.NextState, recovery, null));
            //notStarted.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            // Register OnResult so that the Superstate can handle its state's ExitState transitions
//            level2.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.NextState, recovery, null));

            level2.RegisterOnResultTransition(MyMsgId.Abort, new SpStateTransition<MyMsgId>(SpStateTransitionType.NextState, recovery, new MyTickMsg()));


            //// Register active state transitions
            //active.RegisterOnEventTransition(MyEventType.Stop, new SpStateTransition(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(MyEventType.Abort, new SpStateTransition(SpStateTransitionType.ExitState, null, null));


            //// Only on events registered.  On abor goes exit state


            this.SetEntryState(level2);

        }





    }
}
