using SpStateMachine.Interfaces;

namespace SpStateMachine.States {

    /// <summary>Implementation of SpState handles virtuals left exposed by the State Base</summary>
    /// <typeparam name="T">Generic type that the state represents</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpState<T,TEvent,TState,TMsg> : SpStateBase<T,TEvent,TState,TMsg> where T : class where TEvent : struct where TState : struct where TMsg : struct {

        #region ISpState Sealed Properties

        /// <summary>
        /// Get the fully resolved state name in format parent.parent.state.substate with 
        /// the all the acestors and children until the farthest leaf
        /// state being the leaf
        /// </summary>
        public sealed override string CurrentStateName {
            get {
                return this.FullName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for first level state
        /// </summary>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpMsgFactory msgFactory, TState id, T wrappedObject)
            : base(msgFactory, id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpState<TEvent> parent, ISpMsgFactory msgFactory, TState id, T wrappedObject) 
            : base (parent, msgFactory, id, wrappedObject) {
        }

        #endregion

        #region ISpState Sealed Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public sealed override ISpStateTransition<TEvent> OnEntry(ISpEventMessage msg) {
            return base.OnEntry(msg);
        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public sealed override ISpStateTransition<TEvent> OnTick(ISpEventMessage msg) {
            return base.OnTick(msg);
        }

        #endregion

    }
}
