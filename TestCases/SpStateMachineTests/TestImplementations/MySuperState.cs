using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass> {

        #region Data


        #endregion

        #region Constructors

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        public MySuperState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, id.Int(), dataClass) {
        }

        #endregion

        #region SpStateOverrides

        protected override string ConvertStateIdToString(int id) {
            return id.ToStateId().ToString();
        }


        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertEventIdToString(int id) {
            return id.ToEventType().ToString();
        }

        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertMsgTypedToString(int id) {
            return id.ToMsgType().ToString();
        }




        /// <summary>
        /// Get the default return message for the success
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            return MySpTools.GetDefaultReturnMsg(msg);
        }

        protected override ISpMessage GetReponseMsg(ISpMessage msg) {
            // TODO - build a factory to get the right response message

            throw new NotImplementedException();
        }

        #endregion
    }
}
