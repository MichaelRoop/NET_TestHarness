using SpStateMachine.Core;
using System;

namespace SpStateMachine.Interfaces {
    
    /// <summary>Information required to execute a state transition</summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpStateTransition<TMsgId> : ICloneable where TMsgId : struct {


        /// <summary>Type of transition to execute</summary>
        SpStateTransitionType TransitionType { get; set; }


        /// <summary>Registered next state for NextState transitions</summary>
        ISpState<TMsgId> NextState { get; set; }


        /// <summary>Response message to return to caller</summary>
        ISpEventMessage ReturnMessage { get; set; }
        
    }
}
