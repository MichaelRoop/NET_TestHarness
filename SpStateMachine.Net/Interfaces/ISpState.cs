
using System.Collections.Generic;

namespace SpStateMachine.Interfaces {

    /// <summary>Interface for defining the state interface</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpState <T> where T: struct {

        #region Properties

        /// <summary>The unique integer state identifier</summary>
        int Id { get; }

        /// <summary>Get the full id by combining nested int ids</summary>
        List<int> IdChain { get; }

        /// <summary>This state object name with no reference to ancestors or children</summary>
        string Name { get; }

        /// <summary>
        /// Resolved state name in format parent.parent.state with this state object
        /// being the leaf. No reference to any children in the chain
        /// </summary>
        string FullName { get; }
        
        /// <summary>
        /// Fully resolved state name in format parent.parent.state.substate.substate with 
        /// all acestors and children until the farthest sub state being the leaf
        /// </summary>
        string CurrentStateName { get; }

        #endregion

        #region Methods

        /// <summary>Excecuted once when the state becomes the current state</summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition<T> OnEntry(ISpEventMessage msg);


        /// <summary>Called on every other period after entry</summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition<T> OnTick(ISpEventMessage msg);


        /// <summary>Always invoked on object exit</summary>
        void OnExit();


        /// <summary>Register a state transition from incoming event</summary>
        /// <param name="eventId">The id converter of the incoming event</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnEventTransition(ISpToInt eventId, ISpStateTransition<T> transition);


        /// <summary>Register state transition from the result of state processing</summary>
        /// <param name="responseId">The id converter of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnResultTransition(ISpToInt responseId, ISpStateTransition<T> transition);



        void RegisterOnEventTransition(T eventId, ISpStateTransition<T> transition);

        void RegisterOnResultTransition(T responseId, ISpStateTransition<T> transition);



        void ToNextOnEvent(T ev, ISpState<T> newState);
        void ToNextOnEvent(T ev, ISpState<T> newState, ISpEventMessage returnMsg);
        void ToExitOnEvent(T ev);
        void ToDeferedOnEvent(T ev);
        
        void ToNextOnResult(T ev, ISpState<T> newState);
        void ToNextOnResult(T ev, ISpState<T> newState, ISpEventMessage returnMsg);

        #endregion

    }
}
