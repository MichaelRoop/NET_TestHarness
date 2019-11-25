using System;
using System.Runtime.Serialization;
using ChkUtils.Net;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Messages {

    /// <summary>
    /// Serializable base class for the SpStateMachine messages and responses. This
    /// is WCF friendly and can be extended for WCF provided any extra data elements
    /// are also defined as [DataMember] and registered as part of the contract.
    /// </summary>
    /// <remarks>
    /// Users can use the BaseMsg and BaseResponse directly and use enums with
    /// Extension methods to convert them to int.
    /// </remarks>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    [DataContract]
    public class SpBaseEventMsg<TMsgType,TMsgId> : ISpEventMessage 
        where TMsgType : struct 
        where TMsgId : struct {

        #region Data

        /// <summary>Unique identifier of message</summary>
        private Guid guid = Guid.NewGuid();

        /// <summary>Type identifier in case the message has to be cast to get at the payload</summary>
        private int typeId = 0;

        /// <summary>Event id for the state machine to interpret</summary>
        private int eventId = 0;

        /// <summary>The event priority</summary>
        private SpEventPriority priority = SpEventPriority.Normal;

        /// <summary>Simple string payload field</summary>
        private string stringPayload = "";

        /// <summary>Used for response messages to report on operation status</summary>
        private int returnCode = 0;

        /// <summary>Used for response messages to report on operation status</summary>
        string returnStatus = "";
        
        #endregion
        
        #region ISpMessage Properties

        /// <summary>Unique identifier of message. Setter only for WCF transfer</summary>
        [DataMember]
        public Guid Uid {
            get {
                return this.guid;
            }
            set {
                this.guid = value;
            }
        }

        /// <summary>
        /// Type identifier in case the message has to be cast to get at the 
        /// payload. . Setter only for WCF transfer
        /// </summary>
        [DataMember]
        public int TypeId {
            get {
                return this.typeId;
            }
            set {
                this.typeId = value;
            }
        }

        /// <summary>
        /// Event id for the state machine to interpret. . Setter only for WCF transfer
        /// </summary>
        [DataMember]
        public int EventId {
            get {
                return this.eventId;
            }
            set {
                this.eventId = value;
            }
        }

        /// <summary>
        /// The event priority. . Setter only for WCF transfer
        /// </summary>
        [DataMember]
        public SpEventPriority Priority {
            get {
                return this.priority;
            }
            set {
                this.priority = value;
            }
        }
        
        /// <summary>
        /// Simple string field payload if needed
        /// </summary>
        public string StringPayload { 
            get {
                return this.stringPayload;
            }
            set {
                this.stringPayload = value;
            }
        }
        
        /// <summary>
        /// Used for response messages to report on operation status. . Setter only for WCF transfer
        /// </summary>
        public int ReturnCode {
            get {
                return this.returnCode;
            }
            set {
                this.returnCode = value;
            }
        }

        /// <summary>
        /// Used for response messages to report on operation status. . Setter only for WCF transfer
        /// </summary>
        public string ReturnStatus {
            get {
                return this.returnStatus;
            }
            set {
                this.returnStatus = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>Default constructor in private scope to prevent usage</summary>
        private SpBaseEventMsg() {
        }


        /// <summary>Constructor for Normal Priority messages</summary>
        /// <param name="_type">The type id to cast to derived for payload retrieval</param>
        /// <param name="id">The event identifier</param>
        public SpBaseEventMsg(TMsgType _type, int id) {
            WrapErr.ChkTrue(typeof(TMsgType).IsEnum, 9999, () => string.Format("Event type {0} must be Enum", _type.GetType().Name));
            WrapErr.ChkTrue(typeof(TMsgType).GetEnumUnderlyingType() == typeof(int), 9999,
                () => string.Format("Event type enum {0} must be derived from int", id.GetType().Name));

            this.typeId = Convert.ToInt32(_type);
            this.eventId = id;
            this.priority = SpEventPriority.Normal;
        }



        /// <summary>Constructor</summary>
        /// <param name="_type">Type of message</param>
        /// <param name="id">Message identifier</param>
        /// <param name="priority">Message priority</param>
        public SpBaseEventMsg(TMsgType _type, TMsgId id, SpEventPriority priority) {
            WrapErr.ChkTrue(typeof(TMsgType).IsEnum, 9999, () => string.Format("Event type {0} must be Enum", _type.GetType().Name));
            WrapErr.ChkTrue(typeof(TMsgType).GetEnumUnderlyingType() == typeof(int), 9999,
                () => string.Format("Event type enum {0} must be derived from int", id.GetType().Name));

            WrapErr.ChkTrue(typeof(TMsgId).IsEnum, 9999, () => string.Format("Event id {0} must be Enum", id.GetType().Name));
            WrapErr.ChkTrue(typeof(TMsgId).GetEnumUnderlyingType() == typeof(int), 9999,
                () => string.Format("Event id enum {0} must be derived from int", id.GetType().Name));

            this.typeId = Convert.ToInt32(_type);
            this.eventId = Convert.ToInt32(id);
            this.priority = priority;
        }


        /// <summary>Constructor</summary>
        /// <param name="_type">Type of message</param>
        /// <param name="id">Message identifier</param>
        public SpBaseEventMsg(TMsgType _type, TMsgId id)
            : this(_type, id, SpEventPriority.Normal) {
        }

        #endregion

    }
}
