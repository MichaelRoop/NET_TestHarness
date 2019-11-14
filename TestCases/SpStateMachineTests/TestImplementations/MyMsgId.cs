using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public enum MyMsgId : int {

        Tick = 0,

        Start,

        Stop,

        Abort,

        /// <summary>Test event to push a class to do a defered transition</summary>
        ThinkAboutIt,

        ExitAborted,

        StartGas,
        StopGas,
        OpenWater,
        CloseWater,
        StartHeater,
        StopHeater,

        // -- reponses
        RespDone,

    }

}
