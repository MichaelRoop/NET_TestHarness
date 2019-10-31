using SpStateMachine.Converters;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace SpStateMachineDemo.Net.Messaging.Responses {

    /// <summary>Default OK response</summary>
    public class ResponseOk : DemoMsgBase {

        // TODO - can this be from base response instead or Msg?

        /// <summary>Constructor</summary>
        /// <param name="msg">The message linked to this response</param>
        public ResponseOk(ISpEventMessage msg) : this(msg, "") {
        }


        /// <summary>Constructor</summary>
        /// <param name="msg">The message linked to this response</param>
        /// <param name="status">The status string of the response</param>
        public ResponseOk(ISpEventMessage msg, string status) :
            base(DemoMsgType.SimpleResponse, DemoMsgId.Tick, SpEventPriority.Normal) {
            this.ReturnCode = SpConverter.EnumToInt(DemoReponseId.Success);
            this.ReturnStatus = status;
            this.Uid = msg.Uid;
        }

    }
}
