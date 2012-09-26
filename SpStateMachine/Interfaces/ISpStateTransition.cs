using SpStateMachine.Core;
using System;

namespace SpStateMachine.Interfaces {
    
    /// <summary>
    /// Contains the necessary information to execute a state transition
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpStateTransition : ICloneable {

        /// <summary>
        /// The type of transition to execute
        /// </summary>
        SpStateTransitionType TransitionType { get; set; }

        /// <summary>
        /// The registered next state for NextState transitions
        /// </summary>
        ISpState NextState { get; set; }

        /// <summary>
        /// The response message to return to caller
        /// </summary>
        ISpEventMessage ReturnMessage { get; set; }
        
    }
}
