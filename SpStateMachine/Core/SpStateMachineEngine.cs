using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;
using System.Threading;
using ChkUtils.ErrObjects;

namespace SpStateMachine.Core {

    /// <summary>
    /// Combines the different elements of the State Machine architecture
    /// together to drive events and behavior
    /// </summary>
    /// <typeparam name="TEventObject"></typeparam>
    public sealed class SpStateMachineEngine<TEventObject> : IDisposable {

        #region Data

        object busyLock = new object();

        bool isBusy =  false;

        ISpPeriodicTimer timer = null;

        ISpStateMachine<TEventObject> stateMachine = null;

        ISpEventStore<TEventObject> eventStore = null;

        ISpEventListner <TEventObject> eventListner = null;

        Action wakeUpAction = null;

        Action<TEventObject> eventReceivedAction = null;

        ManualResetEvent threadWakeEvent = new ManualResetEvent(false);

        private bool terminateThread = false;

        private Thread driverThread = null;

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
        public SpStateMachineEngine(ISpEventListner<TEventObject> eventListner, ISpEventStore<TEventObject> eventStore, ISpStateMachine<TEventObject> stateMachine, ISpPeriodicTimer timer) {
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

            this.driverThread = new Thread(new ThreadStart(this.DriverThread));
            this.driverThread.Start();

            this.eventListner.EventReceived += this.eventReceivedAction;
            this.timer.OnWakeup += this.wakeUpAction;
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~SpStateMachineEngine() {
            this.Dispose(false);
        }

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

        /// <summary>
        /// Thread to drive the state machine
        /// </summary>
        private void DriverThread() {
            // Prevent any exceptions from propagating from thread
            ErrReport err = new ErrReport();
            while (!this.terminateThread) {
                WrapErr.ToErrReport(out err, 9999, () => {
                    // Return from push and wait
                    this.ThreadAction(() => { this.SetBusy(false); });
                    this.ThreadAction(() => { this.threadWakeEvent.WaitOne(); });

                    // Wakeup and push next event to state machine
                    this.ThreadAction(() => { this.SetBusy(true, () => { this.threadWakeEvent.Reset(); }); });
                    this.ThreadAction(() => { this.stateMachine.Tick(this.eventStore.Get()); });
                });
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Wrap an action in a check to see first if thread is terminated
        /// </summary>
        /// <param name="action">The action to invoke</param>
        private void ThreadAction(Action action) {
            if (!this.terminateThread) {
                WrapErr.SafeAction(action);
            }
        }


        /// <summary>
        /// Thread safe query of busy state
        /// </summary>
        /// <returns>true if busy, otherwise false</returns>
        private bool IsBusy() {
            lock (this.busyLock) {
                return this.isBusy;
            }
        }


        /// <summary>
        /// Set the busy state with no actions
        /// </summary>
        /// <param name="isBusy">true is state is to be busy, otherwise false</param>
        private void SetBusy(bool isBusy) {
            this.SetBusy(isBusy, () => { });
        }


        /// <summary>
        /// Set the state busy and invoke an action within the state lock
        /// </summary>
        /// <param name="isBusy">true is state is to be busy, otherwise false</param>
        /// <param name="action">The action to invoke within the busy state lock</param>
        private void SetBusy(bool isBusy, Action action) {
            lock (this.busyLock) {
                this.isBusy = false;
                WrapErr.SafeAction(() => action.Invoke());
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

        #region IDisposable

        private bool disposed = false;

        public void Dispose() {
            this.Dispose(true);

            // Prevent finalizer call if already released
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposed) {
                WrapErr.SafeAction(() => this.timer.Stop());

                // always save to shut down the thread first on Dispose even if called by Finalizer
                this.terminateThread = true;
                WrapErr.SafeAction(() => this.threadWakeEvent.Set());

                if (this.driverThread != null) {
                    if (!this.driverThread.Join(5000)) {
                        WrapErr.SafeAction(() => this.driverThread.Abort());
                    }
                }


                if (disposing) {
                    if (this.timer != null) {
                        WrapErr.SafeAction(() => this.timer.Dispose());
                        this.timer = null;
                    }
                    if (this.threadWakeEvent != null) {
                        WrapErr.SafeAction(() => this.threadWakeEvent.Dispose());
                        this.threadWakeEvent = null;
                    }

                    // TODO - find if others are to be disposed
                }
                disposed = true;
            }

        }


        #endregion
    }
}
