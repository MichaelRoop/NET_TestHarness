using System;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to define a listner for ISpMessages
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpEventListner {

        #region Events

        /// <summary>
        /// Event raised when a message is received
        /// </summary>
        event Action<ISpMessage> MsgReceived;

        #endregion

        #region Methods

        /// <summary>
        /// Post an ISpMessage response to an ISpMessage
        /// </summary>
        /// <param name="msg">The reponse to post</param>
        void PostResponse(ISpMessage msg);

        #endregion

    }
}
