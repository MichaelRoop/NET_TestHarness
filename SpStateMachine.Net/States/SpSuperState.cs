using ChkUtils.Net;
using LogUtils.Net;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;

namespace SpStateMachine.States {

    /// <summary>
    /// Implementation of the SpSuperState which handles the virtuals left exposed 
    /// by the State Base and acts as a container of SpState objects as sub states
    /// </summary>
    /// <typeparam name="TMachine">Object that the state represents</typeparam>
    /// <typeparam name="TMsgId">Message id (TEvent)</typeparam>
    /// <typeparam name="TState">State id (TState)</typeparam>
    /// <typeparam name="TMsgType">Message type</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpSuperState<TMachine,TMsgId,TState,TMsgType> : SpStateBase<TMachine,TMsgId,TState,TMsgType> where TMachine : class where TMsgId : struct where TState : struct where TMsgType : struct {

        #region Data 

        /// <summary>The current sub state of this super state</summary>
        ISpState<TMsgId> currentState = null;

        /// <summary>The sub state that is the starting state of this super state</summary>
        ISpState<TMsgId> entryState = null;

        /// <summary>List of this state's substates</summary>
        List<ISpState<TMsgId>> substates = new List<ISpState<TMsgId>>();

        private readonly string className = "SpSuperState";

        #endregion

        #region ISpState Properties

        /// <summary>
        /// Fully resolved state name in format parent.parent.state.substate.substate with 
        /// all acestors and children until the farthest sub state being the leaf
        /// </summary>
        public sealed override string CurrentStateName {
            get {
                lock (this) {
                    if (this.currentState != null) {
                        return this.currentState.CurrentStateName;
                    }
                    // In this case the current super state is the leaf
                    return this.FullName;
                }
            }
        }
        
        #endregion

        #region Constructors

        /// <summary>Constructor for first level state</summary>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpSuperState(ISpMsgFactory msgFactory, TState id, TMachine wrappedObject)
            : base(msgFactory, id, wrappedObject) {
        }


        /// <summary>Constructor</summary>
        /// <param name="parent">The parent state</param>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpSuperState(ISpState<TMsgId> parent, ISpMsgFactory msgFactory, TState id, TMachine wrappedObject)
            : base(parent, msgFactory, id, wrappedObject) {
        }

        #endregion

        #region Public Methods

        /// <summary>Add a state to the list of sub states</summary>
        /// <param name="state">The state to add</param>
        public ISpState<TMsgId> AddSubState(ISpState<TMsgId> state) {
            this.substates.Add(state);
            return state;
        }


        /// <summary>
        /// One of the substates must be listed as the start state of
        /// this superstate. It also becomes the current state for the first tick
        /// </summary>
        /// <param name="state"></param>
        public void SetEntryState(ISpState<TMsgId> state) {
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
        public sealed override ISpStateTransition<TMsgId> OnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "OnEntry", String.Format("'{0}' State Event {1}", this.FullName, this.GetCachedEventId(msg.EventId)));
            WrapErr.ChkVar(this.entryState, 9999, "The 'SentEntryState() Must be Called in the Constructor");

            // Find if there are exit conditions OnEntry at the SuperState level and excecute them first 
            // This will check OnEvent transitions queue and transitions from the overriden ExecOnEntry
            ISpStateTransition<TMsgId> t = base.OnEntry(msg);
            if (t.TransitionType != SpStateTransitionType.SameState) {
                return t;
            }

            // Get the substate that was set as first for this superstate
            this.currentState = this.entryState;

            // Call entry on the first substate
            ISpStateTransition<TMsgId> tr = this.currentState.OnEntry(msg);

            // TODO check logic and see if it can be moved to the SpMachine
            // If the current state's transition is call to exit the substate, then we need 
            // to get the registered next state transistion and call exit on the current and 
            // entry on that new substate
            if (tr.TransitionType == SpStateTransitionType.ExitState) {
                ISpStateTransition<TMsgId> sstr = null;
                if (tr.ReturnMessage != null) {
                    sstr = this.GetSuperStateOnResultTransition(tr.ReturnMessage);
                    if (sstr != null) {
                        if (sstr.NextState != null) {
                            this.currentState.OnExit();
                            this.currentState = sstr.NextState;
                            // This should handle any recursion since it is called on another object
                            // TODO - not sure.  Need to test multiple recursion scenarios
                            return this.currentState.OnEntry(msg);
                        }
                        return sstr;
                    }
                }
            }

