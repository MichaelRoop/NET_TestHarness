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
    /// <typeparam name="T">The payload type</typeparam>
    public class PriorityEventStore<T> : BaseEventStore<T> {

        #region Data

        /// <summary>Low Priority Event queue</summary>
        private Queue<ISpEvent<T>> lowPriorityQueue = new Queue<ISpEvent<T>>();

        /// <summary>Normal Priority Event queue</summary>
        private Queue<ISpEvent<T>> NormalPriorityQueue = new Queue<ISpEvent<T>>();

        /// <summary>Hight Priority Event queue</summary>
        private Queue<ISpEvent<T>> HighPriorityQueue = new Queue<ISpEvent<T>>();

        /// <summary>Urgent Priority Event queue</summary>
        private Queue<ISpEvent<T>> UrgentPriorityQueue = new Queue<ISpEvent<T>>();
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private PriorityEventStore() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public PriorityEventStore(ISpEvent<T> defaultTick) {
            this.defaultTick = defaultTick;
        }
        
        #endregion

        #region BaseEventStore overrides

        /// <summary>
        /// Get an event from the highest level queue descending
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected override ISpEvent<T> GetEvent() {
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
        protected override void AddEvent(ISpEvent<T> eventObject) {
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
