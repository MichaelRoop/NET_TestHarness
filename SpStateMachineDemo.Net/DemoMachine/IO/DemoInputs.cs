using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpStateMachineDemo.Net.DemoMachine.IO {

    public class DemoInputs : IDemoInputs<InputId> {

        private IDemoOutputs<OutputId> outputs = null;
        private List<DemoInput> inputs = new List<DemoInput>();


        public DemoInputs(IDemoOutputs<OutputId> outputs) {
            // This is to allow feedback to demo without actual hardware
            this.outputs = outputs;
        }

        public void Add(InputId id) {
            if (!this.inputs.Exists(x => x.Id == id)) {
                this.inputs.Add(new DemoInput(id));
            }
        }

        public IOState GetState(InputId id) {
            if (this.inputs.Exists(x => x.Id == id)) {
                return this.inputs.Find(x => x.Id == id).State;
            }
            // TODO assertable
            return IOState.Off;
        }


        public void SetState(InputId id, IOState state) {
            if (this.inputs.Exists(x => x.Id == id)) {
                DemoInput input = this.inputs.Find(x => x.Id == id);
                input.State = state;
                this.KludgeCorrespondingOutput(input);
            }
        }


        /// <summary>
        /// Demo kludge that just sets the corresponding output so state machine
        /// will pick up the change event
        /// </summary>
        /// <param name="id">The input id</param>
        /// <param name="state">New input state</param>
        private void KludgeCorrespondingOutput(DemoInput input) {
            switch (input.Id) {
                case InputId.GasOxygen:
                    this.outputs.SetState(OutputId.GasOxygen, input.State);
                    break;
                case InputId.GasNitrogen:
                    this.outputs.SetState(OutputId.GasNitrogen, input.State);
                    break;
                case InputId.Heater:
                    this.outputs.SetState(OutputId.Heater, input.State);
                    break;
            }
        }

    }
}
