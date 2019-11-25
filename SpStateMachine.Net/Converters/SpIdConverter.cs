using SpStateMachine.Converters;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Net.Converters {

    /// <summary>The ID converter which gets enum string value from int id</summary>
    /// <typeparam name="TState">State id</typeparam>
    /// <typeparam name="TEvent">Event id</typeparam>
    /// <typeparam name="TMsg">Message id</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public class SpIdConverter<TState,TEvent,TMsg> : ISpIdConverter<TState,TEvent,TMsg> where TState : struct where TEvent : struct where TMsg : struct {

        #region ISpIdConverter Members

        /// <summary>Convert State id to string via enum</summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        public string StateId(int id) {
            return SpConverter.IntToEnum<TState>(id).ToString();
        }


        /// <summary>Convert event id to string</summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        public string EventId(int id) {
            return SpConverter.IntToEnum<TEvent>(id).ToString();
        }


        /// <summary>Convert the message id to string</summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns>String value id converted back to enum</returns>
        public string MsgTypeId(int id) {
            return SpConverter.IntToEnum<TMsg>(id).ToString();
        }

        #endregion

    }
}
