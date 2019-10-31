using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.Messaging;
using SpStateMachineDemo.Net.Messaging.Responses;
using System;

namespace SpStateMachineDemo.Net.Core {

    public class DemoMsgProvider : ISpMsgProvider {

        #region ISpMsgProvider Members

        /// <summary>Default response to any message is OK</summary>
        /// <param name="msg">The message to link to the reponse</param>
        /// <returns>The default response</returns>
        public ISpEventMessage DefaultMsg(ISpEventMessage msg) {
            return new ResponseOk(msg);
        }


        /// <summary>Return a response for the message</summary>
        /// <param name="msg">The message link to this response</param>
        /// <returns>The response to the message</returns>
        public ISpEventMessage Response(ISpEventMessage msg) {

            DemoMsgId id = SpConverter.IntToEnum<DemoMsgId>(msg.EventId);
            DemoMsgType _type = SpConverter.IntToEnum<DemoMsgType>(msg.TypeId);

            // Only one type of message for now. 
            // Responses also in the same enum as messages so have to handle
            switch (_type) {
                case DemoMsgType.SimpleMsg:
                    // TODO Hmm should this be a response instead of a message?
                    return new DemoMsgBase(_type, id, msg.Priority) {
                        ReturnCode = msg.ReturnCode,
                        StringPayload = msg.StringPayload,
                        ReturnStatus = msg.ReturnStatus,
                        // You can add any other info from derived message 
                    };

                case DemoMsgType.SimpleResponse:
                    throw new Exception(string.Format("Response '{0}' is not a valid message", _type));
                default:
                    throw new Exception(string.Format("Unhandled message '{0}'", _type));
            }


        }

        public ISpEventMessage Response(ISpEventMessage msg, ISpEventMessage registeredMsg) {
            if (registeredMsg == null) {
                return this.Response(msg);
            }

            // In this scenario we would make a copy of the registered msg and 
            // copy in the data from the original incoming message

            // We only have simple messages. Now we can change the data from the cloned 
            // message stored in the transition with the incoming message data. We will preserve 
            // the event and message type from stored object
            ISpEventMessage ret = this.Response(registeredMsg);
            ret.Priority = msg.Priority;
            ret.ReturnCode = msg.ReturnCode;
            ret.ReturnStatus = msg.ReturnStatus;
            ret.StringPayload = msg.StringPayload;
            return ret;
        }

        #endregion
    }
}
