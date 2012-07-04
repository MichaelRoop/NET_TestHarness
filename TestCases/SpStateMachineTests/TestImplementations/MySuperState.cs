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

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass> {

        #region Data


        #endregion

        #region Constructors

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(new SpEnumToInt(id), dataClass) {
        }

        public MySuperState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, new SpEnumToInt(id), dataClass) {
        }

        #endregion

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


        protected override string ConvertStateIdToString(int id) {
            return SpConverter.IntToEnum<MyStateID>(id).ToString();
        }


        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertEventIdToString(int id) {
            return SpConverter.IntToEnum<MyEventType>(id).ToString();
        }

        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertMsgTypedToString(int id) {
            return SpConverter.IntToEnum<MyMsgType>(id).ToString();
        }




        /// <summary>
        /// Get the default return message for the success
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override ISpEventMessage GetDefaultReturnMsg(ISpEventMessage msg) {
            return MySpTools.GetDefaultReturnMsg(msg);
        }

        protected override ISpEventMessage GetReponseMsg(ISpEventMessage msg) {
            // TODO - build a factory to get the right response message

            throw new NotImplementedException();
        }

        #endregion
    }
}
