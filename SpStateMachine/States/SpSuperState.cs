using System;
using System.Collections.Generic;
using ChkUtils;
using LogUtils;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;

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
        /// Get the fully resolved state name in format parent.parent.state.substate with 
        /// the all the acestors and children until the farthest leaf
        /// state being the leaf
        /// </summary>
        public sealed override string CurrentStateName {
            get {
                // TODO - need to thread protect access to current thread var
                if (this.currentState != null) {
                    return this.currentState.CurrentStateName;
                }

                // In this case the current super state is the leaf
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
        public SpSuperState(ISpMsgFactory msgFactory, ISpToInt id, T wrappedObject)
            : base(msgFactory, id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpSuperState(ISpState parent, ISpMsgFactory msgFactory, ISpToInt id, T wrappedObject)
            : base(parent, msgFactory, id, wrappedObject) {
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

        /// <summary>
        /// Execute logic on entry into this superstate
        /// </summary>
        /// <param name="msg">The incoming message with event</param>
        /// <returns>The return transition object with result information</returns>
        public sealed override ISpStateTransition OnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "OnEntry", String.Format("'{0}' State Event {1}", this.FullName, this.ConvertEventIdToString(msg.EventId)));
            WrapErr.ChkVar(this.entryState, 9999, "The 'SentEntryState() Must be Called in the Constructor");

            // Find if there are exit conditions OnEntry at the SuperState level and excecute them first 
            // This will check OnEvent transitions queue and transitions from the overriden ExecOnEntry
            ISpStateTransition t = base.OnEntry(msg);
            if (t.TransitionType != SpStateTransitionType.SameState) {
                return t;
            }

            // return transition
            this.currentState = this.entryState;
            return this.currentState.OnEntry(msg);
        }


        /// <summary>
        /// Execute on each tick period
        /// </summary>
        /// <param name="msg">The incoming message with event</param>
        /// <returns>The return transition object with result information</returns>
        public sealed override ISpStateTransition OnTick(ISpEventMessage msg) {
            //Log.Info(this.className, "OnTick", String.Format("'{0}' State", this.FullName));
            WrapErr.ChkVar(this.entryState, 9999, "The 'SetEntryState() Must be Called in the Constructor");
            WrapErr.ChkVar(this.currentState, 9999, "Current state is not set");
            WrapErr.ChkTrue(this.IsEntryExcecuted, 9999, "Tick Being Called before OnEntry");

            // If there are OnEvent transitions registered at the superstate level return immediately
            ISpStateTransition tr = GetSuperStateOnEventTransition(msg);
            if (tr != null) {
                return tr;
            }
            return this.GetTransition(this.currentState.OnTick, msg);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoke the passed in current substate method to get it's transition object then determine
        /// if there needs to be more processing done because of the transition object type.
        /// </summary>
        /// <param name="stateFunc">The current substate method to execute</param>
        /// <param name="msg">The incoming event message received</param>
        /// <returns>A Transtion object with the results of the state processing</returns>
        ISpStateTransition GetTransition(Func<ISpEventMessage, ISpStateTransition> stateFunc, ISpEventMessage msg) {
            return this.ReadTransitionType(stateFunc.Invoke(msg), msg, false);
        }


        /// <summary>
        /// Read the transition object to determine behavior
        /// </summary>
        /// <param name="tr">The transition object</param>
        /// <param name="msg">the current event message</param>
        /// <param name="superStateLevelEvent">
        ///  true if the transition object is from the current substate, false if the transition was generated
        ///  by the superstate based on a previous Defered Transition type generated from the substate. This 
        ///  prevents infinite recursion.
        /// </param>
        /// <returns>A Transtion object with the results of the state processing</returns>
        ISpStateTransition ReadTransitionType(ISpStateTransition tr, ISpEventMessage msg, bool superStateLevelEvent) {
            WrapErr.ChkVar(tr, 9999, "The transition is null");
            switch (tr.TransitionType) {
                case SpStateTransitionType.SameState:
                    return tr;
                case SpStateTransitionType.NextState:
                    return this.HandleNextStateTransitionType(tr, msg);
                case SpStateTransitionType.ExitState:
                    return this.HandleExitStateTransitionType(msg);
                case SpStateTransitionType.Defered:
                    return this.HandleDeferedStateTransitionType(tr, msg, superStateLevelEvent);
                default:
                    WrapErr.ChkTrue(false, 9999, String.Format("Transition Type {0} not Handled", tr.TransitionType));
                    return tr;
            }
        }


        /// <summary>
        /// Handle the NextState Transition type by setting the next state as 
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private ISpStateTransition HandleNextStateTransitionType(ISpStateTransition tr, ISpEventMessage msg) {
            Log.Info(this.className, "HandleNextStateTransitionType", String.Format("'{0}' State", this.FullName));

            WrapErr.ChkTrue(tr.TransitionType == SpStateTransitionType.NextState, 9999, 
                () => { return String.Format("{0} is not NextState", tr.TransitionType);});

            WrapErr.ChkVar(tr.NextState, 9999, () => { return
                String.Format(
                    "State {0} Specified Next State on Event {1} but Next State Null",
                    this.currentState.FullName, this.GetCachedEventId(msg.EventId));
            });

            // TODO - determine if return message is already attached or needs attaching

            this.currentState.OnExit();
            this.currentState = tr.NextState;

            // TODO - In this scenario we do not tick the OnEntry of the new current state until next iteration. Might have to change

            // Reset transition to SameState to prevent other transitions along the chain on return
            //tr.TransitionType = SpStateTransitionType.SameState;
            //tr.NextState = null;
            //return tr;

            // Probably want to get the default success return type and pass back? Or if it got here from previous error we do not want to lose that information
            if (tr.ReturnMessage == null) {
                return this.currentState.OnEntry(msg);
            }
            return this.currentState.OnEntry(tr.ReturnMessage);
        }


        /// <summary>
        /// Handle the substate ExitState Transition Type by using the event msg passed to it to do a lookup
        /// of this super state's transitions. The super state should have a transition that has been
        /// registered to the same id
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private ISpStateTransition HandleExitStateTransitionType(ISpEventMessage msg) {
            Log.Info(this.className, "HandleExitStateTransitionType", String.Format("'{0}' State", this.FullName));

            // TODO - this is really only another kind of defered. The difference is that the superstate does not
            // TODO     called at runtime to handle the event. Rather the event is passed to the super state's
            // TODO     registered events. In this scenario it may not actually exit the super state but process
            //          the event id to something else. The difference is that the results are being determined
            //          by the registrations at the superstate level rather than the sub state level

            // Check super state registered result transitions against Sub State event id
            ISpStateTransition tr = this.GetSuperStateOnResultTransition(msg);
            WrapErr.ChkVar(tr, 9999, () => {
                return String.Format(
                    "State {0} Specified Exit but SuperState {1} has no handlers for that event id:{2}",
                    this.currentState.FullName, this.FullName, this.ConvertEventIdToString(msg.EventId));
            });

            // At this point, the transition registered to the superstate should have everything set in it
            return tr;
        }


        /// <summary>
        /// Handle Defered transition objects from both the current substate and this super state
        /// </summary>
        /// <param name="tr">The current transition object</param>
        /// <param name="msg"></param>
        /// <param name="fromSuperState">
        /// true if the Transition if from the super state, false if from the substate
        /// </param>
        /// <returns>The Transition</returns>
        private ISpStateTransition HandleDeferedStateTransitionType(ISpStateTransition tr, ISpEventMessage msg, bool fromSuperState) {
            // If the superstate iteself has a Defered transition it will return immediately to parent
            if (fromSuperState) {
                return tr;
            }

            // Call super state override method that derived superstate will use to handle decision point and create a new message with event
            ISpEventMessage deferedMsg = this.OnRuntimeTransitionRequest(msg);

            // Process the transition type from the SubState level
            return this.ReadTransitionType(
                this.GetSuperStateOnResultTransition(deferedMsg), deferedMsg, true);
        }


        /// <summary>
        /// Retrieve the transition object from the OnResults queue of the super state
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private ISpStateTransition GetSuperStateOnResultTransition(ISpEventMessage msg) {

            // Check super state registered result transitions against Sub State event id
            ISpStateTransition tr = this.GetOnResultTransition(msg);
            WrapErr.ChkVar(tr, 9999, () => {
                return String.Format(
                    "State {0} Specified Exit but SuperState {1} has no handlers for that event id:{2}",
                    this.currentState.FullName, this.FullName, this.GetCachedEventId(msg.EventId));
            });

            // At this point, the transition registered to the superstate should have everything set in it  ????
            // TODO - check this out. Some on Result will be null
            if (tr.ReturnMessage == null) {
                tr.ReturnMessage = this.OnGetResponseMsg(msg);
            }
            else {
                // Transfer existing GUID to correlate with sent message
                // TODO - still would have to figure out how to transfer the payload for response
                tr.ReturnMessage.Uid = msg.Uid;
            }
            
            this.LogTransition(tr, msg);
            return tr;    
        }


        private ISpStateTransition GetSuperStateOnEventTransition(ISpEventMessage msg) {
            ISpStateTransition tr = this.GetOnEventTransition(msg);
            if (tr != null) {
                // Get the appropriate related response message to add to transition
//                tr.ReturnMessage = this.OnGetResponseMsg(msg);

                if (tr.ReturnMessage == null) {
                    tr.ReturnMessage = this.OnGetResponseMsg(msg);
                }
                else {
                    // Transfer existing GUID to correlate with sent message
                    // TODO - still would have to figure out how to transfer the payload for response
                    tr.ReturnMessage.Uid = msg.Uid;
                }
                this.LogTransition(tr, msg);
            }
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
            Log.Info(this.className, "ExecOnExit", this.FullName);
            WrapErr.ChkTrue(this.IsEntryExcecuted, 9999, () => { 
                return String.Format("ExecOnExit called Before OnEntry {0} State", this.FullName);
            });

            if (this.currentState != null) {
                this.currentState.OnExit();
            }
            this.currentState = this.entryState;
        }


        #endregion

        #region Virtual Protected Methods

        /// <summary>
        /// The derived super state will override if it has some code to execute on the 
        /// Defered transition type
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual ISpEventMessage OnRuntimeTransitionRequest(ISpEventMessage msg) {
            WrapErr.ChkTrue(false, 9999, () => {
                return String.Format("Not Overriden to Define Runtime Handling of Defered Transition for {0} State", this.FullName);
            });
            return msg;
        }

        #endregion

    }
}
