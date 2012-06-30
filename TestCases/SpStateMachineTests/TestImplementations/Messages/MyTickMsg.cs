using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {
    
    public class MyTickMsg : MyBaseMsg {

        public MyTickMsg()
            : base(MyMsgType.SimpleMsg, MyEventType.Tick, SpEventPriority.Normal) {
        }

    }
}
