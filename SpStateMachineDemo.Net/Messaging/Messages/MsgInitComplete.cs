using System;
using System.Collections.Generic;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachineDemo.Net.Messaging.Messages {
    public class MsgInitComplete : DemoMsgBase {
        
        public MsgInitComplete() 
            : base(DemoMsgType.SimpleMsg, DemoMsgId.InitComplete, SpEventPriority.Normal) {
            // TODO - only works with Simple msg and not simple response types
        }

    }
}
