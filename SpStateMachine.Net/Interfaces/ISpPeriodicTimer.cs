using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to define a periodic timer to pulse
    /// the state machine
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpPeriodicTimer : IDisposable {

        /// <summary>
        /// This event will be raised when the period expires and
        /// the periodic timer wakes up.
        /// </summary>
        event Action OnWakeup;

        /// <summary>
        /// Set the interval for wakeup
        /// </summary>
        /// <param name="interval">The TimeSpan object with interval information</param>
        void SetInterval(TimeSpan interval);


        /// <summary>
        /// Start the periodic timer
        /// </summary>
        void Start();


        /// <summary>
        /// Stop the periodic timer
        /// </summary>
        void Stop();
        

    }
}
