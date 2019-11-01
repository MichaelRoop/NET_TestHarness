using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.States;
using SpStateMachine.Converters;
using SpStateMachine.Core;
using LogUtils;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.CascadeOnExit {






    public class Level3Ss : MySuperState {

        public Level3Ss(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Level3, dataClass) {

            MyState idle = new IdleSt(this, dataClass);
            MyState active = new ActiveSt(this, dataClass);

            this.AddSubState(idle);
            this.AddSubState(active);

            // Register Idle state transitions
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnEventTransition(MyMsgId.Start, new SpStateTransition<MyMsgId>(SpStateTransitionType.NextState, active, null));
            //idle.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // results - this idle class just returns whatever msg we send in. So send Start and it will return it as its return value and provok this transition
            //idle.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Start), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, active, null));
            idle.RegisterOnResultTransition(MyMsgId.Start, new SpStateTransition<MyMsgId>(SpStateTransitionType.NextState, active, null));


            // Register active state transitions
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnEventTransition(MyMsgId.Stop, new SpStateTransition<MyMsgId>(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnEventTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition(SpStateTransitionType.ExitState, null, null));

            // results - this class just returns whatever msg we send in. So send Stop and it will return it as its return value and provok this transition
            //active.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Stop), new SpStateTransition<MyEventType>(SpStateTransitionType.NextState, idle, null));
            //active.RegisterOnResultTransition(new SpEnumToInt(MyEventType.Abort), new SpStateTransition<MyEventType>(SpStateTransitionType.ExitState, null, null));
            active.RegisterOnResultTransition(MyMsgId.Stop, new SpStateTransition<MyMsgId>(SpStateTransitionType.NextState, idle, null));
            active.RegisterOnResultTransition(MyMsgId.Abort, new SpStateTransition<MyMsgId>(SpStateTransitionType.ExitState, null, null));

            this.SetEntryState(idle);
        }

    }



    //public class Level2Active : MyState {

    //    private string className = "Level2Active";
    //    private int triggerCount = 0;


    //    public Level2Active(ISpState parent, MyDataClass dataClass)
    //        : base(parent, MyStateID.Idle, dataClass) {
    //    }

    //    protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
    //        Log.Info(this.className, "ExecOnEntry", "");
    //        return base.ExecOnEntry(msg);
    //    }

    //    protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
    //        Log.Info(this.className, "ExecOnTick", String.Format("With event:{0}", this.ConvertEventIdToString(msg.EventId)));
    //        if (SpConverter.IntToEnum<MyEventType>(msg.EventId) == MyEventType.Abort) {
    //            Log.Info(this.className, "ExecOnTick", String.Format("Exiting with event {0}", MyEventType.ExitAborted));
    //            return ExecOnEntry(new MyBaseMsg(MyMsgType.SimpleResponse, MyEventType.ExitAborted));
    //        }
    //        Log.Info(this.className, "ExecOnTick", "Exiting with default event");
    //        return base.ExecOnEntry(msg);
    //    }

    //    protected override void ExecOnExit() {
    //        Log.Info(this.className, "ExecOnExit", "");
    //        base.ExecOnExit();
    //    }
    //}


    public class Level2Idle : MyState {

        private string className = "Level2Idle";
//        private int triggerCount = 0;


        public Level2Idle(ISpState<MyMsgId> parent, MyDataClass dataClass)
            : base(parent, MyStateID.Idle, dataClass) {
        }

        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnEntry", "");
            return base.ExecOnEntry(msg);
        }

        protected override ISpEventMessage ExecOnTick(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnTick", String.Format("With event:{0}", this.GetCachedEventId(msg.EventId)));
            if (SpConverter.IntToEnum<MyMsgId>(msg.EventId) == MyMsgId.Abort) {
                Log.Info(this.className, "ExecOnTick", String.Format("Exiting with event {0}", MyMsgId.ExitAborted));
                return ExecOnEntry(new MyBaseMsg(MyMsgType.SimpleResponse, MyMsgId.ExitAborted));
            }
            Log.Info(this.className, "ExecOnTick", "Exiting with default event");
            return base.ExecOnEntry(msg);
        }

        protected override void ExecOnExit() {
            Log.Info(this.className, "ExecOnExit", "");
            base.ExecOnExit();
        }


        //override onde


    }





}
