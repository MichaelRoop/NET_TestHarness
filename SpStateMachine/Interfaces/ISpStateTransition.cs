using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {
    
    /// <summary>
    /// Contains the necessary information to execute a state transition
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpStateTransition {

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
        ISpMessage ReturnMessage { get; set; }
        
    }
}
