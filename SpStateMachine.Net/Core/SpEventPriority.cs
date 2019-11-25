namespace SpStateMachine.Core {

    /// <summary>Different priorities of Events</summary>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public enum SpEventPriority {

        /// <summary>Below normal priority level</summary>
        Low,

        /// <summary>Default priority level</summary>
        Normal,

        /// <summary>High priority level</summary>
        High,

        /// <summary>Highest priority level</summary>
        Urgent,

        /// <summary>For Testing Only</summary>
        Undefined,

    }
}
