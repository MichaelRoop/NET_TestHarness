﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for a message event type for the SpStateMachine architecture
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpEventMessage {

        /// <summary>
        /// The message unique identifier used to correlate a response
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// The derived class type ID in case the message needs to be cast to
        /// a derived type to access a payload
        /// </summary>
        int TypeId { get; set; }
        
        /// <summary>
        /// Identifies an event id that the SpStateMachine states can interpret. In many
        /// cases this will be sufficient information and a cast will not be necessary
        /// </summary>
        int EventId { get; set; }
        
        /// <summary>
        /// The message priority
        /// </summary>
        SpEventPriority Priority { get; set; }

        /// <summary>
        /// Simple string field payload if needed
        /// </summary>
        string StringPayload { get; set; }

        /// <summary>
        /// Used for response messages to report on operation status
        /// </summary>
        int ReturnCode { get; set; }

        /// <summary>
        /// Used for response messages to report on operation status
        /// </summary>
        string ReturnStatus { get; set; }

    }
}
