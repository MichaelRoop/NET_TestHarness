using System;
using System.Collections.Generic;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.DemoMachine.IO;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;

namespace SpStateMachineDemo.Net.States {

    public class StateSimpleRecovery : DemoState {

        #region Data

        IDemoInputs<InputId> inputs = null;
        IDemoOutputs<OutputId> outputs = null;

        #endregion
    
        public StateSimpleRecovery(ISpState<DemoMsgId> parent, DemoStateId id, DemoMachineObj machine) 
            : base(parent, id, machine) {
            this.inputs = DummyDI.InputsInstance;
            this.outputs = DummyDI.OutputsInstance;
        }


        protected override ISpEventMessage ExecOnEntry(ISpEventMessage eventMsg) {
            this.inputs.SetState(InputId.Heater, IOState.Off);
            this.inputs.SetState(InputId.GasNitrogen, IOState.Off);
            this.inputs.SetState(InputId.GasOxygen, IOState.Off);
            return base.ExecOnEntry(eventMsg);
        }


        protected override ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
            // If all IO is on, return message
            if (this.outputs.GetState(OutputId.Heater) == IOState.Off &&
                this.outputs.GetState(OutputId.GasNitrogen) == IOState.Off &&
                this.outputs.GetState(OutputId.GasOxygen) == IOState.Off) {
                return new DemoMsgBase(DemoMsgType.SimpleMsg, DemoMsgId.RecoveryComplete) {
                    Uid = eventMsg.Uid
                };
            }
            // Still waiting
            return base.ExecOnTick(eventMsg);
        }

    }
}
