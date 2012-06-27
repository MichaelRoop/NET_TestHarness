using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;

namespace SpStateMachine.EventStores {

    /// <summary>
    /// Simple event object store using a queue
    /// </summary>
    /// <typeparam name="T">The payload type of the event object</typeparam>
    /// <author>Michael Roop</author>
    public class SimpleDequeEventStore : BaseEventStore { 

        #region Data

        /// <summary>Event queue</summary>
        private Queue<ISpEvent> queue = new Queue<ISpEvent>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public SimpleDequeEventStore(ISpEvent defaultTick)
            : base(defaultTick) {
        }
        
        #endregion

        #region BaseEventStore overrides

        /// <summary>
        /// Get an event from the queue
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected override ISpEvent GetEvent() {
            return this.queue.Count == 0 ? null : this.queue.Dequeue();
        }


        /// <summary>
        /// Add an event to the queue
        /// </summary>
        /// <param name="eventObject">The event object to add</param>
        protected override void AddEvent(ISpEvent eventObject) {
            this.queue.Enqueue(eventObject);
        }

        #endregion
    }
}
