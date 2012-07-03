using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;
using SpStateMachine.Core;
using LogUtils;

namespace SpStateMachine.States {

    public abstract class SpSuperState<T> : SpState<T> where T : class {

        #region Data 

        /// <summary>The current sub state of this super state</summary>
        ISpState currentState = null;

        /// <summary>The sub state that is the starting state of this super state</summary>
        ISpState entryState = null;


        /// <summary>List of this state's substates</summary>
        List<ISpState> substates = new List<ISpState>();

        private readonly string className = "SpSuperState";

        #endregion

        #region ISpState Properties

        /// <summary>
        /// From Get the fully resolved state name in format
        /// grandparent.parent.state. The super state gets the information from its
        /// current state rather than that current state will contain the proper
        /// currently resolved state of the state machine
        /// </summary>
        public override string FullName {
            get {
                // TODO - need to thread protect access to current thread var
                if (this.currentState != null) {
                    return this.currentState.FullName;
                }
                return base.FullName;
            }
        }

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
        /// this superstate. It also becomes the current state for the first tick
        /// </summary>
        /// <param name="state"></param>
        public void SetEntryState(ISpState state) {
            this.entryState = state;

            // Set the current state at the same time
            this.currentState = this.entryState;
        }

        #endregion

        #region ISpState overrides

        public override ISpStateTransition OnEntry(ISpMessage msg) {
            Log.Info(this.className, "OnEntry", this.FullName);
            WrapErr.ChkVar(this.entryState, 9999, "The 'SentEntryState() Must be Called in the Constructor");

            // TODO - Find if there are exit conditions OnEntry at the SuperState level and excecute them first - might have to exit immediately
            // return transition
            this.SetEntered(true);
            this.currentState = this.entryState;

            // Could also intercept this also
            return this.currentState.OnEntry(msg);
        }


        public override ISpStateTransition OnTick(ISpMessage msg) {
            Log.Info(this.className, "OnTick", this.FullName);

            WrapErr.ChkVar(this.entryState, 9999, "The 'SentEntryState() Must be Called in the Constructor");
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
            return this.ReadTransitionType(stateFunc.Invoke(msg), msg, false);


            //// Invoke the state method
            //ISpStateTransition tr = stateFunc.Invoke(msg);
            //WrapErr.ChkVar(tr, 9999, "The returned transition is null");
            //switch (tr.TransitionType) {
            //    case SpStateTransitionType.SameState:
            //        return tr;
            //    case SpStateTransitionType.NextState:
            //        return this.HandleNextState(tr, msg);
            //    case SpStateTransitionType.ExitState:
            //        return this.HandleExitState(msg);
            //    case SpStateTransitionType.Defered:
            //        // Call override method that child will use to handle decision point and create a new message with event
            //        ISpMessage deferedMsg = this.OnRuntimeTransitionRequest(msg);

            //        // Get the registered transition for the new event that the child pushed
            //        ISpStateTransition deferedTr = this.GetTransitionFromOnResultRegistrations(deferedMsg);
            //        WrapErr.ChkVar(deferedTr, 9999, "The defered msg id did not find a valid transition");

            //        switch (deferedTr.TransitionType) {
            //            case SpStateTransitionType.SameState:
            //                return deferedTr;
            //            case SpStateTransitionType.NextState:
            //                // TODO - this will be a bit different as the transition that comes back should have the new message ??
            //                return this.HandleNextState(deferedTr, deferedMsg);
            //            case SpStateTransitionType.ExitState:
            //                return this.HandleExitState(deferedMsg);
            //            default:
            //                WrapErr.ChkTrue(false, 9999, String.Format("Transition Type {0} not valid from Defered", deferedTr.TransitionType));
            //                return deferedTr;
            //        }
            //    default:
            //        WrapErr.ChkTrue(false, 9999, String.Format("Transition Type {0} not Handled", tr.TransitionType));
            //        return tr;
            //}
        }


        ISpStateTransition ReadTransitionType(ISpStateTransition tr, ISpMessage msg, bool superStateLevelEvent) {
            WrapErr.ChkVar(tr, 9999, "The transition is null");
            switch (tr.TransitionType) {
                case SpStateTransitionType.SameState:
                    return tr;
                case SpStateTransitionType.NextState:
                    return this.HandleNextState(tr, msg);
                case SpStateTransitionType.ExitState:
                    return this.HandleExitState(msg);
                case SpStateTransitionType.Defered:
                    // Prevent endless recursion - TODO - I think that we can allow it to recurse here but just return immediately 
                    // with the Transition object instead of throwing an exception
                    WrapErr.ChkFalse(superStateLevelEvent, 9999, "Cannot map from Defered transition to another Defered Transition");
                    
                    // Call override method that child will use to handle decision point and create a new message with event
                    ISpMessage deferedMsg = this.OnRuntimeTransitionRequest(msg);

                    // Get the registered transition for the new event that the child pushed
                    ISpStateTransition deferedTr = this.GetTransitionFromOnResultRegistrations(deferedMsg);
                    WrapErr.ChkVar(deferedTr, 9999, "The defered msg id did not find a valid transition");

                    return this.ReadTransitionType(deferedTr, deferedMsg, true);
                default:
                    WrapErr.ChkTrue(false, 9999, String.Format("Transition Type {0} not Handled", tr.TransitionType));
                    return tr;
            }
        }



        private ISpStateTransition HandleNextState(ISpStateTransition tr, ISpMessage msg) {
            WrapErr.ChkTrue(tr.TransitionType == SpStateTransitionType.NextState, 9999, 
                () => { return String.Format("{0} is not NextState", tr.TransitionType);});

            WrapErr.ChkVar(tr.NextState, 9999, () => { return
                String.Format(
                    "State {0} Specified Next State on Event {1} but Next State Null",
                    this.currentState.FullName, msg.EventId);
            });

            // TODO - determine if return message is already attached or needs attaching

            this.currentState.OnExit();
            this.currentState = tr.NextState;

            // Reset transition to SameState to prevent other transitions along the chain on return
            tr.TransitionType = SpStateTransitionType.SameState;
            tr.NextState = null;
            return tr;
        }

        private ISpStateTransition HandleExitState(ISpMessage msg) {
            // Check super state registered result transitions against Sub State event id
            ISpStateTransition tr = this.GetTransitionFromOnResultRegistrations(msg);
            WrapErr.ChkVar(tr, 9999, () => {
                return String.Format(
                    "State {0} Specified Exit but SuperState {1} has no handlers for that event id:{2}", 
                    this.currentState.FullName, this.FullName, msg.EventId);
            });

            // At this point, the transition registered to the superstate should have everything set in it
            return tr;    
        }


        protected virtual ISpMessage OnRuntimeTransitionRequest(ISpMessage msg) {
            WrapErr.ChkTrue(false, 9999,
                String.Format("Was not overrided"));
            return msg;
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
            Log.Info(this.className, "ExecOnExit", this.FullName);

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


        protected ISpState GetCurrentState() {
            return this.currentState;
        }

        // TODO - NOW REDUNDANT.  MOVE CODE TO WHERE USED ?

        private ISpStateTransition GetSuperStateOnEventTransition(ISpMessage msg) {
            ISpStateTransition tr = this.GetTransitionFromOnEventRegistrations(msg);
            if (tr != null) {
                // Get the appropriate related response message to add to transition
                tr.ReturnMessage = this.OnGetResponseMsg(msg);
            }
            return tr;
        }



    }
}
