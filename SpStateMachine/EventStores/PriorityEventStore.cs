using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Core;

namespace SpStateMachine.EventStores {
    
    /// <summary>
    /// Event store based on highest priority event being next in
    /// line. Using a queue for each priority to prevent sorting
    /// and maintain order of same priority events
    /// </summary>
    public class PriorityEventStore : BaseEventStore {

        #region Data

        /// <summary>Low Priority Event queue</summary>
        private Queue<ISpEvent> lowPriorityQueue = new Queue<ISpEvent>();

        /// <summary>Normal Priority Event queue</summary>
        private Queue<ISpEvent> NormalPriorityQueue = new Queue<ISpEvent>();

        /// <summary>Hight Priority Event queue</summary>
        private Queue<ISpEvent> HighPriorityQueue = new Queue<ISpEvent>();

        /// <summary>Urgent Priority Event queue</summary>
        private Queue<ISpEvent> UrgentPriorityQueue = new Queue<ISpEvent>();
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public PriorityEventStore(ISpEvent defaultTick)
            : base(defaultTick) {
        }
        
        #endregion

        #region BaseEventStore overrides

        /// <summary>
        /// Get an event from the highest level queue descending
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected override ISpEvent GetEvent() {
            if (this.UrgentPriorityQueue.Count > 0) {
                return this.UrgentPriorityQueue.Dequeue();
            }
            else if (this.HighPriorityQueue.Count > 0) {
                return this.HighPriorityQueue.Dequeue();
            }
            else if (this.NormalPriorityQueue.Count > 0) {
                return this.NormalPriorityQueue.Dequeue();
            }
            else if (this.lowPriorityQueue.Count > 0) {
                return this.lowPriorityQueue.Dequeue();
            }
            return null;
        }


        /// <summary>
        /// Add an event to the proper priority queue
        /// </summary>
        /// <param name="eventObject">The event object to add</param>
        protected override void AddEvent(ISpEvent eventObject) {
            switch (eventObject.Priority) {
                case SpEventPriority.Low:
                    this.lowPriorityQueue.Enqueue(eventObject);
                    break;
                case SpEventPriority.Normal:
                    this.NormalPriorityQueue.Enqueue(eventObject);
                    break;
                case SpEventPriority.High:
                    this.HighPriorityQueue.Enqueue(eventObject);
                    break;
                case SpEventPriority.Urgent:
                    this.UrgentPriorityQueue.Enqueue(eventObject);
                    break;
                default:
                    // TODO - log
                    break;
            }
        }

        #endregion
    }
}
