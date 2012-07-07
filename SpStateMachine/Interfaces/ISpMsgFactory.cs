
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to a factory that will deliver message
    /// responses passed on user criterial
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpMsgFactory {
        
        /// <summary>
        /// This will return the default success response with only the GUID 
        /// transfered from the incoming message to satisfy any need of msg 
        /// correlation
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage GetDefaultResponse(ISpEventMessage msg);


        /// <summary>
        /// This will return the response with the GUID and payload
        /// transfered from the incoming message
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage GetResponse(ISpEventMessage msg);


        /// <summary>
        /// This will return the response with the GUID and payload transfered 
        /// from the incoming message but the message type and event type from
        /// the registered return message if not null
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <param name="registeredMsg">The return message stored in the registered transaction</param>
        /// <returns>The response message</returns>
        ISpEventMessage GetResponse(ISpEventMessage msg, ISpEventMessage registeredMsg);

    }
}
