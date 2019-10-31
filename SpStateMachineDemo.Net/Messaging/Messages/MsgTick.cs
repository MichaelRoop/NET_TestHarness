namespace SpStateMachineDemo.Net.Messaging.Messages {

    public class MsgTick : DemoMsgBase {
        public MsgTick() : base(DemoMsgType.SimpleMsg, DemoMsgId.Tick) {
        }

    }
}
