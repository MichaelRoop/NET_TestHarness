using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Contains the necessary information to execute a state transition
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public sealed class SpStateTransition : ISpStateTransition {

        #region Data 

        SpStateTransitionType type = SpStateTransitionType.SameState;

        ISpState nextState = null;

        ISpEventMessage returnMsg = null;

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
        public ISpEventMessage ReturnMessage {
            get {
                return this.returnMsg;
            }
            set {
                this.returnMsg = value;
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Make a copy of this object. Note that the copy is only of the pointers
        /// for the ISpState and ISpMessage. Those get set to a new object and I
        /// am only concerned about not changing what the fields in the containers
        /// point to
        /// </summary>
        /// <returns></returns>
        public object Clone() {
            SpStateTransition st = this.MemberwiseClone() as SpStateTransition;
            st.TransitionType = this.TransitionType;
            st.NextState = this.nextState;
            st.ReturnMessage = this.returnMsg;
            return st;
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
        public SpStateTransition(SpStateTransitionType type, ISpState nextState, ISpEventMessage returnMsg) {
            this.type = type;
            this.nextState = nextState;
            this.returnMsg = returnMsg;
        }

        #endregion

    }
}
