using System;
using System.Runtime.Serialization;
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
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    [DataContract]
    public class SpBaseEventMsg : ISpEventMessage {

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

        /// <summary>
        /// Unique identifier of message. Setter only for WCF transfer
        /// </summary>
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

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpBaseEventMsg() {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeId">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        /// <param name="priority">The priority of the message</param>
        public SpBaseEventMsg(ISpToInt typeId, ISpToInt eventId, SpEventPriority priority) {
            this.typeId = typeId.ToInt();
            this.eventId = eventId.ToInt();
            this.priority = priority;
        }
        
        /// <summary>
        /// Constructor for Normal Priority messages
        /// </summary>
        /// <param name="typeId">The type id to cast to derived for payload retrieval</param>
        /// <param name="eventId">The event identifier</param>
        public SpBaseEventMsg(ISpToInt typeId, ISpToInt eventId)
            : this(typeId, eventId, SpEventPriority.Normal) {
        }

        #endregion

    }
}
