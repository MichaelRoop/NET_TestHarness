using LogUtils;
using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachine.States;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass,MyMsgId,MyStateID,MyMsgType> {

        #region Constructors

        // Note: Singletons are test shortcuts. Should pass in Interfaces via DI

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(MyDummyDI.MsgFactoryInstance, id, dataClass) {
        }

        public MySuperState(ISpState<MyMsgId> parent, MyStateID id, MyDataClass dataClass)
            : base(parent, MyDummyDI.MsgFactoryInstance, id, dataClass) {
        }

        #endregion


        //protected override ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
        //    Console.WriteLine("########## {0} has received OnTick event:{1}", this.FullName, eventMsg.EventId);
        //    return base.ExecOnEntry(eventMsg);
        //    // just push the same one back
        //    //return eventMsg;
        //}
        //protected override ISpEventMessage ExecOnTick(ISpEventMessage eventMsg) {
        //    Console.WriteLine("########## {0} has received OnTick event:{1}", this.FullName, eventMsg.EventId);
        //    return base.ExecOnEntry(eventMsg);
        //    // just push the same one back
        //    //return eventMsg;
        //}



        #region SpStateOverrides

        protected override ISpEventMessage OnRuntimeTransitionRequest(ISpEventMessage msg) {
            
            switch (SpConverter.IntToEnum<MyMsgId>(msg.EventId)) {
                case MyMsgId.Abort:
                    Log.Info("MySuperState", "OnRuntimeTransitionRequest", " *** Got abort and returning Stop");

                    // get the GUID
                    ISpEventMessage myMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop);
                    myMsg.Uid = msg.Uid;
                    return myMsg;
                default:

                    break;
            }


            //// Do not know which state this is except it is the current state
            if (msg.EventId == SpConverter.EnumToInt(MyMsgId.Abort)) {
                //this.GetCurrentState().    

                // get the GUID
                ISpEventMessage myMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start);

                // Try recursion - no will not work
                //this.OnTick(myMsg);

                //this.GetCurrentState().

                //return this.GetOnResultTransition(myMsg);
            }


            return base.OnRuntimeTransitionRequest(msg);
        }

        #endregion

    }
}
