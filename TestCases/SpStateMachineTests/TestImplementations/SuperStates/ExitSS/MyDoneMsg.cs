using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {

    public class MyDoneMsg : MyBaseMsg {
        public MyDoneMsg() : base(MyMsgType.SimpleResponse, MyMsgId.RespDone) { }
    }

}
