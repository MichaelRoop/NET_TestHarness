
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to a provider that will deliver implementation specific messages
    /// in response to user criteria passed in. It is also responsible for 
    /// copying over related data from input message to the response
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpMsgProvider {

        /// <summary>
        /// Returns the default success response
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage DefaultMsg(ISpEventMessage msg);


        /// <summary>
        /// Returns the response that corresponds to the incomming message
        /// event Id and type
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        ISpEventMessage Response(ISpEventMessage msg);


        /// <summary>
        /// Returns the response that corresponds to the incomming message
        /// event Id and type. It will also copy over any relevant data from the 
        /// incomming message to the response message
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <param name="registeredMsg">
        /// The return message stored in the registered transaction. If not null it
        /// will provide the lookup for the reponse and the data from the incoming 
        /// message will be copied into it.
        /// </param>
        /// <returns>The response message</returns>
        ISpEventMessage Response(ISpEventMessage msg, ISpEventMessage registeredMsg);


    }
}
