﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface to define a periodic timer to pulse
    /// the state machine
    /// </summary>
    public interface IPeriodicTimer {

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
