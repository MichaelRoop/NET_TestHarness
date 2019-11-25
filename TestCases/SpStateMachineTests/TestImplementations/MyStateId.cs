using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    #region State ID Enum

    public enum MyStateID : int {

        Default,        // Super state
        Main,           // Super state
        NotStarted,     // Super state
        Started,        // Super state
        Recovery,       // Super state

        Level2,
        Level3,


        //======================================
        // State level

        Idle,
        Active,
        Failed,
        WaitingForUserInput,    
        
        // Result exit tests
        S_A1,
        S_A2,
        S_A1_ExitEntry,
        SS_A1,
        SS_B1,
        SS_M,
        SS_M2,
        S_B1,

        Undefined,
    }

    #endregion

}
