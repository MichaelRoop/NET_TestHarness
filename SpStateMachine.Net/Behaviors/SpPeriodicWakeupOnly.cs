using ChkUtils.Net;
using LogUtils.Net;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using System.Threading;

namespace SpStateMachine.Behaviours {

    /// <summary>Implementation of a behavior that wakes on periodic timer</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public sealed class SpPeriodicWakeupOnly : ISpBehaviorOnEvent {

        #region Data

        ManualResetEvent wakeEvent = new ManualResetEvent(false);

        /// <summary>Protect the isBusy status lock</summary>
        object busyLock = new object();

        /// <summary>Busy state flag</summary>
        bool isBusy =  false;


        #endregion

        #region Constructors

        /// <summary>Default constructor</summary>
        public SpPeriodicWakeupOnly() {
        }


        /// <summary>Finalizer</summary>
        ~SpPeriodicWakeupOnly() {
            this.Dispose(false);
        }

        #endregion

        #region ISpBehaviorOnEvent Members

        /// <summary>Invoked when an event is received</summary>
        /// <param name="eventType">The type of event received</param>
        public void EventReceived(BehaviorResponseEventType eventType) {
            WrapErr.ChkDisposed(this.disposed, 50080);
            switch (eventType) {
                case BehaviorResponseEventType.MsgArrived:
                    // We ignore messages received and depend only on the periodic timer 
                    break;
                case BehaviorResponseEventType.PeriodicWakeup:
                    this.OnPeriodicTimer();
                    break;
                case BehaviorResponseEventType.TerminateRequest:
                    this.wakeEvent.Set();
                    break;
                default:
                    Log.Error(50081, String.Format("The Behavior Response Event Type '{0}' is not Supported", eventType));
                    break;
            }
        }


        /// <summary>Wait indefinitely until behavior is satisfied</summary>
        public void WaitOnEvent() {
            WrapErr.ChkDisposed(this.disposed, 50082);
            lock (this.busyLock) {
                this.isBusy = false;
            }

            this.wakeEvent.WaitOne();

            // Reset lock so it will wait on next call
            lock (this.busyLock) {
                this.isBusy = true;
                this.wakeEvent.Reset();
            }
        }

        #endregion

        #region IDisposable

        private bool disposed = false;

        /// <summary>Dispose all resources in this object</summary>
        public void Dispose() {
            this.Dispose(true);

            // Prevent finalizer call if already released
            GC.SuppressFinalize(this);
        }


        /// <summary>Dispose resources        /// </summary>
        /// <param name="disposeManagedResources">
        /// If true it was called by the Dispose method rather than finalizer
        /// </param>
        private void Dispose(bool disposeManagedResources) {
            if (!disposed) {
                // In case something is waiting on it
                if (this.wakeEvent != null) {
                    this.wakeEvent.Set();
                }

                if (disposeManagedResources) {
                    this.DisposeManagedResources();
                }
                this.DisposeNativeResources();
            }
            this.disposed = true;
        }


        /// <summary>Dispose managed resources (those with Dispose methods)</summary>
        private void DisposeManagedResources() {
            WrapErr.ToErrReport(50083, "Error Disposing wakeEvent", () => {
                if (this.wakeEvent != null) {
                    wakeEvent.Dispose();
                    wakeEvent = null;
                }
            });
        }


        /// <summary>Dispose unmanaged native resources (InPtr, file handles)</summary>
        private void DisposeNativeResources() {
            // Nothing to cleanup
        }

        #endregion

        #region Private Methods

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
        /// Set the wakeup when the periodic timer fires unless it has not returned
        /// from its last tick. In this case we lose the tick.
        /// </summary>
        private void OnPeriodicTimer() {
            if (this.IsBusy()) {
                Log.Error(50084, "Still Busy When the Periodic Timer Woke Up");
            }
            else {
                this.wakeEvent.Set();
            }
        }

        #endregion

    }
}
