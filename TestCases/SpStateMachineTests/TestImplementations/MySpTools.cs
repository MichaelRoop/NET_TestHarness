using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public static class MySpTools {


        /// <summary>
        /// Get the default return message for the success
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            WrapErr.ChkParam(msg, "msg", 9999);
            WrapErr.ChkTrue(msg is MyBaseMsg, 9999, () => {
                return String.Format("msg is {0} rather than MyBaseMsg based", msg.GetType().Name);
            });
            return new MySimpleOkResponse((MyBaseMsg)msg);
        }





    }
}
