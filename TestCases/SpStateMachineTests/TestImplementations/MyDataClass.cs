using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>
    /// The shared class that is represented by the state machine. The 
    /// properties and methods are accessible to the states
    /// by all states
    /// </summary>
    public class MyDataClass : IDisposable {

        #region Data

        private int intVal = 0;
        private string strVal = "Original value";

        private int flipStateCount = -1;
        private int flipStateCounter = 0;

        #endregion

        #region Properties

        public int IntVal { get { return this.intVal; } set { this.intVal = value; } }
        public string StrVal { get { return this.strVal; } set { this.strVal = value; } }

        public int FlipStateCount { get { return this.flipStateCount; } set { this.flipStateCount = value; } }

        public bool DoIFlipStates {
            get {
                if (this.flipStateCount == -1) {
                    return false;
                }
                else {
                    if ((++this.flipStateCounter) >= this.flipStateCount) {
                        this.flipStateCount = 0;
                        return true;
                    }
                    return false;
                }
            }
        }


        #endregion

        #region Public Methods

        public string GetMessage() {
            return "This is a message";
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            // Nothing to do. Provided for compatibility
        }

        #endregion
    }


}
