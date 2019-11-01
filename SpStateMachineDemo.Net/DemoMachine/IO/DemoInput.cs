using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine.IO {
    public class DemoInput {

        public InputId Id { get; private set; }
        public IOState State { get; set; }

        private DemoInput() { }

        public DemoInput(InputId id) {
            this.Id = id;
            this.State = IOState.Off;
        }

    }
}
