using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public enum MyMsgType : int {

        // Messages ---------------

        SimpleMsg,

        DataStrMsg,

        // Responses ---------------

        SimpleResponse,


        Undefined,

    }


    /// <summary>
    /// Sample use of enums to provide strong typing for message type ids
    /// </summary>
    public static class MyMsgIdExtensions {

        /// <summary>
        /// Transform the id enum to int
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Int(this MyMsgType self) {
            return (int)self;
        }

        public static MyMsgType ToMsgType(this int value) {
            MyMsgType id = MyMsgType.Undefined;
            WrapErr.ToErrReport(9999, () => { return String.Format(""); }, () => {
                id = (MyMsgType)Enum.Parse(typeof(MyMsgType), value.ToString());
            });
            return id;
        }
    }



}
