using ChkUtils.Net;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;

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
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpBaseEventResponse<TMsgType,TMsgId,TReturn> : SpBaseEventMsg<TMsgType, TMsgId> 
        where TMsgType : struct 
        where TMsgId : struct
        where TReturn : struct {

        /// <summary>Constructor</summary>
        /// <param name="_type">Message type identifier</param>
        /// <param name="id">Message identifier</param>
        /// <param name="priority">Message priority</param>
        /// <param name="msg">The message</param>
        /// <param name="_return">Return code</param>
        /// <param name="returnStatus">String return status</param>
        public SpBaseEventResponse(TMsgType _type, TMsgId id, SpEventPriority priority, ISpEventMessage msg, TReturn _return, string returnStatus)
            : base(_type, id, priority) {
            // Transfer the message guid to the response for correlation
            this.Uid = msg.Uid;
            this.ReturnCode = this.GetReturnId(_return);
            this.ReturnStatus = ReturnStatus;
        }


        // TODO - remove this - do not want to copy the event id automatically ??
        public SpBaseEventResponse(TMsgType _type, ISpEventMessage msg, TReturn _return, string returnStatus)
            : base(_type, msg.EventId) {
            // Transfer the message guid to the response for correlation
            this.Uid = msg.Uid;
            this.ReturnCode = this.GetReturnId(_return);
            this.ReturnStatus = ReturnStatus;
        }


        //// TODO - remove this - do not want to copy the event id automatically ??
        //public SpBaseEventResponse(TMsgType _type, ISpEventMessage msg, TReturn _return, string returnStatus)
        //    : base(_type, new SpIntToInt(msg.EventId)) {
        //    // Transfer the message guid to the response for correlation
        //    this.Uid = msg.Uid;
        //    this.ReturnCode = this.GetReturnId(_return);
        //    this.ReturnStatus = ReturnStatus;
        //}


        //public SpBaseEventResponse(TMsgType _type, TMsgId id, SpEventPriority priority, ISpEventMessage msg, ISpToInt returnCode, string returnStatus)
        //    : base(_type, id, priority) {
        //    // Transfer the message guid to the response for correlation
        //    this.Uid = msg.Uid;
        //    this.ReturnCode = returnCode.ToInt();
        //    this.ReturnStatus = ReturnStatus;
        //}





        private int GetReturnId(TReturn _return) {
            WrapErr.ChkTrue(typeof(TReturn).IsEnum, 9999, () => string.Format("Return id {0} must be Enum", _return.GetType().Name));
            WrapErr.ChkTrue(typeof(TReturn).GetEnumUnderlyingType() == typeof(int), 9999,
                () => string.Format("Event id enum {0} must be derived from int", _return.GetType().Name));
            return Convert.ToInt32(_return);
        }

    }
}
