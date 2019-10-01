using SpStateMachine.Interfaces;

namespace SpStateMachine.States {

    /// <summary>
    /// Implementation of the SpState which handles the virtuals left exposed 
    /// by the State Base
    /// </summary>
    /// <typeparam name="T">Generic type that the state represents</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpState<T> : SpStateBase<T> where T : class {

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
        /// <param name="idConverter">The integer id to string converter</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpMsgFactory msgFactory, ISpIdConverter idConverter, ISpToInt id, T wrappedObject)
            : base(msgFactory, idConverter, id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="idConverter">The integer id to string converter</param>
        /// <param name="id">Unique state id converter</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpState parent, ISpMsgFactory msgFactory, ISpIdConverter idConverter, ISpToInt id, T wrappedObject) 
            : base (parent, msgFactory, idConverter, id, wrappedObject) {
        }

        #endregion

        #region ISpState Sealed Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public sealed override ISpStateTransition OnEntry(ISpEventMessage msg) {
            return base.OnEntry(msg);
        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public sealed override ISpStateTransition OnTick(ISpEventMessage msg) {
            return base.OnTick(msg);
        }

        #region Sealed Methods

        #endregion

        #endregion

    }
}
