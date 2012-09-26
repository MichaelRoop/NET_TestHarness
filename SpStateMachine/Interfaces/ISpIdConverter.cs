
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Used for the conversion of internal int ids to string
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpIdConverter {

        /// <summary>
        /// Convert the type id to string
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        string StateId(int id);


        /// <summary>
        /// Convert the event id to string
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        string EventId(int id);


        /// <summary>
        /// Convert the message id to string
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        string MsgTypeId(int id);

    }

}
