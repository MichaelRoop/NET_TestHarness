using SpStateMachine.Core;
using System;

namespace SpStateMachine.Interfaces {

    /// <summary>Encapsulate state machine behavior on events received</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpBehaviorOnEvent : IDisposable {

        /// <summary>Invoked when an event is received</summary>
        /// <param name="eventType">Type of event received</param>
        void EventReceived(BehaviorResponseEventType eventType);


        /// <summary>Wait indefinitely until behavior is satisfied</summary>
        void WaitOnEvent();
        
    }
}
