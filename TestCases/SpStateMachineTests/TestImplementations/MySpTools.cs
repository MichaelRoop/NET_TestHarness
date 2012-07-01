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

            // TODO - the ChkTrue does not return a message with the exception

            WrapErr.ChkTrue(msg is MyBaseMsg, 9999, () => {
                return String.Format("msg is {0} rather than MyBaseMsg based", msg.GetType().Name);
            });

            //Console.WriteLine("****************");
            //WrapErr.ChkTrue(msg is MyBaseMsg, 9999, "msg is not MyBaseMsg based");
//            WrapErr.ChkTrue(false, 9999, "msg is not MyBaseMsg based");
//            Console.WriteLine("++++++++++++++++");



            return new MySimpleOkResponse((MyBaseMsg)msg);
        }





    }
}
