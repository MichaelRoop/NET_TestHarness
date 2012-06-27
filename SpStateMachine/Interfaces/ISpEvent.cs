using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for a generic event type for the SpStateMachine architecture
    /// </summary>
    /// <typeparam name="T">The payload type</typeparam>
    /// <author>Michael Roop</author>
    public interface ISpEvent<T> {

        /// <summary>
        /// The unique event ID
        /// </summary>
        int Id { get; set; }


        /// <summary>
        /// The event priority
        /// </summary>
        SpEventPriority Priority { get; set; }


        /// <summary>
        /// The event payload
        /// </summary>
        T Payload { get; set; }

    }
}
