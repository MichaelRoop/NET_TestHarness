using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class MyDoneMsg : MyBaseMsg {
        public MyDoneMsg()
            : base(MyMsgType.SimpleMsg, MyMsgId.RespDone) {
        }
    }
}
