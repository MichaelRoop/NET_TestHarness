using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;
using LogUtils;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;

namespace SpStateMachine.States {

    /// <summary>
    /// Base implementation of the ISpState interface
    /// </summary>
    /// <typeparam name="T">Generic type that the state represents</typeparam>
    /// <author>Michael Roop</author>
    public abstract class SpState<T> : ISpState where T : class {

        #region Data

        /// <summary>Holds data and method accessible to all states</summary>
        private T wrappedObject = default(T);

        /// <summary>Tracks the current state of the state</summary>
        private bool isEntered = false;

        /// <summary>List that contains the state id chain build on construction</summary>
        List<int> idChain = new List<int>();

        /// <summary>
        /// List of transitions that are provoqued by incoming events directly
        /// </summary>
        private Dictionary<int, ISpStateTransition> onEventTransitions = new Dictionary<int, ISpStateTransition>();

        /// <summary>
        /// List of transitions that are provoqued by the results of state processing
        /// </summary>
        private Dictionary<int, ISpStateTransition> onResultTransitions = new Dictionary<int, ISpStateTransition>();

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

        private readonly string className = "SpState";

        #endregion

        #region Properties

        /// <summary>
        /// Returns the object which is represented by the state machine
        /// </summary>
        public T This {
            get {
                return this.wrappedObject;
            }
        }

        #endregion

        #region ISpState Properties

        /// <summary>
        /// Queries if OnEntry has already been invoked. It can only
        /// be invoked once until the OnExit is called
        /// </summary>
        public bool IsEntryExcecuted {
            get {
                return this.isEntered;
            }
        }

        /// <summary>
        /// The unique state identifier
        /// </summary>
        public int Id {
            get {
                WrapErr.ChkTrue(this.idChain.Count > 0, 9999, "The state has no id");
                return this.IdChain[this.idChain.Count - 1];
            }
        }

        /// <summary>
        /// Get the full id by combining nested ids
        /// </summary>
        public List<int> IdChain {
            get {
                return this.idChain;
            }
        }


        /// <summary>
        /// The state name without reference to ancestors (i.e. parent.state)
        /// </summary>
        public string Name {
            get {
                return this.name;
            }
        }


        /// <summary>
        /// From Get the fully resolved state name in format
        /// grandparent.parent.state
        /// </summary>
        public virtual string FullName {
            get {
                return this.fullName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpState() {
        }


        /// <summary>
        /// Constructor for first level state
        /// </summary>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpToInt id, T wrappedObject)
            : this(null, id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="id">Unique state id converter</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpState parent, ISpToInt id, T wrappedObject) {
            WrapErr.ChkParam(wrappedObject, "wrappedObject", 50200);
            this.InitStateIds(parent, id.ToInt());
            this.wrappedObject = wrappedObject;
        }

        #endregion

        #region ISpState Methods

        #region Virtual 

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition OnEntry(ISpEventMessage msg) {
            Log.Info(this.className, "ExecOnEntry", this.FullName);
            WrapErr.ChkFalse(this.IsEntryExcecuted, 50201, "OnEntry Cannot be Executed More Than Once Until OnExit is Called");
            this.SetEntered(true);
            return WrapErr.ToErrorReportException(9999, () => {
                return this.GetTransitionInOrder(this.ExecOnEntry, msg);
            });
        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition OnTick(ISpEventMessage msg) {
            //Log.Info(this.className, "ExecOnTick", this.FullName);
            WrapErr.ChkTrue(this.IsEntryExcecuted, 50205, "OnTick Cannot be Executed Before OnEntry");
            return WrapErr.ToErrorReportException(9999, () => {
                return this.GetTransitionInOrder(this.ExecOnTick, msg);
            });
        }


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        public virtual void OnExit() {
            Log.Info(this.className, "ExecOnExit", this.FullName);
            // TODO - check that OnEntry has happened ??
            this.SetEntered(false);
            WrapErr.ToErrorReportException(9999, () => {
                this.ExecOnExit();
            });
        }

        #endregion

        #region Sealed Method

        /// <summary>
        /// Register a state transition from incoming event. Left virtual to allow a derived class
        /// to create a blocking scenario if they want to use enums for the ids (see examples)
        /// </summary>
        /// <param name="eventId">The id converter of the incoming event</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnEventTransition(ISpToInt eventId, ISpStateTransition transition) {
            SpTools.RegisterTransition("OnEvent", eventId, transition, this.onEventTransitions); 
        }
        

        /// <summary>
        /// Register a state transition from the result of state processing. Left virtual to allow a 
        /// derived class to create a blocking scenario if they want to use enums for the ids 
        /// (see examples)
        /// </summary>
        /// <param name="eventId">The id converter of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnResultTransition(ISpToInt eventId, ISpStateTransition transition) {
            SpTools.RegisterTransition("OnResult", eventId, transition, this.onResultTransitions); 
        }

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
            return GetDefaultReturnMsg(eventMsg);
        }


