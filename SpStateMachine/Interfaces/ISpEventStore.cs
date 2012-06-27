using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for an SpEvent object with generic payload
    /// </summary>
    /// <typeparam name="T">The payload type</typeparam>
    /// <author>Michael Roop</author>
    public interface ISpEventStore <T> {

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        void Add(ISpEvent<T> eventObject);

        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The T object</returns>
        ISpEvent<T> Get();
        
    }
}
