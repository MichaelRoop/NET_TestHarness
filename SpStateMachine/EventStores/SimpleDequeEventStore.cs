using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;

namespace SpStateMachine.EventStores {

    public class SimpleDequeEventStore<T> : ISpEventStore<T> {

        #region Data

        /// <summary>Thread safe lock for the queue</summary>
        private object queueLock = new object();

        /// <summary>Event queue</summary>
        private Queue<T> queue = new Queue<T>();

        /// <summary>Used to hold the tick event when there are no queued event objects</summary>
        private T defaultTick = default(T);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SimpleDequeEventStore() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public SimpleDequeEventStore(T defaultTick) {
            this.defaultTick = defaultTick;
        }
        
        #endregion

        #region IEventStore

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        public void Add(T eventObject) {
            lock (this.queueLock) {
                this.queue.Enqueue(eventObject);
            }
        }


        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The T object</returns>
        public T Get() {
            // Make stack variable and only lock the queue for the duration of the copy to
            // free it up for other threads to add other events
            T eventCopy = default(T);
            lock (this.queueLock) {
                eventCopy = this.queue.Count > 0 ? this.queue.Dequeue() : this.defaultTick;
            }
            return eventCopy;
        }

        #endregion
    }
}
