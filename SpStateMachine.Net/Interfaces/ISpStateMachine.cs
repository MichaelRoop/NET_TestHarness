using System;

namespace SpStateMachine.Interfaces {

    /// <summary>Operational interface into the states comprising the state machine</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpStateMachine : IDisposable {

        /// <summary>Current state which has all the ancestors and the actualoperational state as the leaf</summary>
        string CurrentStateName { get; }


        /// <summary>Tick current state to execute the action based on the event message</summary>
        /// <param name="msg">The message pulled from the incoming message queue</param>
        /// <returns>The return message from registered event handling or internal results</returns>
        ISpEventMessage Tick(ISpEventMessage msg);

    }
}
