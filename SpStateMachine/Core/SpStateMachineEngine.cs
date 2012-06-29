using System;
using System.Threading;
using ChkUtils;
using ChkUtils.ErrObjects;
using SpStateMachine.Interfaces;
using LogUtils;

namespace SpStateMachine.Core {

    /// <summary>
    /// Combines the different elements of the State Machine architecture
    /// together to drive events and behavior
    /// </summary>
    public sealed class SpStateMachineEngine : IDisposable {

        #region Data

        //object busyLock = new object();

        //bool isBusy =  false;

        ISpPeriodicTimer timer = null;

        ISpStateMachine stateMachine = null;

        ISpEventStore msgStore = null;

        ISpEventListner msgListner = null;

        ISpBehaviorOnEvent eventBehavior = null;

        Action wakeUpAction = null;

        Action<ISpMessage> msgReceivedAction = null;

        //ManualResetEvent threadWakeEvent = new ManualResetEvent(false);

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
        public SpStateMachineEngine(ISpEventListner msgListner, ISpEventStore msgStore, ISpBehaviorOnEvent eventBehavior, ISpStateMachine stateMachine, ISpPeriodicTimer timer) {
            WrapErr.ChkParam(msgListner, "msgListner", 99999);
            WrapErr.ChkParam(msgStore, "eventStore", 99999);
            WrapErr.ChkParam(eventBehavior, "eventBehavior", 99999);
            WrapErr.ChkParam(stateMachine, "stateMachine", 99999);
            WrapErr.ChkParam(timer, "timer", 99999);

            this.wakeUpAction = new Action(timer_OnWakeup);
            this.msgReceivedAction = new Action<ISpMessage>(this.eventListner_MsgReceived);

            this.msgListner = msgListner;
            this.msgStore = msgStore;
            this.eventBehavior = eventBehavior;
            this.stateMachine = stateMachine;
            this.timer = timer;

            this.driverThread = new Thread(new ThreadStart(this.DriverThread));
            this.driverThread.Start();

            this.msgListner.MsgReceived += this.msgReceivedAction;
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
            WrapErr.ChkDisposed(this.disposed, 9999);
            this.timer.Start();
        }

        public void Stop() {
            WrapErr.ChkDisposed(this.disposed, 9999);
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
                    //this.ThreadAction(() => { this.SetBusy(false); });
                    //this.ThreadAction(() => { this.threadWakeEvent.WaitOne(); });
                    this.ThreadAction(() => { this.eventBehavior.WaitOnEvent(); });

                    //// Wakeup and push next event to state machine
                    //this.ThreadAction(() => { 
                    //    this.SetBusy(true, () => { 
                    //        this.threadWakeEvent.Reset(); 
                    //    }); 
                    //});
                    this.ThreadAction(() => { 
                        this.msgListner.PostResponse(
                            this.stateMachine.Tick(
                                this.msgStore.Get())); 
                    });
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


        ///// <summary>
        ///// Thread safe query of busy state
        ///// </summary>
        ///// <returns>true if busy, otherwise false</returns>
        //private bool IsBusy() {
        //    lock (this.busyLock) {
        //        return this.isBusy;
        //    }
        //}


        ///// <summary>
        ///// Set the busy state with no actions
        ///// </summary>
        ///// <param name="isBusy">true is state is to be busy, otherwise false</param>
        //private void SetBusy(bool isBusy) {
        //    this.SetBusy(isBusy, () => { });
        //}


        ///// <summary>
        ///// Set the state busy and invoke an action within the state lock
        ///// </summary>
        ///// <param name="isBusy">true is state is to be busy, otherwise false</param>
        ///// <param name="action">The action to invoke within the busy state lock</param>
        //private void SetBusy(bool isBusy, Action action) {
        //    lock (this.busyLock) {
        //        this.isBusy = isBusy;
        //        WrapErr.SafeAction(() => action.Invoke());
        //    }
        //}


        /// <summary>
        /// Action that is fired on timer wakeup
        /// </summary>
        void timer_OnWakeup() {
            this.eventBehavior.EventReceived(BehaviorResponseEventType.PeriodicWakeup);

            //if (this.IsBusy()) {
            //    // TODO - We leave here and loose a period. Could inject custom behaviors
            //    Log.Error(9999, "Worker Thread Still Busy When the Periodic Timer Woke Up");
            //}
            //else {
            //    this.threadWakeEvent.Set();
            //}
        }


        /// <summary>
        /// Event from the event listner gets stuffed in the store
        /// </summary>
        /// <param name="eventObject"></param>
        void eventListner_MsgReceived(ISpMessage eventObject) {
            this.msgStore.Add(eventObject);
            this.eventBehavior.EventReceived(BehaviorResponseEventType.MsgArrived);
        }


        /// <summary>
        /// Stop the timer and tick thread
        /// </summary>
        private void ShutDownThread() {
            WrapErr.SafeAction(() => this.timer.Stop());

            this.terminateThread = true;
            //WrapErr.SafeAction(() => this.threadWakeEvent.Set());
            this.eventBehavior.EventReceived(BehaviorResponseEventType.TerminateRequest);

            if (this.driverThread != null) {
                if (!this.driverThread.Join(5000)) {
                    WrapErr.SafeAction(() => this.driverThread.Abort());
                }
            }
        }

        #endregion

        #region IDisposable

        private bool disposed = false;

        public void Dispose() {
            this.Dispose(true);

            // Prevent finalizer call if already released
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposeManagedResources">
        /// If true it was called by the Dispose method rather than finalizer
        /// </param>
        private void Dispose(bool disposeManagedResources) {
            if (!disposed) {
                this.ShutDownThread();

                if (disposeManagedResources) {
                    this.DisposeManagedResources();
                }
                this.DisposeNativeResources();
            }
            this.disposed = true;
        }


        /// <summary>
        /// Dispose managed resources (those with Dispose methods)
        /// </summary>
        private void DisposeManagedResources() {
            this.DisposeObject(this.timer, "timer");
            this.DisposeObject(this.eventBehavior, "eventBehavior");
            //this.DisposeObject(this.threadWakeEvent, "threadWakeEvent");
            this.DisposeObject(this.stateMachine, "stateMachine");
            this.DisposeObject(this.msgStore, "msgStore");
            this.DisposeObject(this.msgListner, "msgListner");

            this.timer = null;
            this.eventBehavior = null;
            //this.threadWakeEvent = null;
            this.stateMachine = null;
            this.msgStore = null;
            this.msgListner = null;
        }


        /// <summary>
        /// Factor out the disposal of objects
        /// </summary>
        /// <param name="disposableObject"></param>
        /// <param name="name"></param>
        private void DisposeObject(IDisposable disposableObject, string name) {
            Log.Debug("SpStateMachineEngine", "DisposeObject", String.Format("Object name:{0}", name));
            WrapErr.ToErrReport(9999, String.Format("Disposing Object:{0}", name), () => {
                if (disposableObject != null) {
                    disposableObject.Dispose();
                }
            });
        }

        /// <summary>
        /// Dispose unmanaged native resources (InPtr, file handles)
        /// </summary>
        private void DisposeNativeResources() {
            // Nothing to cleanup
        }
        
        #endregion
    }
}
