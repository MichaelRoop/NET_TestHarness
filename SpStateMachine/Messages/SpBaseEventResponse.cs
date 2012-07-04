using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Converters;

namespace SpStateMachine.Messages {

    /// <summary>
    /// Base class of a message response using values from an ISpEventMessage. 
    /// The most important it the GUID which can then be used to correlate responses
    /// to their parent message. This is WCF friendly and can be extended for WCF 
    /// provided any extra data elements are also defined as [DataMember] and 
    /// registered as part of the contract.
    /// </summary>
    /// <author>Michael Roop</author>
    public class SpBaseEventResponse : SpBaseEventMsg {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeId">
        /// The type identifier in case you need to cast to a derived type to retrieve a payload
        /// </param>
        /// <param name="msg">The message that this is responding to</param>
        /// <param name="returnCode">The operation return code</param>
        /// <param name="returnStatus">Additional information on the operation status</param>
        public SpBaseEventResponse(ISpToInt typeId, ISpEventMessage msg, ISpToInt returnCode, string returnStatus)
            : base(typeId, new SpIntToInt(msg.EventId)) {
                // Transfer the message guid to the response for correlation
                this.Uid = msg.Uid;
                this.ReturnCode = returnCode.ToInt();
                this.ReturnStatus = ReturnStatus;
        }


        /// <summary>
        /// Constructor for a successful response with respond code 0 and no additional string
        /// information on operation status
        /// </summary>
        /// <param name="typeId">
        /// The type identifier in case you need to cast to a derived type to retrieve a payload
        /// </param>
        /// <param name="msg">The message that this is responding to</param>
        public SpBaseEventResponse(ISpToInt typeId, ISpEventMessage msg)
            : this(typeId, msg, new SpIntToInt(0), "") {
        }

    }
}
