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
        }

        #region Windows events

        private void Window_ContentRendered(object sender, EventArgs e) {
            this.machineWrapper.StateMachineStateChange += this.MachineWrapper_StateMachineStateChange;
            this.machineWrapper.OutputStateChange += this.Outputs_StateChange;
            this.machineWrapper.InputStateChange += this.Inputs_StateChange;
            this.machineWrapper.Init();

            LogUtils.Net.Log.Warning(0, "In the content rendered");

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            // We could intercept this until done
            this.machineWrapper.Teardown();
            this.machineWrapper.OutputStateChange -= this.Outputs_StateChange;
            this.machineWrapper.InputStateChange -= this.Inputs_StateChange;
            this.machineWrapper.StateMachineStateChange -= this.MachineWrapper_StateMachineStateChange;
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

        private void MachineWrapper_StateMachineStateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                this.txtState.Text = ((StateChangeArgs)e).State;
            });
        }


        private void Inputs_StateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                InputChangeArgs args = (InputChangeArgs)e;
                this.GetCtrl(args.Id).Set(args.State);
            });
        }


        /// <summary>Event handler from ouputs</summary>
        /// <param name="sender">The object sending the event (DemoOutput)</param>
        /// <param name="e">Event args (OutputChangeArgs)</param>
        private void Outputs_StateChange(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                OutputChangeArgs args = (OutputChangeArgs)e;
                this.GetCtrl(args.Id).Set(args.State);
            });
        }


        /// <summary>Get the IO visual for an IO output</summary>
        /// <param name="id">The state machine output id</param>
        /// <returns>The corresponding visual object</returns>
        UC_Output GetCtrl(OutputId id) {
            switch (id) {
                case OutputId.GasNitrogen:  return this.gasNitrogenOut;
                case OutputId.GasOxygen:    return this.gasOxygenOut;
                case OutputId.Heater:       return this.heaterOut;
                default:                    return this.gasNitrogenOut;
            }
        }


        /// <summary>Get the IO visual for an IO input</summary>
        /// <param name="id">The state machine input id</param>
        /// <returns>The corresponding visual object</returns>
        UC_Output GetCtrl(InputId id) {
            switch (id) {
                case InputId.GasNitrogen: return this.gasNitrogenIn;
                case InputId.GasOxygen: return this.gasOxygenIn;
                case InputId.Heater: return this.heaterIn;
                default: return this.gasNitrogenIn;
            }
        }

        #endregion

    }

    #region Extensions

    /// <summary>Extensions to join the IO objects with the UI controls</summary>
    public static class IOCtrlExtensions {

        /// <summary>Set the visual state of the IO control from the state machine state</summary>
        /// <param name="ctrl">The visual control</param>
        /// <param name="state">The state to change</param>
        public static void Set(this UC_Output ctrl, IOState state) {
            ctrl.SetState(state == IOState.On ? UC_Output.State.On : UC_Output.State.Off);
        }
    }

    #endregion

}
