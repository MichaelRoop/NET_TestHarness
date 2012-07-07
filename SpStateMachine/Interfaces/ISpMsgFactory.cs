
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


        //ISpEventMessage GetResponse(int registeredMsgId);



        // Extension Test
 //       ISpStateTransition GetOnEventTransition(ISpStateTransition registered, ISpEventMessage msg, bool getDefault);


//        ISpStateTransition GetDefaultTransitionClone();



    }
}
