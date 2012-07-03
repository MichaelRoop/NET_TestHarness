using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;
using SpStateMachine.Messages;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {

    /// <summary>
    /// Sample implementation of BaseMsg with strong typing of type and
    /// event by using enums
    /// </summary>
    /// <author>Michael Roop</author>
    public class MyBaseResponse : SpBaseEventResponse {

        /// <summary>
        /// Base response type for strong type check on enums
        /// </summary>
        /// <param name="msgType">The return message type</param>
        /// <param name="msg">
        /// The paired msg for this response which transmits the UID for correlation
        /// </param>
        /// <param name="code">The return code</param>
        /// <param name="status">The return string</param>
        public MyBaseResponse(MyMsgType msgType, ISpEventMessage msg, MyReturnCode code, string status)
            : base(msgType.Int(), msg, code.Int(), status) {
        }


    }
}
