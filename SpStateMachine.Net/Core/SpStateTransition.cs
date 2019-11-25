using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Contains the necessary information to execute a state transition
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public sealed class SpStateTransition<TMsgId> : ISpStateTransition<TMsgId> where TMsgId : struct {

        #region Data 

        private SpStateTransitionType type = SpStateTransitionType.SameState;

        private ISpState<TMsgId> nextState = null;

        private ISpEventMessage returnMsg = null;

        #endregion

        #region ISpStateTransition Members

        /// <summary>The type of transition to execute</summary>
        public SpStateTransitionType TransitionType {
            get { 
                return this.type; 
            }
            set {
                this.type = value;
            }
        }


        /// <summary>Registered next state for NextState transitions</summary>
        public ISpState<TMsgId> NextState {
            get {
                return this.nextState;
            }
            set {
                this.nextState = value;
            }
        }


        /// <summary>Response message to return to caller</summary>
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
            SpStateTransition<TMsgId> st = this.MemberwiseClone() as SpStateTransition<TMsgId>;
            st.TransitionType = this.TransitionType;
            st.NextState = this.nextState;
            st.ReturnMessage = this.returnMsg;
            return st;
        }

        #endregion

        #region Static Helpers

        public static SpStateTransition<TMsgId> ToNext(ISpState<TMsgId> nextState) {
            return new SpStateTransition<TMsgId>(
                SpStateTransitionType.NextState, nextState, null);
        }


        public static SpStateTransition<TMsgId> ToNext(ISpState<TMsgId> nextState, ISpEventMessage returnMsg) {
            return new SpStateTransition<TMsgId>(
                SpStateTransitionType.NextState, nextState, returnMsg);
        }

        public static SpStateTransition<TMsgId> ToExit() {
            return new SpStateTransition<TMsgId>(SpStateTransitionType.ExitState, null, null);
        }

        public static SpStateTransition<TMsgId> ToDefered() {
            return new SpStateTransition<TMsgId>(SpStateTransitionType.Defered, null, null);
        }
        
        #endregion

        #region Constructors 

        /// <summary>Default constructor in private scope to prevent usage</summary>
        private SpStateTransition() {
        }


        /// <summary>Constructor</summary>
        /// <param name="type">The transition type</param>
        /// <param name="nextState">The next state for next state transitions</param>
        /// <param name="returnMsg">The repsponse to return to the caller</param>
        public SpStateTransition(SpStateTransitionType type, ISpState<TMsgId> nextState, ISpEventMessage returnMsg) {
            this.type = type;
            this.nextState = nextState;
            this.returnMsg = returnMsg;
        }

        #endregion

    }
}
