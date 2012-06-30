using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;

namespace SpStateMachine.States {

    public abstract class SpSuperState<T> : SpState<T> where T : class {

        #region Data 

        /// <summary>The current sub state of this super state</summary>
        ISpState currentState = null;

        /// <summary>The sub state that is the starting state of this super state</summary>
        ISpState entryState = null;


        /// <summary>List of this state's substates</summary>
        List<ISpState> substates = new List<ISpState>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for first level state
        /// </summary>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpSuperState(int id, T wrappedObject)
            : base(id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpSuperState(ISpState parent, int id, T wrappedObject)
            : base(parent, id, wrappedObject) {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a state to the list of sub states
        /// </summary>
        /// <param name="state"></param>
        public void AddSubState(ISpState state) {
            this.substates.Add(state);
        }


        /// <summary>
        /// One of the substates must be listed as the start state of
        /// this superstate
        /// </summary>
        /// <param name="state"></param>
        public void SetEntryState(ISpState state) {
            this.entryState = state;
        }

        #endregion

        #region ISpState overrides

        public override ISpStateTransition OnEntry(ISpMessage msg) {
            // Find if there are exit conditions on event if so exit immediately
            // return transition
            this.SetEntered(true);
            this.currentState = this.entryState;

            // Could also intercept this
            return this.currentState.OnEntry(msg);
        }


        public override ISpStateTransition OnTick(ISpMessage msg) {
            WrapErr.ChkVar(this.currentState, 9999, "Current state is not set");

            // Find if there are OnEvent transitions registered at the superstate level first
            ISpStateTransition tr = GetSuperStateOnEventTransition(msg);
            if (tr != null) {
                return tr;
            }
            
            // behavior of only one call per tick
            if (!this.currentState.IsEntryExcecuted) {
                return this.GetTransition(this.currentState.OnEntry, msg);
            }
            else {
                return this.GetTransition(this.currentState.OnTick, msg);
            }
        }

        ISpStateTransition GetTransition(Func<ISpMessage, ISpStateTransition> stateFunc, ISpMessage msg) {
            
            // Invoke the state method
            ISpStateTransition tr = stateFunc.Invoke(msg);
            if (!tr.HasTransition) {
                // No transition required, simple exit
                return tr;
            }

            // If transition and next state is known, set new state and return transition object.
            if (tr.NextState != null) {
                this.currentState.OnExit();
                this.currentState = tr.NextState;

                // Reset the transition to false so it will not provoke other transitions along the chain
                tr.HasTransition = false;
                tr.NextState = null;
                return tr;
            }
            
            // Check super state registered transitions on result
            ISpStateTransition ssTr = this.GetOnResultTransition(msg);
            WrapErr.ChkVar(ssTr, 9999, String.Format("Superstate: {0} No handlers for event id:{1}", this.Name, msg.EventId));
            if (ssTr.HasTransition && ssTr.NextState == null) {
                ssTr = this.OnRuntimeTransitionRequest(ssTr, msg);
            }

            if (ssTr.HasTransition) {
                WrapErr.ChkVar(ssTr.NextState, 9999, String.Format("SuperState:{0} No Next State on event Id:{1}", this.Name, msg.EventId));
                this.currentState.OnExit();
                this.currentState = ssTr.NextState;

                // Reset the transition to false so it will not provoke other transitions along the chain
                ssTr.HasTransition = false;
                ssTr.NextState = null;
            }
            return ssTr;
        }



        protected virtual ISpStateTransition OnRuntimeTransitionRequest(ISpStateTransition tr, ISpMessage msg) {
            WrapErr.ChkTrue(false, 9999, 
                String.Format(""));
            return tr;
        }


        #endregion

        #region SpState overrides

        /// <summary>
        /// Override and seal the ExecOnExit to automate common super state 
        /// entry functionality. After execution the SuperStateOnExit()
        /// is called.
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>The appropriate return message</returns>
        protected sealed override void ExecOnExit() {
            if (!this.IsEntryExcecuted) {
                this.SetEntered(false);
                if (this.currentState != null) {
                    this.currentState.OnExit();
                    this.currentState = null;
                }
                // TODO - an overrideable method 
            }
        }


        #endregion

        #region Virtual Protected Methods

        //protected virtual ISpMessage SuperStateExceOnEntry(ISpMessage msg) {
        //    // Nothing done
        //    return msg;
        //}


        //protected virtual void SuperStateExecOnExit() {
        //    // Nothing done
        //}


        #endregion


        private ISpStateTransition GetSuperStateOnEventTransition(ISpMessage msg) {
            ISpStateTransition tr = this.GetOnEventTransition(msg);
            if (tr != null) {
                // Get the appropriate related response message to add to transition
                tr.ReturnMessage = this.OnGetResponseMsg(msg);
            }
            return tr;
        }



    }
}
