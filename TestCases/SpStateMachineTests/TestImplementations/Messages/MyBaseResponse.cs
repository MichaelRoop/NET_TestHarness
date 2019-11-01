using SpStateMachine.Interfaces;
using SpStateMachine.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {

    /// <summary>
    /// Implementation of SpBaseEventResponse with msg, msg type, return enums 
    /// </summary>
    /// <author>Michael Roop</author>
    public class MyBaseResponse : SpBaseEventResponse<MyMsgType,MyMsgId,MyReturnCode> {

        /// <summary>Base response type for strong type check on enumssummary>
        /// <param name="msgType">The return message type</param>
        /// <param name="msg">Paired msg for this response which transmits the UID for correlation</param>
        /// <param name="code">The return code</param>
        /// <param name="status">The return string</param>
        public MyBaseResponse(MyMsgType msgType, ISpEventMessage msg, MyReturnCode code, string status)
            : base(msgType, msg, code, status) {
        }

    }
}
