using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Core;
using ChkUtils;

namespace SpStateMachine.EventStores {
    
    /// <summary>
    /// Event store based on highest priority event being next in
    /// line. Using a queue for each priority to prevent sorting
    /// and maintain order of same priority events
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class PriorityEventStore : BaseEventStore {

        #region Data

        /// <summary>Low Priority Event queue</summary>
        private Queue<ISpEventMessage> lowPriorityQueue = new Queue<ISpEventMessage>();

        /// <summary>Normal Priority Event queue</summary>
        private Queue<ISpEventMessage> NormalPriorityQueue = new Queue<ISpEventMessage>();

        /// <summary>Hight Priority Event queue</summary>
        private Queue<ISpEventMessage> HighPriorityQueue = new Queue<ISpEventMessage>();

        /// <summary>Urgent Priority Event queue</summary>
        private Queue<ISpEventMessage> UrgentPriorityQueue = new Queue<ISpEventMessage>();
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultTick">
        /// The default tick event if to provide if there are no queued event objects
        /// </param>
        public PriorityEventStore(ISpEventMessage defaultTick)
            : base(defaultTick) {
        }
        
        #endregion

        #region BaseEventStore overrides

        /// <summary>
        /// Get an event from the highest level queue descending
        /// </summary>
        /// <returns>The next event or null if none found</returns>
        protected override ISpEventMessage GetEvent() {
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
        /// <param name="msg">The msg to add</param>
        protected override void AddEvent(ISpEventMessage msg) {
            switch (msg.Priority) {
                case SpEventPriority.Low:
                    this.lowPriorityQueue.Enqueue(msg);
                    break;
                case SpEventPriority.Normal:
                    this.NormalPriorityQueue.Enqueue(msg);
                    break;
                case SpEventPriority.High:
                    this.HighPriorityQueue.Enqueue(msg);
                    break;
                case SpEventPriority.Urgent:
                    this.UrgentPriorityQueue.Enqueue(msg);
                    break;
                default:
                    WrapErr.ChkTrue(false, 50150, String.Format("The Priority Type '{0}' is not Handled", msg.Priority));
                    break;
            }
        }

        #endregion
    }
}
