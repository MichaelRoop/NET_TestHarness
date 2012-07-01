using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    #region State ID Enum

    public enum MyStateID : int {

        Default,
        NotStarted,             // Super state
        Started,                // Super state

        //======================================
        // State level

        Idle,
        Active,
        Failed,
        WaitingForUserInput,    
        
        Undefined,
    }

    #endregion

    #region State ID Enum Extensions

    /// <summary>
    /// Sample use of enums to provide strong typing for ids
    /// </summary>
    public static class MyStateIdExtensions {

        /// <summary>
        /// Transform the id enum to int
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Int(this MyStateID self) {
            return (int)self;
        }

        public static MyStateID ToStateId(this int value) {
            MyStateID id = MyStateID.Undefined;
            WrapErr.ToErrReport(9999, () => { return String.Format(""); }, () => {
                id = (MyStateID)Enum.Parse(typeof(MyStateID), value.ToString());
            });
            return id;
        }
    }

    #endregion

}
