using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;


namespace SpStateMachine.EventStores {

    /// <summary>
    /// Base class for event stores that handle locking and using
    /// default tick object for non sent event ticks
    /// </summary>
    /// <author>Michael Roop</author>
    public abstract class BaseEventStore : ISpEventStore {
        
        #region Data

        /// <summary>Thread safe lock for the queue</summary>
        private object queueLock = new object();

        /// <summary>Used to hold the tick event when there are no queued event objects</summary>
        private ISpEvent defaultTick = null;

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
        public BaseEventStore(ISpEvent defaultTick) {
            this.defaultTick = defaultTick;
        }
        
        #endregion

        #region IEventStore

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        public void Add(ISpEvent eventObject) {
            lock (this.queueLock) {
                this.AddEvent(eventObject);
            }
        }


        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The T object</returns>
        public ISpEvent Get() {
            // Make stack variable and only lock the queue for the duration of the copy to
            // free it up for other threads to add events
            ISpEvent eventCopy = null;
            lock (this.queueLock) {
                eventCopy = this.GetEvent();
            }
            return eventCopy == null ? this.defaultTick : eventCopy;
        }

        #endregion
        
        #region Abstract Methods

        /// <summary>
        /// Get an event from the store child implementation
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected abstract ISpEvent GetEvent();


        /// <summary>
        /// Add an event to the child implementation
        /// </summary>
        /// <param name="eventObject">The event object to add</param>
        protected abstract void AddEvent(ISpEvent eventObject);

        #endregion


    }
}
