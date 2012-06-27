using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils.ErrObjects;
using ChkUtils;
using System.Threading;
using System.Diagnostics;

namespace SpStateMachine.EventListners {

    public class SimpleEventListner : ISpEventListner {

        #region ISpEventListner Events

        /// <summary>
        /// Event raised when a message is received. This would originate from the 
        /// outside and be subscribed to by the engine to be pushed to the 
        /// state machine
        /// </summary>
        public event Action<ISpMessage>  MsgReceived;

        /// <summary>
        /// Event raised when a response is received. This would originate from the
        /// state machine and be subscribed to by the originator of original message 
        /// to the state machine
        /// </summary>
        public event Action<ISpMessage>  ResponseReceived;

        #endregion

        #region ISpEventListner Methods

        /// <summary>
        /// Post an ISpMessage to those listening
        /// </summary>
        /// <param name="msg">The message to post</param>
        public void PostMessage(ISpMessage msg) {
            this.RaiseEvent(this.MsgReceived, msg, "Message");
        }


        /// <summary>
        /// Post an ISpMessage response to an ISpMessage
        /// </summary>
        /// <param name="msg">The reponse to post</param>
        public void PostResponse(ISpMessage msg) {
            this.RaiseEvent(this.ResponseReceived, msg, "Response");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raise an event in a thread pool if there are subscribers to the event
        /// </summary>
        /// <param name="action">The action to raise</param>
        /// <param name="msg">The message or response to push</param>
        /// <param name="type">Either message or response identifier string</param>
        private void RaiseEvent(Action<ISpMessage> action, ISpMessage msg, string type) {
            Debug.WriteLine(String.Format("Raising Event:{0}", type));

            ErrReport err = new ErrReport();
            WrapErr.ToErrReport(out err, 9999,
                () => { return String.Format("Unexpected Error Raising Event {0}", type); },
                () => {
                    if (action != null) {
                        ThreadPool.QueueUserWorkItem((threadContext) => {
                            // Check again just before execution
                            if (action != null) {
                                action.Invoke(msg);
                            }
                            else {
                                // TODO log it
                                Debug.WriteLine(String.Format("No subscribers to {0} message", type));
                            }
                        });
                    }
                    else {
                        // TODO log it
                        Debug.WriteLine(String.Format("No subscribers to {0} message", type));
                    }
                });
        }


        #endregion


    }
}
