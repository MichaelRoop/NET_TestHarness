using System;

namespace SpStateMachine.Interfaces {

    /// <summary>Interface to define a periodic timer to pulse the state machine</summary>
    /// <author>Michael Roop</author>
    /// <remarks>
    /// You can have a manually set period such as in the WinSimpleTimer or you can implement
    /// a handle into a real time clock to generate the pulse
    /// </remarks>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpPeriodicTimer : IDisposable {

        /// <summary>Event will be raised when the period expires</summary>
        event Action OnWakeup;

        /// <summary>Set interval for wakeup</summary>
        /// <param name="interval">The TimeSpan object with interval information</param>
        void SetInterval(TimeSpan interval);


        /// <summary>Start the periodic timer</summary>
        void Start();


        /// <summary>Stop the periodic timer</summary>
        void Stop();

    }
}
