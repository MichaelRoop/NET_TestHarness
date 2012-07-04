using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {


    /// <summary>
    /// Sample implementation of BaseMsg with strong typing of type and
    /// event by using enums
    /// </summary>
    public class MyBaseMsg : SpBaseEventMsg {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        /// <param name="priority">The priority of the message</param>
        public MyBaseMsg(MyMsgType msgType, MyEventType eventType, SpEventPriority priority)
            : base(msgType.Int(), SpConverter.EnumToInt(eventType), priority) {
        }


        /// <summary>
        /// Constructor for Normal Priority messages
        /// </summary>
        /// <param name="typeId">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        public MyBaseMsg(MyMsgType msgType, MyEventType eventId)
            : this(msgType, eventId, SpEventPriority.Normal) {
        }


    }

}