        /// <summary>
        /// Virtual method invoked on every tick after. If not overriden it 
        /// will return the default return event message
        /// </summary>
        /// <param name="eventMsg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
            return GetDefaultReturnMsg(eventMsg);
        }


        /// <summary>
        /// Virtual method invoked on exit from state.
        /// </summary>
        protected virtual void ExecOnExit() {
        }


        /// <summary>
        /// Returns a default transition object set to no transition, no next 
        /// state and the default return message
        /// </summary>
        /// <param name="eventMsg">The incoming message</param>
        /// <returns>The default transition with no transition set</returns>
        protected virtual ISpStateTransition GetDefaultTransition(ISpEventMessage eventMsg) {
            return new SpStateTransition(SpStateTransitionType.SameState, null, this.GetDefaultReturnMsg(eventMsg));
        }
        
        #endregion

        #region Abstract Properties and Methods
        
        /// <summary>
        /// Provides the default return msg
        /// </summary>
        /// <param name="msg">The incomming message</param>
        protected abstract ISpEventMessage GetDefaultReturnMsg(ISpEventMessage msg);


        /// <summary>
        /// Get the appropriate paired return message for the message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected abstract ISpEventMessage GetReponseMsg(ISpEventMessage msg);


        /// <summary>
        /// Allows derived classes to convert the type to string if they are using strongly 
        /// typed convetible enums. you can also just decide to use int.ToString you could
        /// end up with a name chaine some like '2.4.12'.
        /// *NOTE*: Although the derived class can call this directly it should rather call the 
        /// GetCachedStateId in order to maximize efficiency. The cached call will only
        /// ever call this conversion method once per unique id
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected abstract string ConvertStateIdToString(int id);


        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. It will also make the logs more readeable. 
        /// *NOTE*: Although the derived class can call this directly it should rather call 
        /// the GetCachedEventId in order to maximize efficiency. The cached call will only
        /// ever call this conversion method once per unique id  
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected abstract string ConvertEventIdToString(int id);


        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. It will also make the logs more readeable. 
        /// *NOTE*: Although the derived class can call this directly it should rather call 
        /// the GetCachedMsgTypeId in order to maximize efficiency. The cached call will only
        /// ever call this conversion method once per unique id
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected abstract string ConvertMsgTypeIdToString(int id);
        
        #endregion

        #region Protected Methods

        /// <summary>
        /// Set the entered flag
        /// </summary>
        /// <param name="isEntered"></param>
        protected void SetEntered(bool isEntered) {
            this.isEntered = isEntered;
        }


        /// <summary>
        /// Wrapper to retrieve OnEvent Transition Object
        /// </summary>
        /// <param name="eventMsg">The incomming event message</param>
        /// <returns>The transition if present, otherwise null</returns>
        protected ISpStateTransition GetTransitionFromOnEventRegistrations(ISpEventMessage eventMsg) {
            return this.GetTransitionFromStore(this.onEventTransitions, eventMsg);
        }


        /// <summary>
        /// Wrapper to retrieve OnResult Transition Object
        /// </summary>
        /// <param name="eventMsg">The incomming event message</param>
        /// <returns>The transition if present, otherwise null</returns>
        protected ISpStateTransition GetTransitionFromOnResultRegistrations(ISpEventMessage eventMsg) {
            return this.GetTransitionFromStore(this.onResultTransitions, eventMsg);
        }
        

