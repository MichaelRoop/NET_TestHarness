using LogUtils.Net;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.DemoMachine.IO;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.Messaging.Messages;

namespace SpStateMachineDemo.Net.States {

    public class StateInitIO : DemoState {

        #region Data

        IDemoInputs<InputId> inputs = null;
        IDemoOutputs<OutputId> outputs = null;

        #endregion


        public StateInitIO(ISpState<DemoMsgId> parent, DemoMachineObj machine) 
            : base(parent, DemoStateId.InitIO, machine) {

            this.inputs = DummyDI.InputsInstance;
            this.outputs = DummyDI.OutputsInstance;
        }


        protected override ISpEventMessage ExecOnEntry(ISpEventMessage eventMsg) {
            if (this.outputs.GetState(OutputId.Heater)  == IOState.On) {
                // Bad, should have been turned off
                // return unrecoverable error
            }

            // Set IO to entry values
            //this.inputs.SetState(InputId.Heater, IOState.Off); // this done on recovery
            this.inputs.SetState(InputId.GasNitrogen, IOState.On);
            this.inputs.SetState(InputId.GasOxygen, IOState.On);
            return base.ExecOnEntry(eventMsg);
        }


        protected override ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
            // If all IO is on, return message
            if (this.outputs.GetState(OutputId.GasNitrogen) == IOState.On &&
                this.outputs.GetState(OutputId.GasOxygen) == IOState.On) {
                Log.Warning(2, "******* Everyting is ON  **********");
                return new MsgInitComplete();
            }
            // Still waiting
            Log.Warning(3, "******* STILL WAITING  **********");
            return base.ExecOnTick(eventMsg);
        }

    }
}
