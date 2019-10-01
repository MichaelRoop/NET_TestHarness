using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {


    /// <summary>
    /// Encapsulate the state machine behavior on events received
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpBehaviorOnEvent : IDisposable {

        /// <summary>
        /// Invoked when an event is received
        /// </summary>
        /// <param name="eventType">The type of event received</param>
        void EventReceived(BehaviorResponseEventType eventType);


        /// <summary>
        /// Wait indefinitely until behavior is satisfied
        /// </summary>
        void WaitOnEvent();
        
    }
}
