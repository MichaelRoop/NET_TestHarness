
using System.Collections.Generic;

namespace SpStateMachine.Interfaces {

    /// <summary>Interface for defining the state interface</summary>
    /// <typeparam name="TEvent">Event id (TEvent)</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpState <TEvent> where TEvent : struct {

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

        #region State change methods

        /// <summary>Excecuted once when the state becomes the current state</summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition<TEvent> OnEntry(ISpEventMessage msg);


        /// <summary>Called on every other period after entry</summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition<TEvent> OnTick(ISpEventMessage msg);


        /// <summary>Always invoked on object exit</summary>
        void OnExit();

        #endregion

        #region Transition registration methods for methods on receiving an event

        /// <summary>Register a state transition from incoming event</summary>
        /// <param name="eventId">The event id</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnEventTransition(TEvent eventId, ISpStateTransition<TEvent> transition);


        /// <summary>Register transition to next state on event</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        void ToNextOnEvent(TEvent ev, ISpState<TEvent> newState);


        /// <summary>Register transition to next state on event</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        /// <param name="returnMsg">The message to return on transition</param>
        void ToNextOnEvent(TEvent ev, ISpState<TEvent> newState, ISpEventMessage returnMsg);


        /// <summary>Register exit transition on event</summary>
        /// <param name="ev">The event</param>
        void ToExitOnEvent(TEvent ev);


        /// <summary>Register defered action on event</summary>
        /// <param name="ev">The event</param>
        void ToDeferedOnEvent(TEvent ev);

        #endregion

        #region Transition registration methods for methods after internal processing

        /// <summary>Register state transition from the result of state processing</summary>
        /// <param name="responseId">The result id the state returns as the result of processing</param>
        /// <param name="transition">The transition object</param>
        void RegisterOnResultTransition(TEvent responseId, ISpStateTransition<TEvent> transition);


        /// <summary>Register transition to next state on processing results</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        void ToNextOnResult(TEvent ev, ISpState<TEvent> newState);


        /// <summary>Register transition to next state on processing results</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        /// <param name="returnMsg">The message to return on transition</param>
        void ToNextOnResult(TEvent ev, ISpState<TEvent> newState, ISpEventMessage returnMsg);

        #endregion

    }
}
