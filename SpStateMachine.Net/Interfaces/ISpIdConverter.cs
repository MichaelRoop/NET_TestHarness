
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Used for the conversion of internal int ids to string
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpIdConverter<TState,TMsgId,TMsgType> where TState : struct where TMsgId :struct where TMsgType : struct {

        /// <summary>Convert State id to string via enum</summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        string StateId(int id);


        /// <summary>Convert event id to string</summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        string EventId(int id);


        /// <summary>Convert the message id to string</summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        string MsgTypeId(int id);

    }

}