        /// <summary>
        /// Wraps the call to GetResponseMsg with some error handling
        /// </summary>
        /// <param name="eventMsg">The return message from the derived class</param>
        protected ISpEventMessage OnGetResponseMsg(ISpEventMessage eventMsg) {
            WrapErr.ChkParam(eventMsg, "eventMsg", 9999);
            ISpEventMessage ret = this.GetReponseMsg(eventMsg);
            WrapErr.ChkVar(ret, 9999, "The call to overriden 'GetReponseMsg' returned a null event message");
            return ret;
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
            return SpTools.GetIdString(id, this.stateIdCache, this.ConvertStateIdToString);
        }

        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected string GetCachedEventId(int id) {
            return SpTools.GetIdString(id, this.eventIdCache, this.ConvertEventIdToString);
        }


        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected string GetCachedMsgTypeId(int id) {
            return SpTools.GetIdString(id, this.msgIdCache, this.ConvertMsgTypeIdToString);
        }
        
        #endregion
        
        /// <summary>
        /// Factor out the reporting of state transitions for clarity
        /// </summary>
        /// <param name="tr">The transition object</param>
        /// <param name="eventMsg">The event message which pushed this transition</param>
        protected void LogTransition(ISpStateTransition tr, ISpEventMessage eventMsg) {
            WrapErr.ToErrorReportException(9999, () => {
                Log.Debug("SpState", "LogTransition",
                    String.Format(
                        "Transition Type:{0} From:{1} To:{2} On Msg:{3} Event:{4}",
                        tr.TransitionType,
                        this.FullName,
                        tr.NextState == null ? "Unknown" : tr.NextState.FullName,
                        this.GetCachedMsgTypeId(eventMsg.TypeId),
                        this.GetCachedEventId(eventMsg.EventId)));
            });
        }
        
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
        private ISpStateTransition GetTransitionInOrder(Func<ISpEventMessage, ISpEventMessage> stateFunc, ISpEventMessage eventMsg) {
            return WrapErr.ToErrorReportException(9999, () => {
                // Query the OnEvent queue for a transition BEFORE calling state function (OnEntry, OnTick)
                ISpStateTransition tr = this.GetTransitionFromOnEventRegistrations(eventMsg);
                if (tr != null) {
                    // Call to derived class to get the return message related to the incoming message
                    tr.ReturnMessage = this.OnGetResponseMsg(this.GetReponseMsg(eventMsg));
                    return tr;
                }

                // TODO - clarify this - we use the event id after processing so the return message is already selected
                // Think that the derived should just send the same Msg or another msg back so that we can get the response msg from the same call as above
                if ((tr = this.GetTransitionFromOnResultRegistrations(stateFunc.Invoke(eventMsg))) != null) {
                    return tr;
                }
                return this.GetDefaultTransition(eventMsg);
            });
        }


        /// <summary>
        /// Get the transition object from the store or null if not found
        /// </summary>
        /// <param name="store">The store to search</param>
        /// <param name="eventMsg">The message to insert in the transition object</param>
        /// <returns>The transition object from the store or null if not found</returns>
        private ISpStateTransition GetTransitionFromStore(Dictionary<int, ISpStateTransition> store, ISpEventMessage eventMsg) {
            return WrapErr.ToErrorReportException(50204, () => {
                if (store.Keys.Contains(eventMsg.EventId)) {
                    Log.Debug("SpState", "GetTransition", String.Format("Found transition for event:{0}", this.GetCachedEventId(eventMsg.EventId)));

                    // Clone Transition object from store since its pointers get reset later
                    ISpStateTransition tr = (ISpStateTransition)store[eventMsg.EventId].Clone();
                    this.LogTransition(tr, eventMsg);
                    tr.ReturnMessage = eventMsg;
                    return tr;
                }
                return null;
            });
        }


        /// <summary>
        /// Initialise the state id chain from ancestors to this state 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        private void InitStateIds(ISpState parent, int id) {
            // Add any ancestor state ids to the chain
            WrapErr.ToErrorReportException(50207, () => {
                if (parent != null) {
                    WrapErr.ChkVar(parent.IdChain, 50206, "The Parent has a Null Id Chain");
                    this.idChain.Clear();
                    parent.IdChain.ForEach((item) => this.idChain.Add(item));
                }
                // This state id is the leaf
                this.idChain.Add(id);
                this.BuildName();
            });
        }


        /// <summary>
        /// Builds the fully resolved name by iterating through the  the name based on the 
        /// </summary>
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
