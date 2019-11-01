using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine.IO {

    public class OutputChangeArgs : EventArgs {
        public OutputId Id { get; private set; }
        public IOState State { get; private set; }
        private OutputChangeArgs() { }
        public OutputChangeArgs(DemoOutput output) {
            this.Id = output.Id;
            this.State = output.State;
        }
    }


    public class DemoOutputs : IDemoOutputs<OutputId> {

        List<DemoOutput> outputs = new List<DemoOutput>();


        public event EventHandler StateChange;

        public DemoOutputs() {
        }


        public void Add(OutputId id) {
            if (!this.outputs.Exists(x => x.Id == id)) {
                this.outputs.Add(new DemoOutput(id));
            }
        }

        public IOState GetState(OutputId id) {
            if (this.outputs.Exists(x => x.Id == id)) {
                return this.outputs.Find(x => x.Id == id).State;
            }
            // TODO assertable
            return IOState.Off;
        }


        public void SetState(OutputId id, IOState state) {
            if (this.outputs.Exists(x => x.Id == id)) {
                DemoOutput output = this.outputs.Find(x => x.Id == id);
                output.State = state;
                this.StateChange?.Invoke(this, new OutputChangeArgs(output));
            }
        }
    }
}
