
using System.Collections.Generic;
using SpStateMachine.Core;
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for defining the state interface
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpState {

        #region Properties

        /// <summary>
        /// Queries if OnEntry has already been invoked. It can only
        /// be invoked once until the OnExit is called
        /// </summary>
        bool IsEntryExcecuted { get; }

        /// <summary>
        /// The unique state identifier
        /// </summary>
        int Id { get; }


        /// <summary>
        /// Get the full id by combining nested ids
        /// </summary>
        List<int> IdChain { get; }

        /// <summary>
        /// Get the fully resolved state name in format
        /// parent.parent.state
        /// </summary>
        string Name { get; }


        #endregion

        #region Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnEntry(ISpMessage msg);


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnTick(ISpMessage msg);


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        void OnExit();


        /// <summary>
        /// Register a state transition from incoming event
        /// </summary>
        /// <param name="eventId">The id of the incoming event</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnEventTransition(int eventId, ISpStateTransition transition);

        /// <summary>
        /// Register a state transition from the result of state processing
        /// </summary>
        /// <param name="responseId">The id of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnResultTransition(int responseId, ISpStateTransition transition); 


        #endregion

    }
}
