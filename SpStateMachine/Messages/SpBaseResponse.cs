using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Messages {

    /// <summary>
    /// Base class of a message response using values from the same message
    /// </summary>
    /// <author>Michael Roop</author>
    public class SpBaseResponse : SpBaseMsg {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeId">
        /// The type identifier in case you need to cast to a derived type to retrieve a payload
        /// </param>
        /// <param name="msg">The message that this is responding to</param>
        /// <param name="returnCode">The operation return code</param>
        /// <param name="returnStatus">Additional information on the operation status</param>
        public SpBaseResponse(int typeId, SpBaseMsg msg, int returnCode, string returnStatus)
            : base(typeId, msg.EventId) {
                // Transfer the message guid to the response for correlation
                this.Uid = msg.Uid;
                this.ReturnCode = returnCode;
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
        public SpBaseResponse(int typeId, SpBaseMsg msg)
            : this(typeId, msg, 0, "") {
        }

    }
}
