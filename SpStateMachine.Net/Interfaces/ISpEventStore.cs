using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for an SpEvent object with generic payload
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpEventStore : IDisposable {

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        void Add(ISpEventMessage eventObject);

        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns>The event object</returns>
        ISpEventMessage Get();
        
    }
}
