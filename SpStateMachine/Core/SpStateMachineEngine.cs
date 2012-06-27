using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;
using System.Threading;

namespace SpStateMachine.Core {

    public sealed class SpStateMachineEngine<TEventObject> {

        #region Data

        object busyLock = new object();

        bool isBusy =  false;

        IPeriodicTimer timer = null;

        ISpStateMachine<TEventObject> stateMachine = null;

        IEventStore<TEventObject> eventStore = null;

        IEventListner <TEventObject> eventListner = null;

        Action wakeUpAction = null;

        Action<TEventObject> eventReceivedAction = null;

        ManualResetEvent threadWakeEvent = new ManualResetEvent(false);

        private bool terminateThread = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpStateMachineEngine() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventListner">The event listner object that receives events</param>
        /// <param name="eventStore">The object that stores events</param>
        /// <param name="stateMachine">The state machine that interprets the events</param>
        /// <param name="timer">The periodic timer</param>
        public SpStateMachineEngine(IEventListner<TEventObject> eventListner, IEventStore<TEventObject> eventStore, ISpStateMachine<TEventObject> stateMachine, IPeriodicTimer timer) {
            WrapErr.ChkParam(eventStore, "eventStore", 99999);
            WrapErr.ChkParam(eventListner, "eventListner", 99999);
            WrapErr.ChkParam(stateMachine, "stateMachine", 99999);
            WrapErr.ChkParam(timer, "timer", 99999);

            this.wakeUpAction = new Action(timer_OnWakeup);
            this.eventReceivedAction = new Action<TEventObject>(eventListner_EventReceived);

            this.eventListner = eventListner;
            this.eventStore = eventStore;
            this.stateMachine = stateMachine;
            this.timer = timer;

            this.eventListner.EventReceived += this.eventReceivedAction;
            this.timer.OnWakeup += this.wakeUpAction;
        }


        // TODO IDisposable

        #endregion

        #region Public Methods

        public void Start() {
            this.timer.Start();
        }

        public void Stop() {
            this.timer.Stop();
        }

        #endregion

        #region Private Thread Methods

        private void DriverThread() {

            while (!this.terminateThread) {
                this.SetBusyState(false);
                this.threadWakeEvent.WaitOne();
                this.SetBusyState(true);
                this.threadWakeEvent.Reset();

                // Get the event from the event object and push to state machine
                this.stateMachine.Tick(this.eventStore.Get());
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Thread safe query of busy state
        /// </summary>
        /// <returns></returns>
        private bool IsBusy() {
            lock (this.busyLock) {
                return this.isBusy;
            }
        }

        /// <summary>
        /// Thread safe set of busy state
        /// </summary>
        /// <param name="isBusy">busy state status</param>
        private void SetBusyState(bool isBusy) {
            lock (this.busyLock) {
                this.isBusy = isBusy;
            }
        }


        /// <summary>
        /// Action that is fired on timer wakeup
        /// </summary>
        void timer_OnWakeup() {
            if (this.IsBusy()) {
                // Log error - if we leave here we will loose a period
            }
            else {
                this.threadWakeEvent.Set();
            }
        }


        /// <summary>
        /// Event from the event listner gets stuffed in the store
        /// </summary>
        /// <param name="eventObject"></param>
        void eventListner_EventReceived(TEventObject eventObject) {
            this.eventStore.Add(eventObject);
        }

        #endregion


    }
}
