using System;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to define a listner for ISpMessages
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpEventListner : IDisposable {

        #region Events

        /// <summary>
        /// Event raised when a message is received. This would originate from the 
        /// outside and be subscribed to by the engine to be pushed to the 
        /// state machine
        /// </summary>
        event Action<ISpMessage> MsgReceived;

        /// <summary>
        /// Event raised when a response is received. This would originate from the
        /// state machine and be subscribed to by the originator of original message 
        /// to the state machine
        /// </summary>
        event Action<ISpMessage> ResponseReceived;
        
        #endregion

        #region Methods

        /// <summary>
        /// Post an ISpMessage to those listening
        /// </summary>
        /// <param name="msg">The message to post</param>
        void PostMessage(ISpMessage msg);


        /// <summary>
        /// Post an ISpMessage response to an ISpMessage
        /// </summary>
        /// <param name="msg">The reponse to post</param>
        void PostResponse(ISpMessage msg);

        #endregion

    }
}
