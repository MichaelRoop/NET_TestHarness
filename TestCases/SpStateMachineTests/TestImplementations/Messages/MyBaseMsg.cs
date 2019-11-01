using SpStateMachine.Core;
using SpStateMachine.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {

    /// <summary>Sample implementation of BaseMsg with msg type, msg id enums</summary>
    public class MyBaseMsg : SpBaseEventMsg<MyMsgType,MyMsgId> {

        /// <summary>Constructor</summary>
        /// <param name="type">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        /// <param name="priority">The priority of the message</param>
        public MyBaseMsg(MyMsgType msgType, MyMsgId msgId, SpEventPriority priority)
            : base(msgType, msgId, priority) {
        }


        /// <summary>Constructor for Normal Priority messages</summary>
        /// <param name="msgType">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        public MyBaseMsg(MyMsgType msgType, MyMsgId eventId)
            : this(msgType, eventId, SpEventPriority.Normal) {
        }

    }

}
