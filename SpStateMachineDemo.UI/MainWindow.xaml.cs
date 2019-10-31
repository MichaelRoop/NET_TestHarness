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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        #region Windows events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            // TODO - code to shut down state machine
            MessageBox.Show("Shutting down state machine");
        }

        #endregion

        #region Fake a hardware output change to exercise state machine

        // These functions will in the future actuate the fake IO block which 
        // will raise an event which will set the output displays

        private void btnOxygen_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(this.gasOxygen);
        }

        private void btnNitrogen_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(this.gasNitrogen);
        }

        private void btnHeat_Click(object sender, RoutedEventArgs e) {
            this.ToggleOutput(this.heater);
        }

        private void ToggleOutput(UC_Output output) {
            output.SetState(output.IsOn ? UC_Output.State.Off : UC_Output.State.On);
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

    }
}
