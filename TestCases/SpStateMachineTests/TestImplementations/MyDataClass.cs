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

        #endregion

        #region Properties

        public int IntVal { get { return this.intVal; } set { this.intVal = value; } }
        public string StrVal { get { return this.strVal; } set { this.strVal = value; } }

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
