using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using ChkUtils;
using LogUtils;
using SpStateMachine.Converters;
using SpStateMachine.Core;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass,MyEventType> {

        #region Data


        #endregion

        #region Constructors

        // Singletons are test shortcuts. Should use Interface for DI

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(MyMsgFactory.Instance, MyIdConverter.Instance, new SpEnumToInt(id), dataClass) {
        }

        public MySuperState(ISpState<MyEventType> parent, MyStateID id, MyDataClass dataClass)
            : base(parent, MyMsgFactory.Instance, MyIdConverter.Instance, new SpEnumToInt(id), dataClass) {
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
            
            switch (SpConverter.IntToEnum<MyEventType>(msg.EventId)) {
                case MyEventType.Abort:
                    Log.Info("MySuperState", "OnRuntimeTransitionRequest", " *** Got abort and returning Stop");

                    // get the GUID
                    ISpEventMessage myMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Stop);
                    myMsg.Uid = msg.Uid;
                    return myMsg;
                default:

                    break;
            }


            //// Do not know which state this is except it is the current state
            if (msg.EventId == SpConverter.EnumToInt(MyEventType.Abort)) {
                //this.GetCurrentState().    

                // get the GUID
                ISpEventMessage myMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Start);

                // Try recursion - no will not work
                //this.OnTick(myMsg);

                //this.GetCurrentState().

                //return this.GetOnResultTransition(myMsg);
            }


            return base.OnRuntimeTransitionRequest(msg);
        }

        #endregion

        #region State transition helpers

        protected void ToNextOnResult(MyEventType ev, MyState newState) {
            this.RegisterOnResultTransition(
                new SpEnumToInt(ev), SpStateTransition<MyEventType>.ToNext(newState));
        }

        #endregion


    }
}
