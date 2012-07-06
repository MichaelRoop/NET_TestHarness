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
        public static ISpEventMessage GetDefaultReturnMsg(ISpEventMessage msg) {
            WrapErr.ChkParam(msg, "msg", 9999);


            // From MySpTools
            return new MyBaseResponse(
                MyMsgType.SimpleResponse,
                new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Tick),
                MyReturnCode.Success,
                "OK"); 


//            // TODO - the ChkTrue does not return a message with the exception

//            //WrapErr.ChkTrue(msg is MyBaseMsg, 9999, () => {
//            //    return String.Format("msg is {0} rather than MyBaseMsg based", msg.GetType().Name);
//            //});

//            //Console.WriteLine("****************");
//            //WrapErr.ChkTrue(msg is MyBaseMsg, 9999, "msg is not MyBaseMsg based");
////            WrapErr.ChkTrue(false, 9999, "msg is not MyBaseMsg based");
////            Console.WriteLine("++++++++++++++++");

//            if (msg is MyBaseMsg) {
//                return new MySimpleOkResponse((MyBaseMsg)msg);
//            }
//            else if (msg is MyBaseResponse) {

//                // TODO - figure this one out
//                return msg;
//            }

//            WrapErr.ChkTrue(false, 9999, "Msg is neither type");
//            return msg;

////            return new MySimpleOkResponse((MyBaseMsg)msg);
        }





    }
}
