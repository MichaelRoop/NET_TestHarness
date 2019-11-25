using SpStateMachine.Interfaces;
using System.Collections.Generic;

namespace SpStateMachine.EventStores {

    /// <summary>Simple event object store using a queue</summary>
    /// <typeparam name="T">The payload type of the event object</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SimpleDequeEventStore : BaseEventStore { 

        #region Data

        /// <summary>Event queue</summary>
        private Queue<ISpEventMessage> queue = new Queue<ISpEventMessage>();

        #endregion

        #region Constructors

        /// <summary>Constructor</summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public SimpleDequeEventStore(ISpEventMessage defaultTick)
            : base(defaultTick) {
        }
        
        #endregion

        #region BaseEventStore overrides

        /// <summary>Get an event from the queue</summary>
        /// <returns>The next event or null if none found</returns>
        protected override ISpEventMessage GetEvent() {
            return this.queue.Count == 0 ? null : this.queue.Dequeue();
        }


        /// <summary>Add an event to the queue</summary>
        /// <param name="msg">The message to add</param>
        protected override void AddEvent(ISpEventMessage msg) {
            this.queue.Enqueue(msg);
        }

        #endregion
    }
}
