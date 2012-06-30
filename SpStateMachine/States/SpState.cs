using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using ChkUtils;
using LogUtils;

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
        public SpState(int id, T wrappedObject)
            : this(null, id, wrappedObject) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent state</param>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(ISpState parent, int id, T wrappedObject) {
            WrapErr.ChkParam(wrappedObject, "wrappedObject", 9999);
            this.InitStateIds(parent, id);
            this.wrappedObject = wrappedObject;
        }

        #endregion

        #region ISpState Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition OnEntry(ISpMessage msg) {
            WrapErr.ChkFalse(this.IsEntryExcecuted, 9999, "OnEntry Cannot be Executed More Than Once Until OnExit is Called");
            this.SetEntered(true);
            return this.GetTransition(msg, this.ExecOnEntry);
        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public virtual ISpStateTransition OnTick(ISpMessage msg) {
            return this.GetTransition(msg, this.ExecOnTick);
        }


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        public void OnExit() {
            this.SetEntered(false);
            this.ExecOnExit();
        }


        /// <summary>
        /// Register a state transition from incoming event
        /// </summary>
        /// <param name="eventId">The id of the incoming event</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnEventTransition(int eventId, ISpStateTransition transition) {
            this.ValidateNotFound(eventId, transition);
            this.onEventTransitions.Add(eventId, transition);
        }
        

        /// <summary>
        /// Register a state transition from the result of state processing
        /// </summary>
        /// <param name="eventId">The id of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnResultTransition(int eventId, ISpStateTransition transition) {
            this.ValidateNotFound(eventId, transition);
            this.onResultTransitions.Add(eventId, transition);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Virtual method invoked on entry. If not overriden it will return 
        /// the default return object
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpMessage ExecOnEntry(ISpMessage msg) {
            return GetDefaultReturnMsg(msg);
        }


        /// <summary>
        /// Virtual method invoked on every tick after. If not overriden it 
        /// will return the default return object
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpMessage ExecOnTick(ISpMessage msg) {
            return GetDefaultReturnMsg(msg);
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
        /// <param name="msg">The incoming message</param>
        /// <returns>The default transition with no transition set</returns>
        protected virtual ISpStateTransition GetDefaultTransition(ISpMessage msg) {
            return new SpStateTransition(SpStateTransitionType.SameState, null, this.GetDefaultReturnMsg(msg));
        }


        /// <summary>
        /// The user can override this method on a state by state basis to stuff
        /// data into the return msg class if necessary. By default it is a pass through
        /// </summary>
        /// <param name="msg">The return message from the derived class</param>
        protected virtual ISpMessage OnGetResponseMsg(ISpMessage msg) {
            return msg;
        }
        
        #endregion

        #region abstract Properties and Methods

        /// <summary>
        /// From ISpState Get the fully resolved state name in format
        /// grandparent.parent.state
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Provides the default return msg
        /// </summary>
        /// <param name="msg">The incomming message</param>
        protected abstract ISpMessage GetDefaultReturnMsg(ISpMessage msg);


        /// <summary>
        /// Get the appropriate paired return message for the message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected abstract ISpMessage GetReponseMsg(ISpMessage msg);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Set the entered flag
        /// </summary>
        /// <param name="isEntered"></param>
        protected void SetEntered(bool isEntered) {
            this.isEntered = isEntered;
        }


        protected ISpStateTransition GetOnEventTransition(ISpMessage msg) {
            return this.GetTransition(this.onEventTransitions, msg);
        }

        protected ISpStateTransition GetOnResultTransition(ISpMessage msg) {
            return this.GetTransition(this.onResultTransitions, msg);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Validate against duplicate transitions by id both within a container and between containers
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="transition"></param>
        private void ValidateNotFound(int eventId, ISpStateTransition transition) {
            WrapErr.ChkFalse(this.onEventTransitions.Keys.Contains(eventId), 9999, () => {
                return String.Format("OnEvent Already Contains Transition for Id:{0}", eventId);
            });
            WrapErr.ChkFalse(this.onResultTransitions.Keys.Contains(eventId), 9999, () => {
                return String.Format("OnResult Already Contains Transition for Id:{0}", eventId);
            });
        }


        /// <summary>
        /// Verifies transitions in order of onEvent, onResult and if not found will return the 
        /// default transition
        /// </summary>
        /// <param name="msg">The incoming message to validate against the onEvent list</param>
        /// <param name="func">
        /// The function that is invoked that will return return message from the state processing
        /// and use it to check against the onResult queue.
        /// </param>
        /// <returns>The OnEvent, OnResult or default transition</returns>
        private ISpStateTransition GetTransition(ISpMessage msg, Func<ISpMessage, ISpMessage> func) {

            // We used the event id in the incoming message against the OnEvent list
            ISpStateTransition tr = this.GetOnEventTransition(msg); // this.GetTransition(this.onEventTransitions, msg);
            if (tr != null) {
                // Call to derived class to replace the incoming message with its paired return message
                // The user can override the OnGetResponse on a state by state basis to stuff data in
                // particular return message types
                tr.ReturnMessage = this.OnGetResponseMsg(this.GetReponseMsg(msg));
                return tr;
            }

            // we use the event id after processing so the return message is already selected
            //if ((tr = this.GetTransition(this.onResultTransitions, func.Invoke(msg))) != null) {
            if ((tr = this.GetOnResultTransition(func.Invoke(msg))) != null) {
                return tr;
            }
            return this.GetDefaultTransition(msg);
        }


        /// <summary>
        /// Get the transition object from the store or null if not found
        /// </summary>
        /// <param name="store">The store to search</param>
        /// <param name="msg">The message to insert in the transition object</param>
        /// <returns>The transition object from the store or null if not found</returns>
        private ISpStateTransition GetTransition(Dictionary<int, ISpStateTransition> store, ISpMessage msg) {
            if (store.Keys.Contains(msg.EventId)) {
                Log.Debug("SpState", "GetTransition", String.Format("Found transition for event:{0}", msg.EventId));
                ISpStateTransition tr = store[msg.EventId];
                Log.Debug("SpState", "GetTransition",
                    String.Format(
                        "Type:{0} From:{1} To:{2} MsgType:{3} MsgEventId:{4}",
                        tr.TransitionType,
                        this.Name,
                        tr.NextState == null ? "Null" : tr.NextState.Name,
                        tr.ReturnMessage == null ? "Null" : tr.ReturnMessage.TypeId.ToString(),
                        tr.ReturnMessage == null ? "Null" : tr.ReturnMessage.EventId.ToString()));
                tr.ReturnMessage = msg;
                return tr;
            }
            return null;
        }


        /// <summary>
        /// Initialise the state id chain from ancestors to this state 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        private void InitStateIds(ISpState parent, int id) {
            // Add any ancestor state ids to the chain
            if (parent != null) {
                WrapErr.ChkVar(parent.IdChain, 9999, "The parent has a null id chain");
                this.idChain.Clear();
                parent.IdChain.ForEach((item) => this.idChain.Add(item));
            }
            // This state id is the leaf
            this.idChain.Add(id);
        }

        #endregion
        
    }
}
