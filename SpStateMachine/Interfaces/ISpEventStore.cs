using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for an SpEvent object with generic payload
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpEventStore : IDisposable {

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        void Add(ISpMessage eventObject);

        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The event object</returns>
        ISpMessage Get();
        
    }
}
