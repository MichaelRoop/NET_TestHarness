using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {

    /// <summary>Default tick message</summary>
    public class MyTickMsg : MyBaseMsg {

        /// <summary>Constructor</summary>
        public MyTickMsg()
            : base(MyMsgType.SimpleMsg, MyEventType.Tick, SpEventPriority.Normal) {
        }

    }
}
