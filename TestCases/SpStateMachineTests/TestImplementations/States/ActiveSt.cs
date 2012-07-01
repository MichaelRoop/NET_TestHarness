﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using LogUtils;

namespace TestCases.SpStateMachineTests.TestImplementations.States {

    public class ActiveSt : MyState {

        private string className = "ActiveSt";

        public ActiveSt(ISpState parent, MyDataClass dataClass)
            : base(parent, MyStateID.Active, dataClass) {
        }

        protected override ISpMessage ExecOnEntry(ISpMessage msg) {
            Log.Info(this.className, "ExecOnEntry", "");
            return base.ExecOnEntry(msg);
        }

        protected override ISpMessage ExecOnTick(ISpMessage msg) {
            Log.Info(this.className, "ExecOnTick", "");
            return base.ExecOnEntry(msg);
        }

        protected override void ExecOnExit() {
            Log.Info(this.className, "ExecOnExit", "");
            base.ExecOnExit();
        }

    }
}
