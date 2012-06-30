﻿using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Contains the necessary information to execute a state transition
    /// </summary>
    /// <author>Michael Roop</author>
    public sealed class SpStateTransition : ISpStateTransition {

        #region Data 

        SpStateTransitionType type = SpStateTransitionType.SameState;

        ISpState nextState = null;

        ISpMessage returnMsg = null;

        #endregion

        #region ISpStateTransition Members

        /// <summary>
        /// The type of transition to execute
        /// </summary>
        public SpStateTransitionType TransitionType {
            get { 
                return this.type; 
            }
            set {
                this.type = value;
            }
        }


        /// <summary>
        /// The registered next state for NextState transitions
        /// </summary>
        public ISpState NextState {
            get {
                return this.nextState;
            }
            set {
                this.nextState = value;
            }
        }


        /// <summary>
        /// The response message to return to caller
        /// </summary>
        public ISpMessage ReturnMessage {
            get {
                return this.returnMsg;
            }
            set {
                this.returnMsg = value;
            }
        }

        #endregion
        
        #region Constructors 

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpStateTransition() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The transition type</param>
        /// <param name="nextState">The next state for next state transitions</param>
        /// <param name="returnMsg">The repsponse to return to the caller</param>
        public SpStateTransition(SpStateTransitionType type, ISpState nextState, ISpMessage returnMsg) {
            this.type = type;
            this.nextState = nextState;
            this.returnMsg = returnMsg;
        }

        #endregion
        
    }
}