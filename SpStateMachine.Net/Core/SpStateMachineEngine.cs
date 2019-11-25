using System;
using System.Threading;
using ChkUtils.Net;
using LogUtils.Net;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Combines different elements of the State Machine architecture to 
    /// drive events and behavior
    /// <summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public sealed class SpStateMachineEngine : IDisposable {

        #region Data

        ISpPeriodicTimer timer = null;

        ISpStateMachine stateMachine = null;

        ISpEventStore msgStore = null;

        ISpEventListner msgListner = null;

        ISpBehaviorOnEvent eventBehavior = null;

        Action wakeUpAction = null;

        private bool terminateThread = false;

        private Thread driverThread = null;

        ClassLog log = new ClassLog(typeof(SpStateMachineEngine).Name);

        #endregion

        #region Constructors

        /// <summary>Default constructor in private scope to prevent usage</summary>
        private SpStateMachineEngine() {
        }


        /// <summary>Constructor</summary>
        /// <param name="msgListner">The event listner object that receives events</param>
        /// <param name="msgStore">The object that stores the messages</param>
        /// <param name="eventBehavior">The behavior object that interacts with incoming events</param>
        /// <param name="stateMachine">The state machine that interprets the events</param>
        /// <param name="timer">The periodic timer</param>
        public SpStateMachineEngine(
            ISpEventListner msgListner,
            ISpEventStore msgStore,
            ISpBehaviorOnEvent eventBehavior,
            ISpStateMachine stateMachine,
            ISpPeriodicTimer timer) {

            WrapErr.ChkParam(msgListner, "msgListner", 50050);
            WrapErr.ChkParam(msgStore, "msgStore", 50051);
            WrapErr.ChkParam(eventBehavior, "eventBehavior", 50052);
            WrapErr.ChkParam(stateMachine, "stateMachine", 50053);
            WrapErr.ChkParam(timer, "timer", 50054);

            WrapErr.ToErrorReportException(50055, () => {
                this.msgListner = msgListner;
                this.msgStore = msgStore;
                this.eventBehavior = eventBehavior;
                this.stateMachine = stateMachine;
                this.timer = timer;

                this.driverThread = new Thread(new ThreadStart(this.DriverThread));
                this.driverThread.Start();

                // Initalise events that will be raised and connect them to objects that will raise them
                this.wakeUpAction = new Action(this.timer_OnWakeup);
                this.msgListner.MsgReceived += this.eventListner_MsgReceived;
                this.timer.OnWakeup += this.wakeUpAction;
            });
        }


        /// <summary>Finalizer</summary>
        ~SpStateMachineEngine() {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>Start up the Engine</summary>
        public void Start() {
            WrapErr.ChkDisposed(this.disposed, 50056);
            this.timer.Start();
        }


        /// <summary>Stop the Engine</summary>
        public void Stop() {
            WrapErr.ChkDisposed(this.disposed, 50057);
            this.timer.Stop();
        }

        #endregion

        #region Private Thread Methods

        /// <summary>
        /// Thread to drive the state machine
        /// </summary>
        private void DriverThread() {
            this.log.DebugEntry("DriverThread");

            while (!this.terminateThread) {
                WrapErr.ToErrReport(50058, () => {
                    if (!this.terminateThread) {
                        this.eventBehavior.WaitOnEvent();
                    }
                    if (!this.terminateThread) {
                        this.msgListner.PostResponse(this.stateMachine.Tick(this.msgStore.Get()));
                    }
                });
            }
            this.log.DebugExit("DriverThread");
        }

        #endregion

        #region Private Methods

        /// <summary>Action fired on timer wakeup</summary>
        void timer_OnWakeup() {
            this.eventBehavior.EventReceived(BehaviorResponseEventType.PeriodicWakeup);
        }


        /// <summary>Event from the event listner gets stuffed in the store</summary>
        /// <param name="msg"></param>
        void eventListner_MsgReceived(object sender, EventArgs e) {
            this.msgStore.Add(((SpMessagingArgs)e).Payload);
            this.eventBehavior.EventReceived(BehaviorResponseEventType.MsgArrived);
        }


        /// <summary>Stop the timer and tick thread        /// </summary>
        private void ShutDownThread() {
            this.log.DebugEntry("ShutDownThread");

            WrapErr.ToErrReport(50059, () => {
                WrapErr.SafeAction(() => this.timer.Stop());
                this.terminateThread = true;
                WrapErr.SafeAction(() =>
                    this.eventBehavior.EventReceived(BehaviorResponseEventType.TerminateRequest));

                if (this.driverThread != null) {
                    if (this.driverThread.IsAlive) {
                        if (!this.driverThread.Join(1000)) {
                            WrapErr.SafeAction(() => this.driverThread.Abort());
                        }
                    }
                    this.driverThread = null;
                }
            });
            this.log.DebugExit("ShutDownThread");
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
            this.log.Info("Dispose", String.Format("Disposed:{0} diposeManagedResources:{1}", this.disposed, disposeManagedResources));

            if (!disposed) {
                this.ShutDownThread();

                if (disposeManagedResources) {
                    this.DisposeManagedResources();
                }
                this.DisposeNativeResources();
            }
            this.disposed = true;
        }


        /// <summary>Dispose managed resources (those with Dispose methods)</summary>
        private void DisposeManagedResources() {
            this.log.DebugEntry("DisposeManagedResources");

            // Disconnect event handling
            WrapErr.SafeAction(() => { this.msgListner.MsgReceived -= this.eventListner_MsgReceived; });
            WrapErr.SafeAction(() => { this.timer.OnWakeup -= this.wakeUpAction; });

            this.DisposeObject(this.timer, "timer");
            this.DisposeObject(this.eventBehavior, "eventBehavior");
            this.DisposeObject(this.stateMachine, "stateMachine");
            this.DisposeObject(this.msgStore, "msgStore");
            this.DisposeObject(this.msgListner, "msgListner");

            this.timer = null;
            this.eventBehavior = null;
            this.stateMachine = null;
            this.msgStore = null;
            this.msgListner = null;
        }


        /// <summary>Factor out the disposal of objects</summary>
        /// <param name="disposableObject">object to dispose</param>
        /// <param name="name">Name of object for error logging</param>
        private void DisposeObject(IDisposable disposableObject, string name) {
            WrapErr.ToErrReport(50060, String.Format("Error Disposing Object:{0}", name), () => {
                if (disposableObject != null) {
                    disposableObject.Dispose();
                }
            });
        }


        /// <summary>Dispose unmanaged native resources (InPtr, file handles)</summary>
        private void DisposeNativeResources() {
            // Nothing to cleanup
        }

        #endregion
    }
}
