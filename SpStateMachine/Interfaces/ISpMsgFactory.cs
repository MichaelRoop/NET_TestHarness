
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to a factory that will get message responses from a provider
    /// and make sure that the transfer is safe the GUID is copied over to 
    /// the response for message correlation
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpMsgFactory {
        
        /// <summary>
        /// Retrieves the default return message and initialises its correlating GUID 
        /// correlation
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage GetDefaultResponse(ISpEventMessage msg);


        /// <summary>
        /// Retrieves the response message and initialises its correlating GUID 
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage GetResponse(ISpEventMessage msg);


        /// <summary>
        /// Retrieves the response message and initialises its correlating GUID 
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <param name="registeredMsg">The return message stored in the registered transaction</param>
        /// <returns>The response message</returns>
        ISpEventMessage GetResponse(ISpEventMessage msg, ISpEventMessage registeredMsg);

    }
}
