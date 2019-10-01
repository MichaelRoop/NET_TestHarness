

namespace SpStateMachine.Core {

    /// <summary>
    /// Specify the kind of transistion to excecute
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public enum SpStateTransitionType {

        /// <summary>No transition</summary>
        SameState,

        /// <summary>Transition to the next state</summary>
        NextState,

        /// <summary>Exit this state</summary>
        ExitState,

        /// <summary>Defer transition decision</summary>
        Defered,

    }
}
