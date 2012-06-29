﻿using System;
using System.Threading;
using ChkUtils;
using LogUtils;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Combines the different elements of the State Machine architecture
    /// together to drive events and behavior
    /// </summary>
    /// <author>Michael Roop</author>
    public sealed class SpStateMachineEngine : IDisposable {

        #region Data

        ISpPeriodicTimer timer = null;

        ISpStateMachine stateMachine = null;

        ISpEventStore msgStore = null;

        ISpEventListner msgListner = null;

        ISpBehaviorOnEvent eventBehavior = null;

        Action wakeUpAction = null;

        Action<ISpMessage> msgReceivedAction = null;

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
        /// <param name="msgListner">The event listner object that receives events</param>
        /// <param name="msgStore">The object that stores the messages</param>
        /// <param name="eventBehavior">The behavior object that interacts with incoming events</param>
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
            while (!this.terminateThread) {
                WrapErr.ToErrReport(9999, () => {
                    this.ThreadAction(() => { this.eventBehavior.WaitOnEvent(); });
                    this.ThreadAction(() => { this.msgListner.PostResponse(this.stateMachine.Tick(this.msgStore.Get())); 
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


        /// <summary>
        /// Action that is fired on timer wakeup
        /// </summary>
        void timer_OnWakeup() {
            this.eventBehavior.EventReceived(BehaviorResponseEventType.PeriodicWakeup);
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
            WrapErr.SafeAction(() => 
                this.eventBehavior.EventReceived(BehaviorResponseEventType.TerminateRequest));

            if (this.driverThread != null) {
                if (!this.driverThread.Join(5000)) {
                    WrapErr.SafeAction(() => this.driverThread.Abort());
                }
                this.driverThread = null;
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
            this.DisposeObject(this.stateMachine, "stateMachine");
            this.DisposeObject(this.msgStore, "msgStore");
            this.DisposeObject(this.msgListner, "msgListner");

            this.timer = null;
            this.eventBehavior = null;
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
            WrapErr.ToErrReport(9999, String.Format("Error Disposing Object:{0}", name), () => {
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
