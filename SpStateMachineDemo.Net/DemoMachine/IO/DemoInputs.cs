using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpStateMachineDemo.Net.DemoMachine.IO {

    public class InputChangeArgs : EventArgs {
        public InputId Id { get; private set; }
        public IOState State { get; private set; }
        private InputChangeArgs() { }
        public InputChangeArgs(DemoInput input) {
            this.Id = input.Id;
            this.State = input.State;
        }
    }


    public class DemoInputs : IDemoInputs<InputId> {

        private IDemoOutputs<OutputId> outputs = null;
        private List<DemoInput> inputs = new List<DemoInput>();

        public event EventHandler StateChange;


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
                this.StateChange?.Invoke(this, new InputChangeArgs(input));
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
            // TODO - put in a thread delay to simulate
            switch (input.Id) {
                case InputId.GasOxygen:
                    this.KludgeOutputTimeDelaySet(OutputId.GasOxygen, input.State, 1000);
                    break;
                case InputId.GasNitrogen:
                    this.KludgeOutputTimeDelaySet(OutputId.GasNitrogen, input.State, 1100);
                    break;
                case InputId.Heater:
                    this.KludgeOutputTimeDelaySet(OutputId.Heater, input.State, 400);
                    break;
            }
        }


        private void KludgeOutputTimeDelaySet(OutputId id, IOState state, int delayMs) {
            switch (state) {
                case IOState.Off:
                    this.outputs.SetState(id, state);
                    break;
                case IOState.On:
                    //Task.Run(() => {
                    //    Thread.Sleep(delayMs);
                        this.outputs.SetState(id, state);
                    //});
                    break;
            }
        }

    }
}
