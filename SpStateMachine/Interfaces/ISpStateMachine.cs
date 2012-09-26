using System;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Operational interface into the states comprising the 
    /// state machine
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpStateMachine : IDisposable {

        /// <summary>
        /// The current state which has all the ancestors and the actual
        /// operational state as the leaf.
        /// </summary>
        string CurrentStateName { get; }


        /// <summary>
        /// Tick the current state to execute the action based on the event message
        /// </summary>
        /// <param name="eventMessage">The event message</param>
        /// <returns>The return message from the action</returns>
        ISpEventMessage Tick(ISpEventMessage eventMessage);

    }
}
