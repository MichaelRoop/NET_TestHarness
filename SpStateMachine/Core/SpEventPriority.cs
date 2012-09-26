using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Core {

    /// <summary>
    /// Differnt priorities of Events
    /// </summary>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
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
