using SpStateMachine.Core;
using SpStateMachine.Messages;

namespace SpStateMachineDemo.Net.Messaging {

    /// <summary>Msg Base implementation with defined generic parameters</summary>
    public class DemoMsgBase : SpBaseEventMsg<DemoMsgType, DemoMsgId> {

        /// <summary>Constructor</summary>
        /// <param name="msgType">The message type</param>
        /// <param name="msgId">The message identifier</param>
        /// <param name="priority">The message priority</param>
        public DemoMsgBase(DemoMsgType msgType, DemoMsgId msgId, SpEventPriority priority) :
            base (msgType, msgId, priority) {
        }


        /// <summary>Constructor for normal priority message</summary>
        /// <param name="msgType">The message type</param>
        /// <param name="msgId">The message identifier</param>
        public DemoMsgBase(DemoMsgType msgType, DemoMsgId msgId) :
            base(msgType, msgId, SpEventPriority.Normal) {
        }



    }
}
