using SpStateMachineDemo.Net.DemoMachine;
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

        DemoMachineWrapper machineWrapper = new DemoMachineWrapper();

        public MainWindow() {
            InitializeComponent();
            this.machineWrapper.Init();

            this.machineWrapper.OutputStateChange += this.Outputs_StateChange;
            this.machineWrapper.InputStateChange += this.Inputs_StateChange;
        }

        #region Windows events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            // We could intercept this until done
            this.machineWrapper.Teardown();
            this.machineWrapper.OutputStateChange -= this.Outputs_StateChange;
            this.machineWrapper.InputStateChange -= this.Inputs_StateChange;
        }

        #endregion

        #region Fake a hardware output change to exercise state machine

        private void btnOxygenOut_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(OutputId.GasOxygen);
        }

        private void btnNitrogenOut_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(OutputId.GasNitrogen);
        }

        private void btnHeatOut_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(OutputId.Heater);
        }

        #endregion

        #region Fake a hardware input change to exercise state machine

        private void btnOxygenIn_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(InputId.GasOxygen);
        }

        private void btnNitrogenIn_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(InputId.GasNitrogen);
        }

        private void btnHeatIn_Click(object sender, RoutedEventArgs e) {
            this.machineWrapper.ToggleIO(InputId.Heater);
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

        private void Inputs_StateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                InputChangeArgs args = (InputChangeArgs)e;
                switch (args.Id) {
                    case InputId.GasNitrogen:
                        this.SetIOCtrl(this.gasNitrogenIn, args.State);
                        break;
                    case InputId.GasOxygen:
                        this.SetIOCtrl(this.gasOxygenIn, args.State);
                        break;
                    case InputId.Heater:
                        this.SetIOCtrl(this.heaterIn, args.State);
                        break;
                }
            });
        }


        /// <summary>Event handler from ouputs</summary>
        /// <param name="sender">The object sending the event (DemoOutput)</param>
        /// <param name="e">Event args (OutputChangeArgs)</param>
        private void Outputs_StateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                OutputChangeArgs args = (OutputChangeArgs)e;
                switch (args.Id) {
                    case OutputId.GasNitrogen:
                        this.SetIOCtrl(this.gasNitrogenOut, args.State);
                        break;
                    case OutputId.GasOxygen:
                        this.SetIOCtrl(this.gasOxygenOut, args.State);
                        break;
                    case OutputId.Heater:
                        this.SetIOCtrl(this.heaterOut, args.State);
                        break;
                }
            });
        }


        private void SetIOCtrl(UC_Output ctrl, IOState state) {
            ctrl.SetState(state == IOState.On ? UC_Output.State.On : UC_Output.State.Off);
        }

        private UC_Output.State Translate(IOState state) {
            return state == IOState.On ? UC_Output.State.On : UC_Output.State.Off;
        }

        #endregion

    }
}
