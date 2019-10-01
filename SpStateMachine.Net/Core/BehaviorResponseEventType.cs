using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Core {

    /// <summary>
    /// Strongly typed event types which the ISpBehavior has to handle
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public enum BehaviorResponseEventType {

        /// <summary>The Periodic Timer Fired</summary>
        PeriodicWakeup,

        /// <summary>A message was placed in the store</summary>
        MsgArrived,

        /// <summary>A request to Terminate wait was received</summary>
        TerminateRequest,

        /// <summary>For testing only</summary>
        Undefined,

    }
}
