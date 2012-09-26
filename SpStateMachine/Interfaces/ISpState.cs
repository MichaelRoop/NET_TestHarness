
using System.Collections.Generic;
using SpStateMachine.Core;
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for defining the state interface
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpState {

        #region Properties

        /// <summary>
        /// The unique state identifier as an integer
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Get the full id by combining nested int ids
        /// </summary>
        List<int> IdChain { get; }

        /// <summary>
        /// Get the name of the state alone without reference to ancestors (i.e. parent.state)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the fully resolved state name in format parent.parent.state with the current
        /// state being the leaf
        /// </summary>
        string FullName { get; }
        
        /// <summary>
        /// Get the fully resolved state name in format parent.parent.state.substate with 
        /// all acestors and children until the farthest sub state being the leaf
        /// </summary>
        string CurrentStateName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnEntry(ISpEventMessage msg);


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnTick(ISpEventMessage msg);


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        void OnExit();


        /// <summary>
        /// Register a state transition from incoming event
        /// </summary>
        /// <param name="eventId">The id converter of the incoming event</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnEventTransition(ISpToInt eventId, ISpStateTransition transition);

        /// <summary>
        /// Register a state transition from the result of state processing
        /// </summary>
        /// <param name="responseId">The id converter of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnResultTransition(ISpToInt responseId, ISpStateTransition transition); 


        #endregion

    }
}
