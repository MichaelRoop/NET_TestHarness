using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    public interface IEventListner <TEventObject> {

        event Action<TEventObject> EventReceived;

    }
}