            return tr;
        }


        /// <summary>
        /// Execute on each tick period
        /// </summary>
        /// <param name="msg">The incoming message with event</param>
        /// <returns>The return transition object with result information</returns>
        public sealed override ISpStateTransition<TMsgId> OnTick(ISpEventMessage msg) {
            //Log.Info(this.className, "OnTick", String.Format("'{0}' State", this.FullName));
            WrapErr.ChkVar(this.entryState, 9999, "The 'SetEntryState() Must be Called in the Constructor");
            WrapErr.ChkVar(this.currentState, 9999, "Current state is not set");
            WrapErr.ChkTrue(this.IsEntryExcecuted, 9999, "Tick Being Called before OnEntry");

            // If there are OnEvent transitions registered at the superstate level return immediately
            ISpStateTransition<TMsgId> tr = GetSuperStateOnEventTransition(msg);
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
        ISpStateTransition<TMsgId> GetTransition(Func<ISpEventMessage, ISpStateTransition<TMsgId>> stateFunc, ISpEventMessage msg) {
            ISpStateTransition<TMsgId> tr = stateFunc.Invoke(msg);
            if (tr.TransitionType == SpStateTransitionType.ExitState) {
                ISpStateTransition<TMsgId> sstr = null;
                if (tr.ReturnMessage != null) {
                    sstr = this.GetSuperStateOnResultTransition(tr.ReturnMessage);
                    if (sstr != null) {
                        return sstr;
                    }
                }
            }
        
        return this.ReadTransitionType(tr, msg, false);

        //return this.ReadTransitionType(stateFunc.Invoke(msg), msg, false);
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
        ISpStateTransition<TMsgId> ReadTransitionType(ISpStateTransition<TMsgId> tr, ISpEventMessage msg, bool superStateLevelEvent) {
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
        private ISpStateTransition<TMsgId> HandleNextStateTransitionType(ISpStateTransition<TMsgId> tr, ISpEventMessage msg) {
            Log.Info(this.className, "HandleNextStateTransitionType", String.Format("'{0}' State", this.FullName));

            WrapErr.ChkTrue(tr.TransitionType == SpStateTransitionType.NextState, 9999, 
                () => { return String.Format("{0} is not NextState", tr.TransitionType);});

            WrapErr.ChkVar(tr.NextState, 9999, () => { return
                String.Format(
                    "State {0} Specified Next State on Event {1} but Next State Null",
                    this.currentState.FullName, this.GetCachedEventId(msg.EventId));
            });

            this.currentState.OnExit();
            this.currentState = tr.NextState;
            return this.currentState.OnEntry(this.MsgFactory.GetDefaultResponse(msg));
        }


        /// <summary>
        /// Handle the substate ExitState Transition Type by using the event msg passed to it to do a lookup
        /// of this super state's transitions. The super state should have a transition that has been
        /// registered to the same id
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private ISpStateTransition<TMsgId> HandleExitStateTransitionType(ISpEventMessage msg) {
            Log.Info(this.className, "HandleExitStateTransitionType", 
                String.Format("State '{0}' msg type {1} msg id {2}", this.FullName, msg.TypeId, msg.EventId));

            // TODO - this is really only another kind of defered. The difference is that the superstate does not
            // TODO     called at runtime to handle the event. Rather the event is passed to the super state's
            // TODO     registered events. In this scenario it may not actually exit the super state but process
            //          the event id to something else. The difference is that the results are being determined
            //          by the registrations at the superstate level rather than the sub state level

            // Check super state registered result transitions against Sub State event id
            ISpStateTransition<TMsgId> tr = this.GetSuperStateOnResultTransition(msg);
            WrapErr.ChkVar(tr, 9999, () => {
                return String.Format(
                    "State {0} Specified Exit but SuperState {1} has no handlers for that event id:{2}",
                    this.currentState.FullName, this.FullName, this.GetCachedEventId(msg.EventId));
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
        private ISpStateTransition<TMsgId> HandleDeferedStateTransitionType(ISpStateTransition<TMsgId> tr, ISpEventMessage msg, bool fromSuperState) {
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
        private ISpStateTransition<TMsgId> GetSuperStateOnResultTransition(ISpEventMessage msg) {
            
            // Check super state registered result transitions against Sub State event id
            ISpStateTransition<TMsgId> tr = this.GetOnResultTransition(msg);
            WrapErr.ChkVar(tr, 9999, () => {
                return String.Format(
                    "State {0} Specified Exit but SuperState {1} has no handlers for that event id:{2}",
                    this.currentState.FullName, this.FullName, this.GetCachedEventId(msg.EventId));
            });

            tr.ReturnMessage = this.MsgFactory.GetResponse(msg, tr.ReturnMessage);
            return tr;
        }


        private ISpStateTransition<TMsgId> GetSuperStateOnEventTransition(ISpEventMessage msg) {
            ISpStateTransition<TMsgId> tr = this.GetOnEventTransition(msg);
            if (tr != null) {
                tr.ReturnMessage = (tr.ReturnMessage == null) 
                    ? this.MsgFactory.GetResponse(msg) 
                    : this.MsgFactory.GetResponse(tr.ReturnMessage); 
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
