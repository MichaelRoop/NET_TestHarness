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
    public abstract class SpState<T> : ISpState {

        #region Internal Classes

        //class TransitionInfo {
        //    SpStateTransitionType type = SpStateTransitionType.OnEvent;
        //    ISpStateTransition transition = null;
        //    int responseId = 0;
        //}
        
        #endregion

        #region Data

        /// <summary>
        /// Holds data and method accessible to all states
        /// </summary>
        private T wrappedObject = default(T);

        /// <summary>
        /// The default response if there are no responses registered
        /// </summary>
  //      private ISpMessage defaultResonse = null;

        /// <summary>
        /// Tracks the current state of the state
        /// </summary>
        private bool isEntered = false;
        
        int id = 0;

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
        /// Returns the core object which is state wrapped by the state machine
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
                return this.id;
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
            this.id = id;

            // The sum of the id chain is all ancestors then this state id as leaf
            if (parent != null) {
                this.idChain = parent.IdChain;
            }
            this.idChain.Add(this.Id);

            this.wrappedObject = wrappedObject;
        }

        #endregion

        #region ISpState Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public ISpStateTransition OnEntry(ISpMessage msg) {
            WrapErr.ChkFalse(this.IsEntryExcecuted, 9999, "OnEntry Cannot be Executed More Than Once Until OnExit is Called");
            this.isEntered = true;
            return this.GetTransition(msg, this.ExecOnEntry);

        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public ISpStateTransition OnTick(ISpMessage msg) {
            return this.GetTransition(msg, this.ExecOnTick);
        }


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        public void OnExit() {
            this.isEntered = false;
            this.ExecOnExit();
        }


        /// <summary>
        /// Register a state transition from incoming event
        /// </summary>
        /// <param name="eventId">The id of the incoming event</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnEventTransition(int eventId, ISpStateTransition transition) {
            this.ValidateNotFound(eventId, transition);

            // This one will respond to the incoming id and will require a corresponding response msg from factory
            this.onEventTransitions.Add(eventId, transition);
        }
        

        /// <summary>
        /// Register a state transition from the result of state processing
        /// </summary>
        /// <param name="responseId">The id of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        public void RegisterOnResultTransition(int responseId, ISpStateTransition transition) {
            this.ValidateNotFound(responseId, transition);

            // This one will respond to the result id and will require a corresponding response msg from factory
            this.onResultTransitions.Add(responseId, transition);
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
            // If no override it will query to get default return
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


        /// <summary>
        /// Virtual method invoked on every tick after. If not overriden it 
        /// will return the default return object
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpMessage ExecOnTick(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


        /// <summary>
        /// Virtual method invoked on exit from state.
        /// </summary>
        protected virtual void ExecOnExit() {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual ISpStateTransition GetDefaultTransition(ISpMessage msg) {
            return new SpStateTransition(false, null, this.GetDefaultReturnMsg(msg));
        }


        //protected virtual ISpStateTransition GetResultTransition(ISpMessage msg) {
        //    // always check
            
        //    //ISpStateTransition st = 


        //    // for now just get default
        //    return this.GetDefaultTransition(msg);

        //    //return new SpStateTransition(false, null, this.GetDefaultReturnMsg(msg));
        //}

        #endregion



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


        private ISpStateTransition GetTransition(ISpMessage msg, Func<ISpMessage, ISpMessage> func) {
            // Check event driven transitions before calling into the func
            //if (this.onEventTransitions.Keys.Contains(msg.EventId)) {
            //    ISpStateTransition tr = this.onEventTransitions[msg.EventId];
            //    // Runtime copy of response message into transition object
            //    tr.ReturnMessage = this.GetReponseMsg(msg.EventId, msg);
            //    return tr;
            //}

            //// pass the message to the state for processing and retrieve the returned message.
            //ISpMessage response = func.Invoke(msg);
            //if (this.onResultTransitions.Keys.Contains(response.EventId)) {
            //    ISpStateTransition tr = this.onResultTransitions[msg.EventId];
            //    tr.ReturnMessage = response;
            //    return tr;
            //}

            //// No specialised behavior registered so return default
            //return this.GetDefaultTransition(msg);


            ISpStateTransition st = null;
            if ((st = this.GetTransition(this.onEventTransitions, msg)) == null) {
                if ((st = this.GetTransition(this.onResultTransitions, func.Invoke(msg))) == null) {
                    st = this.GetDefaultTransition(msg);
                }
            }

            //if (st.HasTransition) {
            //    Log.Debug("SpState", "GetTransition", "**************** Has transition *******************");
            //}

            //Log.Debug("SpState", "GetTransition", 
            //    String.Format(
            //        "HasTransition:{0} to state:{1} with Return Msg Event id:{2}" , 
            //        st.HasTransition, 
            //        st.NextState == null ? "null" : st.NextState.Name,
            //        st.ReturnMessage == null ? "null" : st.ReturnMessage.EventId.ToString()));
            return st;
        }


        ISpStateTransition GetTransition(Dictionary<int, ISpStateTransition> store, ISpMessage msg) {
            //Log.Debug("SpState", "GetTransition $$$$$$$$$  ", String.Format("$$$ - incoming event:{0}", msg.EventId));
            if (store.Keys.Contains(msg.EventId)) {
                Log.Debug("SpState", "GetTransition", String.Format("Found transition for event:{0}", msg.EventId));
                ISpStateTransition tr = store[msg.EventId];

                Log.Debug("SpState", "GetTransition",
                    String.Format(
                        "** Found transition object. HasTransition:{0} to state:{1} with Return Msg Event id:{2}",
                        tr.HasTransition,
                        tr.NextState == null ? "null" : tr.NextState.Name,
                        tr.ReturnMessage == null ? "null" : tr.ReturnMessage.EventId.ToString()));

                tr.ReturnMessage = msg;
                return tr;
            }
            else {
                //Log.Debug("SpState", "GetTransition", String.Format("### - Nothing found for incoming event:{0}", msg.EventId));
            }
            return null;
        }



        #region abstract Properties and Methods

        /// <summary>
        /// Get the fully resolved state name in format
        /// parent.parent.state
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Provides the default return msg
        /// </summary>
        /// <param name="msg">The incomming message</param>
        protected abstract ISpMessage GetDefaultReturnMsg(ISpMessage msg);


        protected abstract ISpMessage GetReponseMsg(int responseId, ISpMessage msg);


        #endregion





    }
}
