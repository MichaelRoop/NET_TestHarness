using SpStateMachineDemo.Net.DemoMachine.IO;
using SpStateMachineDemo.Net.DI;
using SpStateMachineDemo.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpStateMachineDemo.UI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window {

        IDemoOutputs<OutputId> outputs = DummyDI.OutputsInstance;
        IDemoInputs<InputId> inputs = DummyDI.InputsInstance;

        public MainWindow() {
            InitializeComponent();
            this.SetupIO();
        }

        #region Windows events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.outputs.StateChange -= this.Outputs_StateChange;

            // TODO - code to shut down state machine
            //MessageBox.Show("Shutting down state machine");
        }

        #endregion

        #region Fake a hardware output change to exercise state machine

        private void btnOxygen_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(OutputId.GasOxygen);
        }

        private void btnNitrogen_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(OutputId.GasNitrogen);
            //this.ToggleInput(InputId.GasNitrogen);
        }

        private void btnHeat_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(OutputId.Heater);
        }

        // Use later ?  The state machine will be turning on the elements. No need to do it manually
        private void ToggleInput(InputId id) {
            this.inputs.SetState(id, this.inputs.GetState(id) == IOState.On ? IOState.Off : IOState.On);
        }


        /// <summary>
        /// Force the outputs to toggle. Will be picked up by state machine and also IO 
        /// raises an event so that the UI can adjust its display
        /// </summary>
        /// <param name="id">The output identifier</param>
        private void ToggleOutput(OutputId id) {
            this.outputs.SetState(id, this.outputs.GetState(id) == IOState.On ? IOState.Off : IOState.On);
        }

        #endregion

        #region Button events

        private void btnAbort_Click(object sender, RoutedEventArgs e) {
        }


        private void btnStart_Click(object sender, RoutedEventArgs e) {
        }


        private void btnExit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        #endregion

        #region Private

        private void SetupIO() {
            // TODO - Setup will be moved to machine code

            this.outputs.Add(OutputId.GasNitrogen);
            this.outputs.Add(OutputId.GasOxygen);
            this.outputs.Add(OutputId.Heater);

            this.inputs.Add(InputId.GasNitrogen);
            this.inputs.Add(InputId.GasOxygen);
            this.inputs.Add(InputId.Heater);

            this.outputs.StateChange += this.Outputs_StateChange;
        }


        /// <summary>Event handler from ouputs</summary>
        /// <param name="sender">The object sending the event (DemoOutput)</param>
        /// <param name="e">Event args (OutputChangeArgs)</param>
        private void Outputs_StateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                OutputChangeArgs args = (OutputChangeArgs)e;
                switch (args.Id) {
                    case OutputId.GasNitrogen:
                        this.SetOutputCtrl(this.gasNitrogen, args.State);
                        break;
                    case OutputId.GasOxygen:
                        this.SetOutputCtrl(this.gasOxygen, args.State);
                        break;
                    case OutputId.Heater:
                        this.SetOutputCtrl(this.heater, args.State);
                        break;
                }
            });
        }


        private void SetOutputCtrl(UC_Output output, IOState state) {
            output.SetState(state == IOState.On ? UC_Output.State.On : UC_Output.State.Off);
        }

        #endregion
    }
}
