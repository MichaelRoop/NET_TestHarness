using ChkUtils.Net;
using LogUtils.Net;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachine.Net.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachine.States {

    /// <summary>Base implementation of the ISpState interface</summary>
    /// <typeparam name="TMachine">Object type that the state represents</typeparam>
    /// <typeparam name="TMsgId">Message id (TEvent)</typeparam>
    /// <typeparam name="TState">State id (TState)</typeparam>
    /// <typeparam name="TMsgType">Message type</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpStateBase<TMachine,TMsgId,TState,TMsgType> : ISpState<TMsgId> where TMachine : class where TMsgId: struct where TState : struct where TMsgType : struct {

        #region Data

        /// <summary>Holds data and method accessible to all states</summary>
        private TMachine wrappedObject = default(TMachine);

        /// <summary>Tracks the if current state OnEntry has been called</summary>
        private bool isEntered = false;

        /// <summary>List that contains the state id chain built on construction</summary>
        List<int> idChain = new List<int>();

        /// <summary>List of transitions provoqued by incoming events directly</summary>
        private Dictionary<int, ISpStateTransition<TMsgId>> onEventTransitions = new Dictionary<int, ISpStateTransition<TMsgId>>();

        /// <summary>List of transitions provoqued by the results of state processing</summary>
        private Dictionary<int, ISpStateTransition<TMsgId>> onResultTransitions = new Dictionary<int, ISpStateTransition<TMsgId>>();

        /// <summary>state name without reference to ancestors</summary>
        private string name = "";

        /// <summary>Fully resolved state name</summary>
        private string fullName = "";

        /// <summary>Name cache for state int Id lookups</summary>
        private Dictionary<int,string> stateIdCache = new Dictionary<int, string>();

        /// <summary>Name cache for event int Id lookups</summary>
        private Dictionary<int,string> eventIdCache = new Dictionary<int, string>();

        /// <summary>Name cache for msg int Id lookups</summary>
        private Dictionary<int,string> msgIdCache = new Dictionary<int, string>();

        /// <summary>Factory to produce messages</summary>
        private ISpMsgFactory msgFactory = null;

        /// <summary>Convert the id integers to implementation level string equivalents</summary>
        private ISpIdConverter<TState,TMsgId,TMsgType> idConverter = null;

        private ClassLog log = new ClassLog("SpStateBase");

        #endregion

        #region Properties

        /// <summary>Returns object represented by the state machine</summary>
        public TMachine This {
            get {
                return this.wrappedObject;
            }
        }

        /// <summary>The message factory</summary>
        protected ISpMsgFactory MsgFactory {
            get {
                return this.msgFactory;
            }
        }

        #endregion

        #region ISpState Properties

        /// <summary>The unique integer state identifier</summary>
        public int Id {
            get {
                WrapErr.ChkTrue(this.idChain.Count > 0, 9999, "The state has no id");
                return this.IdChain[this.idChain.Count - 1];
            }
        }

        /// <summary>Get the full id by combining nested ids</summary>
        public List<int> IdChain {
            get {
                return this.idChain;
            }
        }

        /// <summary>This state object name with no reference to ancestors or children</summary>
        public string Name {
            get {
                return this.name;
            }
        }

        /// <summary>
        /// Resolved state name in format parent.parent.state with this state object
        /// being the leaf. No reference to any children in the chain
        /// </summary>
        public string FullName {
            get {
                return this.fullName;
            }
        }

        /// <summary>
        /// Fully resolved state name in format parent.parent.state.substate.substate with 
        /// all acestors and children until the farthest sub state being the leaf
        /// </summary>
        public virtual string CurrentStateName {
            get {
                return this.fullName;
            }
        }

        #endregion

        #region SpState Protected Properties

        /// <summary>
        /// Queries if OnEntry has already been invoked. It can only
        /// be invoked once until the OnExit is called
        /// </summary>
        protected bool IsEntryExcecuted {
            get {
                return this.isEntered;
            }
        }

        #endregion

        #region Constructors

        /// <summary>Default constructor in private scope to prevent usage</summary>
        private SpStateBase() {
        }


        /// <summary>Constructor for first level state</summary>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="idConverter">The integer id to string converter</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpStateBase(ISpMsgFactory msgFactory, TState id, TMachine wrappedObject)
            : this(null, msgFactory, id, wrappedObject) {
        }


        /// <summary>Constructor</summary>
        /// <param name="parent">The parent state</param>
        /// <param name="msgFactory">Message Factory</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpStateBase(ISpState<TMsgId> parent, ISpMsgFactory msgFactory, TState id, TMachine wrappedObject) {
            WrapErr.ChkParam(msgFactory, "msgFactory", 9999);
            WrapErr.ChkParam(wrappedObject, "wrappedObject", 50200);
            this.msgFactory = msgFactory;
            this.idConverter = new SpIdConverter<TState,TMsgId,TMsgType>();
            this.InitStateIds(parent, id);
            this.wrappedObject = wrappedObject;
        }

        #endregion

        #region ISpState Methods

        #region Virtual

        /// <summary>Excecuted once when the state becomes the current state/// </summary>
        /// <remarks>Only override in super state</remarks>        
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition<TMsgId> OnEntry(ISpEventMessage msg) {
            this.log.Info("OnEntry", String.Format("'{0}' State {1} - Event", this.FullName, this.GetCachedEventId(msg.EventId)));
            WrapErr.ChkFalse(this.IsEntryExcecuted, 50201, "OnEntry Cannot be Executed More Than Once Until OnExit is Called");
            return WrapErr.ToErrorReportException(9999, () => {
                return this.GetTransition(true, this.ExecOnEntry, msg);
            });
        }


        /// <summary>Called on every other period after entry</summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition<TMsgId> OnTick(ISpEventMessage msg) {
            //this.log.Info("OnTick", String.Format("'{0}' State - {1}", this.FullName, this.ConvertEventIdToString(msg.EventId)));
            WrapErr.ChkTrue(this.IsEntryExcecuted, 50205, () => {
                return String.Format("OnTick for '{0}' State Cannot be Executed Before OnEntry", this.FullName);
            });
            return WrapErr.ToErrorReportException(9999, () => {
                return this.GetTransition(false, this.ExecOnTick, msg);
            });
        }

        #endregion

        #region Closed

        /// <summary>Always invoked on object exit        /// </summary>
        public void OnExit() {
            this.log.Info("OnExit", String.Format("'{0}' State", this.FullName));
            // TODO - check that OnEntry has happened ??
            WrapErr.ToErrorReportException(9999, () => {
                // Only execute ExceOnExit code if the state had been entered
                if (this.IsEntryExcecuted) {
                    this.ExecOnExit();
                }
                else {
                    this.log.Warning(9999, String.Format(
                        "ExecOnExit for State:{0} not called because OnEntry was preempted by an OnEvent Transition",
                        this.FullName));
                }
            },
            () => { this.isEntered = false; });
        }

        #region Transition registration methods for methods on receiving an event

        /// <summary>Register a state transition from incoming event</summary>
        /// <param name="eventId">The event id</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnEventTransition(TMsgId eventId, ISpStateTransition<TMsgId> transition) {
            SpTools.RegisterTransition("OnEvent", eventId, transition, this.onEventTransitions);
        }


        /// <summary>Register transition to next state on event</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        public void ToNextOnEvent(TMsgId ev, ISpState<TMsgId> newState) {
            this.RegisterOnEventTransition(ev, SpStateTransition<TMsgId>.ToNext(newState));
        }


        /// <summary>Register transition to next state on event</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        /// <param name="returnMsg">The message to return on transition</param>
        public void ToNextOnEvent(TMsgId ev, ISpState<TMsgId> newState, ISpEventMessage returnMsg) {
            this.RegisterOnEventTransition(ev, SpStateTransition<TMsgId>.ToNext(newState, returnMsg));
        }


        /// <summary>Register exit transition on event</summary>
        /// <param name="ev">The event</param>
        public void ToExitOnEvent(TMsgId ev) {
            this.RegisterOnEventTransition(ev, SpStateTransition<TMsgId>.ToExit());
        }


        /// <summary>Register defered action on event</summary>
        /// <param name="ev">The event</param>
        public void ToDeferedOnEvent(TMsgId ev) {
            this.RegisterOnEventTransition(ev, SpStateTransition<TMsgId>.ToDefered());
        }

        #endregion

        #region Transition registration methods for methods after internal processing

        /// <summary>Register state transition from the result of state processing</summary>
        /// <param name="responseId">The result id the state returns as the result of processing</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnResultTransition(TMsgId responseId, ISpStateTransition<TMsgId> transition) {
            SpTools.RegisterTransition("OnResult", responseId, transition, this.onResultTransitions);
        }


        /// <summary>Register transition to next state on processing results</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        public void ToNextOnResult(TMsgId ev, ISpState<TMsgId> newState) {
            this.RegisterOnResultTransition(ev, SpStateTransition<TMsgId>.ToNext(newState));
        }


        /// <summary>Register transition to next state on processing results</summary>
        /// <param name="ev">The event</param>
        /// <param name="newState">The target state for transition</param>
        /// <param name="returnMsg">The message to return on transition</param>
        public void ToNextOnResult(TMsgId ev, ISpState<TMsgId> newState, ISpEventMessage returnMsg) {
            this.RegisterOnResultTransition(ev, SpStateTransition<TMsgId>.ToNext(newState, returnMsg));
        }


        /// <summary>Register exit transition on result</summary>
        /// <param name="ev">The returned result</param>
        public void ToExitOnResult(TMsgId ev) {
            this.RegisterOnResultTransition(ev, SpStateTransition<TMsgId>.ToExit());
        }

        public void DebugDumpTransitions() {
            this.log.Warning(0, string.Format("----- Result Transitions ({0}) -----", this.onResultTransitions.Keys.Count));

            foreach (var k in this.onResultTransitions.Keys) {
                this.log.Warning(0, string.Format(
                    "     ID: {0} Type: {1} Next state: {2}",
                    k,
                    this.onResultTransitions[k].TransitionType,
                    this.onResultTransitions[k].NextState));
            }

            this.log.Warning(0, string.Format("----- Event Transitions ({0}) -----", this.onEventTransitions.Keys.Count));
            foreach (var k in this.onEventTransitions.Keys) {
                this.log.Warning(0, string.Format(
                    "     ID: {0} Type: {1} Next state: {2}",
                    k,
                    this.onEventTransitions[k].TransitionType,
                    this.onEventTransitions[k].NextState));
            }
        }

        #endregion

        #endregion

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Virtual method invoked on entry. If not overriden it will return 
        /// the default return event message
        /// </summary>
        /// <param name="eventMsg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpEventMessage ExecOnEntry(ISpEventMessage eventMsg) {
            return this.MsgFactory.GetDefaultResponse(eventMsg);
        }


        /// <summary>
        /// Virtual method invoked on every tick after. If not overriden it 
        /// will return the default return event message
        /// </summary>
        /// <param name="eventMsg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
            return this.MsgFactory.GetDefaultResponse(eventMsg);
        }


        /// <summary>
        /// Virtual method invoked on exit from state.
        /// </summary>
        protected virtual void ExecOnExit() {
        }

        #endregion

        #region Protected Methods

        /// <summary>Wrapper to retrieve OnEvent Transition Object</summary>
        /// <param name="eventMsg">The incomming event message</param>
        /// <returns>The transition if present, otherwise null</returns>
        protected ISpStateTransition<TMsgId> GetOnEventTransition(ISpEventMessage eventMsg) {
            return this.GetTransitionFromStore(this.onEventTransitions, eventMsg);
        }


        /// <summary>Wrapper to retrieve OnResult Transition Object</summary>
        /// <param name="eventMsg">The incomming event message</param>
        /// <returns>The transition if present, otherwise null</returns>
        protected ISpStateTransition<TMsgId> GetOnResultTransition(ISpEventMessage eventMsg) {
            return this.GetTransitionFromStore(this.onResultTransitions, eventMsg);
        }

        #region Cached id names

        /// <summary>
        /// Allows derived classes to convert the type to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString() so you
        /// would end up with a name chaine some like '2.4.12'
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected string GetCachedStateId(int id) {
            return SpTools.GetIdString(id, this.stateIdCache, this.idConverter.StateId);
        }

        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected string GetCachedEventId(int id) {
            return SpTools.GetIdString(id, this.eventIdCache, this.idConverter.EventId);
        }


        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected string GetCachedMsgTypeId(int id) {
            return SpTools.GetIdString(id, this.msgIdCache, this.idConverter.MsgTypeId);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Verifies transitions in order of onEvent, onResult and if not found will return the 
        /// default transition
        /// </summary>
        /// <param name="stateFunc">
        /// The function that is invoked that will return return message from the state processing
        /// and use it to check against the onResult queue.
        /// </param>
        /// <param name="eventMsg">The incoming message to validate against the onEvent list</param>
        /// <returns>The OnEvent, OnResult or default transition</returns>
        private ISpStateTransition<TMsgId> GetTransition(bool onEntry, Func<ISpEventMessage, ISpEventMessage> stateFunc, ISpEventMessage msg) {
            return WrapErr.ToErrorReportException(9999, () => {

                //this.log.Warning(88, "||||||| State get result transition");

                // Query the OnEvent queue for a transition BEFORE calling state function (OnEntry, OnTick)
                ISpStateTransition<TMsgId> tr = this.GetOnEventTransition(msg);
                if (tr != null) {
                    tr.ReturnMessage = (tr.ReturnMessage == null) 
                        ? this.MsgFactory.GetResponse(msg) 
                        : this.MsgFactory.GetResponse(tr.ReturnMessage);
                    return tr;
                }

                // Only considered entered if you do not encounter a preemptive OnEvent transition on entry. In this way
                // you could get a situation where you cascade down several state depths based on higher up events
                if (onEntry) {
                    this.isEntered = true;
                }

                // Get the transition object from the 'OnResult' queue
                if ((tr = this.GetOnResultTransition(stateFunc.Invoke(msg))) != null) {
                    tr.ReturnMessage = this.MsgFactory.GetResponse(msg, tr.ReturnMessage);
                    this.log.Info("GetTransition", () => string.Format(
                        "Got result transition type {0} from factory with return code {1}", tr.TransitionType, tr.ReturnMessage.ReturnCode));
                    return tr;
                }

                // If no transitions registered return SameState with default success message
                return new SpStateTransition<TMsgId>(SpStateTransitionType.SameState, null, this.MsgFactory.GetDefaultResponse(msg));
            });
        }


        /// <summary>Get the transition object from the store or null if not found</summary>
        /// <param name="store">The store to search</param>
        /// <param name="eventMsg">The message to insert in the transition object</param>
        /// <returns>The transition object from the store or null if not found</returns>
        private ISpStateTransition<TMsgId> GetTransitionFromStore(Dictionary<int, ISpStateTransition<TMsgId>> store, ISpEventMessage eventMsg) {
            return WrapErr.ToErrorReportException(50204, () => {
                ISpStateTransition<TMsgId> t = SpTools.GetTransitionCloneFromStore(store, eventMsg);
                if (t != null) {
                    this.LogTransition(t, eventMsg);
                }
                return t;
            });
        }


        /// <summary>Factor out the reporting of state transitions for clarity</summary>
        /// <param name="tr">The transition object</param>
        /// <param name="eventMsg">The event message which pushed this transition</param>
        private void LogTransition(ISpStateTransition<TMsgId> tr, ISpEventMessage eventMsg) {
            WrapErr.ToErrorReportException(9999, () => {
                this.log.Debug("LogTransition",
                    String.Format(
                        "{0} OnMsg({1} - {2}) - From:{3} To:{4}",
                        tr.TransitionType,
                        this.GetCachedMsgTypeId(eventMsg.TypeId),
                        this.GetCachedEventId(eventMsg.EventId),
                        this.FullName,
                        tr.NextState == null ? "Unknown(null)" : tr.NextState.FullName));
            });
        }


        /// <summary>Initialise the state id chain from ancestors to this state</summary>
        /// <param name="parent">The parent state</param>
        /// <param name="id">The enum state id</param>
        private void InitStateIds(ISpState<TMsgId> parent, TState id) {
            // Add any ancestor state ids to the chain
            WrapErr.ToErrorReportException(50207, () => {
                WrapErr.ChkTrue(typeof(TState).IsEnum, 9999, () => string.Format("State type {0} must be Enum", id.GetType().Name));
                WrapErr.ChkTrue(typeof(TState).GetEnumUnderlyingType() == typeof(int), 9999,
                    () => string.Format("State type enum {0} must be derived from int", id.GetType().Name));
                int tmp = Convert.ToInt32(id);

                if (parent != null) {
                    WrapErr.ChkVar(parent.IdChain, 50206, "The Parent has a Null Id Chain");
                    this.idChain.Clear();
                    parent.IdChain.ForEach((item) => this.idChain.Add(item));
                }
                // This state id is the leaf
                this.idChain.Add(tmp);
                this.BuildName();
            });
        }

        /// <summary>Builds fully resolved name by iterating through names based on Ids</summary>
        private void BuildName() {
            WrapErr.ToErrorReportException(9999, () => {
                StringBuilder sb = new StringBuilder(75);
                this.IdChain.ForEach((item) => {
                    sb.Append(String.Format(".{0}", this.GetCachedStateId(item)));
                });
                this.fullName = sb.Length > 0 ? sb.ToString(1, sb.Length - 1) : "FullNameSearchFailed";
                this.name = this.idChain.Count > 0 ? this.GetCachedStateId(this.idChain[this.idChain.Count - 1]) : "NameSearchFailed";
            });
        }

        #endregion

    }
}
