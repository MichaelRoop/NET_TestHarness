using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpStateMachineDemo.UI.Controls {

    /// <summary>Interaction logic for UC_Input.xaml</summary>
    public partial class UC_Output : UserControl {

        public enum State {
            On,
            Off,
        }


        public string CtrlName {
            get { return (string)this.GetValue(CtrlNameProperty); }
            set { this.SetValue(CtrlNameProperty, value); }
        }

        public bool IsOn { get; private set; }


        public UC_Output() {
            InitializeComponent();
            // Only funnel the selected scope DataContext to avoid overriding all
            this.layoutRoot.DataContext = this;
            this.SetState(State.Off);
        }


        public void SetState(UC_Output.State state) {
            switch (state) {
                case UC_Output.State.On:
                    this.rectBlock.Fill = new SolidColorBrush(Colors.LightGreen);
                    this.IsOn = true;
                    break;
                case UC_Output.State.Off:
                    this.rectBlock.Fill = new SolidColorBrush(Colors.LightGray);
                    this.IsOn = false;
                    break;
            }
        }

        #region Dependency binding

        public static readonly DependencyProperty CtrlNameProperty =
            DependencyProperty.Register(
                "CtrlName", 
                typeof(string), 
                typeof(UC_Output), 
                new PropertyMetadata(null));

        //https://blog.scottlogic.com/2012/02/06/a-simple-pattern-for-creating-re-useable-usercontrols-in-wpf-silverlight.html

        #endregion

    }
}
