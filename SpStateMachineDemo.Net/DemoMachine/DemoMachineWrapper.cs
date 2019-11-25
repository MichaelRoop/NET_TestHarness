using LogUtils.Net;
using SpStateMachine.Behaviours;
using SpStateMachine.Core;
using SpStateMachine.EventListners;
using SpStateMachine.EventStores;
using SpStateMachine.Interfaces;
using SpStateMachine.PeriodicTimers;
using SpStateMachineDemo.Net.Core;
using SpStateMachineDemo.Net.DemoMachine.IO;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.Messaging.Messages;
using SpStateMachineDemo.Net.SuperStates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpStateMachineDemo.Net.DemoMachine {

    public  class StateChangeArgs : EventArgs {
        public string State { get; private set; }
        public StateChangeArgs(string newState) {
            this.State = newState;
        }
    }

    public class DemoMachineWrapper {

        #region Data

        IDemoOutputs<OutputId> outputs = DummyDI.OutputsInstance;
        IDemoInputs<InputId> inputs = DummyDI.InputsInstance;

        #endregion

        #region Events

        public event EventHandler InputStateChange = null;
        public event EventHandler OutputStateChange = null;
        public event EventHandler StateMachineStateChange = null;

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
            this.CreateStateMachine();

            // Get response change to the state


            this.stateMachineEngine.Start();
        }


        //public void StartEngine() {
        //    this.stateMachineEngine.Start();
        //}

        //public void StopEngine() {
        //    this.stateMachineEngine.Stop();
        //}


        public void SendMsg(DemoMsgId id) {
            DummyDI.EventListnerInstance.PostMessage(new DemoMsgBase(DemoMsgType.SimpleMsg, id));
        }


        public void SendMsg(ISpEventMessage msg) {
            DummyDI.EventListnerInstance.PostMessage(msg);
        }


        public void Teardown() {
            this.stateMachineEngine.Stop();
            // TODO - code to shut down state machine
            this.outputs.StateChange -= this.Outputs_StateChange;
            this.inputs.StateChange -= this.Inputs_StateChange;
            DummyDI.EventListnerInstance.ResponseReceived -= this.EventListnerInstance_ResponseReceived;
            //DummyDI.EventListnerInstance.MsgReceived -= this.EventListnerInstance_MsgReceived;

             
            //this.stateMachine.Dispose();
            this.stateMachineEngine.Dispose();

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


        SpStateMachineEngine stateMachineEngine = null;
        ISpStateMachine stateMachine = null;
        string currentState = "";

        private void CreateStateMachine() {
            this.stateMachine = new DemoStateMachine(
                        DummyDI.DemoMachineObjInstance,
                        new SuperStateMain(DummyDI.DemoMachineObjInstance));

            this.stateMachineEngine = new SpStateMachineEngine(
                DummyDI.EventListnerInstance,
                new PriorityEventStore(new MsgTick()),
                new SpPeriodicWakeupOnly(),
                this.stateMachine,
                new WinSimpleTimer(new TimeSpan(0, 0, 0, 0, 1000)));



            //Thread.Sleep(2000);

            //DummyDI.EventListnerInstance.MsgReceived += EventListnerInstance_MsgReceived;

            DummyDI.EventListnerInstance.ResponseReceived -= this.EventListnerInstance_ResponseReceived;
            DummyDI.EventListnerInstance.ResponseReceived += this.EventListnerInstance_ResponseReceived;
            //DummyDI.EventListnerInstance.ResponseReceived += this.EventListnerInstance_ResponseReceived;
            //DummyDI.EventListnerInstance.ResponseReceived += new Action<ISpEventMessage>((msg) => {
        }


        private void EventListnerInstance_ResponseReceived(object sender, EventArgs e) {

            Log.Warning(0, "****************************** Response received");

            //MsgOrResponseArgs args = (MsgOrResponseArgs)e;
            if (this.stateMachine.CurrentStateName != this.currentState) {
                this.currentState = this.stateMachine.CurrentStateName;
                this.StateMachineStateChange?.Invoke(this, new StateChangeArgs(this.currentState));
            }

        }




    }
}
