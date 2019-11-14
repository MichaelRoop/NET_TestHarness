using System;
using System.Collections.Generic;
using System.Text;
using LogUtils.Net;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.Messaging.Messages;
using SpStateMachineDemo.Net.States;

namespace SpStateMachineDemo.Net.SuperStates {
    public class SuperStateMain : DemoSuperState {

        ISpState<DemoMsgId> notStarted = null;
        ISpState<DemoMsgId> idle = null;

        public SuperStateMain(DemoMachineObj machine) 
            : base(DemoStateId.Initial, machine) {

            this.notStarted = this.AddSubState(new SuperStateNotStarted(this, DemoStateId.NotStarted, machine));
            this.idle = this.AddSubState(new SuperStateIdle(this, DemoStateId.Idle, machine));

            this.notStarted.ToNextOnResult(DemoMsgId.InitComplete, this.idle);

            //this.notStarted.ToNextOnResult(DemoMsgId.Tick, this.idle, new MsgTick());



            Log.Warning(0, "----- Not started transitions -----");
            //this.notStarted.DebugDumpTransitions();

            //Log.Warning(0, "----- Main transitions -----");
            //            this.DebugDumpTransitions();




            this.SetEntryState(this.notStarted);
        }



    }
}
