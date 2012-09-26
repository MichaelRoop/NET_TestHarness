using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;


namespace SpStateMachine.EventStores {

    /// <summary>
    /// Base class for event stores that handle locking and using
    /// default tick object for non sent event ticks
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public abstract class BaseEventStore : ISpEventStore {
        
        #region Data

        /// <summary>Thread safe lock for the queue</summary>
        private object queueLock = new object();

        /// <summary>Used to hold the tick event when there are no queued event objects</summary>
        private ISpEventMessage defaultTick = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private BaseEventStore() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public BaseEventStore(ISpEventMessage defaultTick) {
            WrapErr.ChkParam(defaultTick, "defaultTick", 50110);
            this.defaultTick = defaultTick;
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~BaseEventStore() {
            this.Dispose(false);
        }

        #endregion

        #region IEventStore

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="msg">The message event</param>
        public void Add(ISpEventMessage msg) {
            WrapErr.ChkDisposed(this.disposed, 50111);
            WrapErr.ChkParam(msg, "msg", 50112);
            lock (this.queueLock) {
                this.AddEvent(msg);
            }
        }


        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The T object</returns>
        public ISpEventMessage Get() {
            WrapErr.ChkDisposed(this.disposed, 50113);
            // Make stack variable and only lock the queue for the duration of the copy to
            // free it up for other threads to add events
            ISpEventMessage eventCopy = null;
            lock (this.queueLock) {
                eventCopy = this.GetEvent();
            }
            return eventCopy == null ? this.defaultTick : eventCopy;
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// Dispose any resources in the object
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            // Prevent finalizer call since we are releasing resources early
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
                if (disposeManagedResources) {
                    WrapErr.SafeAction(() => this.DisposeManagedResources());
                }
                WrapErr.SafeAction(() => this.DisposeNativeResources());
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose managed resources (those with Dispose methods)
        /// </summary>
        protected virtual void DisposeManagedResources() {
            // Nothing to cleanup
        }

        /// <summary>
        /// Dispose unmanaged native resources (InPtr, file handles)
        /// </summary>
        protected virtual void DisposeNativeResources() {
            // Nothing to cleanup
        }

        #endregion
                
        #region Abstract Methods

        /// <summary>
        /// Get an event from the store child implementation
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected abstract ISpEventMessage GetEvent();


        /// <summary>
        /// Add an event to the child implementation
        /// </summary>
        /// <param name="eventObject">The event object to add</param>
        protected abstract void AddEvent(ISpEventMessage eventObject);

        #endregion


    }
}
