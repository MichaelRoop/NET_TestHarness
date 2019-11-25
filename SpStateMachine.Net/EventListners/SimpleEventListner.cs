using ChkUtils.Net;
using LogUtils.Net;
using SpStateMachine.Interfaces;
using System;
using System.Threading;

namespace SpStateMachine.EventListners {

    /// <summary>Args for the messages or responses</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpMessagingArgs : EventArgs {

        /// <summary>Args payload which is the event message</summary>
        public ISpEventMessage Payload { get; private set; }

        public SpMessagingArgs(ISpEventMessage msg) {
            this.Payload = msg;
        }

    };


    /// <summary>
    /// A simple event listner. This class could be shared between the the
    /// state machine engine and a communications module as a bridge
    /// to push messages into the state machine and get back the responses
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public sealed class SimpleEventListner : ISpEventListner {

        #region ISpEventListner Events

        /// <summary>
        /// Event raised when a message is received. This would originate from the 
        /// outside and be subscribed to by the engine to be pushed to the 
        /// state machine
        /// </summary>
        public event EventHandler MsgReceived;

        /// <summary>
        /// Event raised when a response is received. This would originate from the
        /// state machine and be subscribed to by the originator of original message 
        /// to the state machine
        /// </summary>
        public event EventHandler ResponseReceived;

        #endregion

        #region ISpEventListner Methods

        /// <summary>Post ISpMessage to listeners. This will raise the MsgReceived event</summary>
        /// <param name="msg">The message to post</param>
        public void PostMessage(ISpEventMessage msg) {
            WrapErr.ChkDisposed(this.disposed, 50032);
            this.RaiseEvent(this.MsgReceived, msg, "Message");
        }


        /// <summary>
        /// Post an ISpMessage response to an ISpMessage. This will raise the 
        /// ResponseReceived event
        /// </summary>
        /// <param name="msg">The reponse to post</param>
        public void PostResponse(ISpEventMessage msg) {
            WrapErr.ChkDisposed(this.disposed, 50033);
            this.RaiseEvent(this.ResponseReceived, msg, "Response");
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            // There are no resources to dispose of in this implementation but keep 
            // same behavior that no usage after Dispose just for conformity
            this.disposed = true;
        }

        #endregion

        #region Private Methods

        /// <summary>Raise an event in a thread pool if there are subscribers to the event</summary>
        /// <param name="action">The action to raise</param>
        /// <param name="msg">The message or response to push</param>
        /// <param name="type">Either message or response identifier string</param>
        private void RaiseEvent(EventHandler action, ISpEventMessage msg, string type) {
            Log.Info("SimpleEventListner", "RaiseEvent", String.Format("Raising Event:{0} type:{1} Event Id:{2}", type, msg.TypeId, msg.EventId));
            
            if (action != null) {
                ThreadPool.QueueUserWorkItem((threadContext) => {
                    WrapErr.ToErrReport(50030,
                        () => { return String.Format("Unexpected Error Raising Event '{0}'", type); },
                        () => {
                            // Check again just before execution
                            if (action != null) {
                                action.Invoke(this, new SpMessagingArgs(msg));
                            }
                            else {
                                Log.Warning(50034, String.Format("In thread - No subscribers to '{0}' message", type));
                            }
                        });
                });
            }
            else {
                Log.Warning(50031, String.Format("No subscribers to '{0}' message", type));
            }
        }

        #endregion

    }
}
