using SpStateMachineDemo.Net.DemoMachine.IO;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine {
    public class DemoMachineWrapper {

        #region Data

        IDemoOutputs<OutputId> outputs = DummyDI.OutputsInstance;
        IDemoInputs<InputId> inputs = DummyDI.InputsInstance;

        #endregion

        #region Events

        public event EventHandler InputStateChange = null;
        public event EventHandler OutputStateChange = null;

        #endregion

        public void Init() {
            this.outputs.Add(OutputId.GasNitrogen);
            this.outputs.Add(OutputId.GasOxygen);
            this.outputs.Add(OutputId.Heater);

            this.inputs.Add(InputId.GasNitrogen);
            this.inputs.Add(InputId.GasOxygen);
            this.inputs.Add(InputId.Heater);

            this.outputs.StateChange += this.Outputs_StateChange;
            this.inputs.StateChange += this.Inputs_StateChange;

            // Create state machine and engine

        }

        public void SendMsg(DemoMsgId id) {

        }


        public void Teardown() {
            // TODO - code to shut down state machine
            this.outputs.StateChange -= this.Outputs_StateChange;
            this.inputs.StateChange -= this.Inputs_StateChange;
        }


        /// <summary>Demo method to manualy toggle the input. Normaly done by state machine</summary>
        /// <param name="id">The input id</param>
        public void ToggleIO(InputId id) {
            this.inputs.SetState(id, this.inputs.GetState(id) == IOState.On ? IOState.Off : IOState.On);
        }


        /// <summary>Demo method to manualy toggle the input. Normaly done by HW and read by state machine</summary>
        /// <param name="id">The output id</param>
        public void ToggleIO(OutputId id) {
            this.outputs.SetState(id, this.outputs.GetState(id) == IOState.On ? IOState.Off : IOState.On);
        }


        private void Inputs_StateChange(object sender, EventArgs args) {
            this.InputStateChange?.Invoke(sender, args);
        }


        /// <summary>Event handler from ouputs</summary>
        /// <param name="sender">The object sending the event (DemoOutput)</param>
        /// <param name="e">Event args (OutputChangeArgs)</param>
        private void Outputs_StateChange(object sender, EventArgs args) {
            this.OutputStateChange?.Invoke(sender, args);
        }

    }
}
