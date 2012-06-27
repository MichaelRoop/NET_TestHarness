using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for a generic event type for the SpStateMachine architecture
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpEvent {

        // TODO - may need a GUID to correlate it to the event response

        /// <summary>
        /// The unique event ID
        /// </summary>
        int Id { get; set; }


        /// <summary>
        /// The event priority
        /// </summary>
        SpEventPriority Priority { get; set; }

    }
}
