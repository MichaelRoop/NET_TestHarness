using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine.IO {
    public class DemoOutput {
        public OutputId Id { get; private set; }
        public IOState State { get; set; }

        private DemoOutput() { }

        public DemoOutput(OutputId id) {
            this.Id = id;
            this.State = IOState.Off;
        }

    }
}
