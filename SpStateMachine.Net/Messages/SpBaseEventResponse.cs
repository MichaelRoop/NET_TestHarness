using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Converters;
using SpStateMachine.Core;

namespace SpStateMachine.Messages {

    /// <summary>
    /// Base class of a response which transfers the GUID from the message 
    /// so it can be correlated and returned to sender. The base which it is
    /// derived from is WCF friendly. To maintain that capability, any classes
    /// derived with data fields will need to mark them with [DataMember] and
    /// have the class type present as part of the data contract
    /// </summary>
    /// <remarks>
    /// The derived classes can be cast based on the relationship to the 
    /// typeId to get at included payloads.
    /// </remarks>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpBaseEventResponse : SpBaseEventMsg {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeId">The type identifier</param>
        /// <param name="msg">The message that this is responding to</param>
        /// <param name="returnCode">The operation return code</param>
        /// <param name="returnStatus">Additional information on the operation status</param>
        public SpBaseEventResponse(ISpToInt typeId, ISpToInt eventId, SpEventPriority priority, ISpEventMessage msg, ISpToInt returnCode, string returnStatus)
            : base(typeId, eventId, priority) {
            // Transfer the message guid to the response for correlation
            this.Uid = msg.Uid;
            this.ReturnCode = returnCode.ToInt();
            this.ReturnStatus = ReturnStatus;
        }


        // TODO - remove this - do not want to copy the event id automatically ??
        public SpBaseEventResponse(ISpToInt typeId, ISpEventMessage msg, ISpToInt returnCode, string returnStatus)
            : base(typeId, new SpIntToInt(msg.EventId)) {
            // Transfer the message guid to the response for correlation
            this.Uid = msg.Uid;
            this.ReturnCode = returnCode.ToInt();
            this.ReturnStatus = ReturnStatus;
        }


    }
}
