using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>
    /// Example of using enums to strongly type the in values
    /// </summary>
    public enum MyReturnCode : int {

        Success,

        FailedPresure,

        Undefined,

    }


    /// <summary>
    /// Sample use of enums to provide strong typing for message type ids
    /// </summary>
    public static class MyReturnCodeExtensions {

        /// <summary>
        /// Transform the id enum to int
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Int(this MyReturnCode self) {
            return (int)self;
        }

        public static MyReturnCode ToReturnCode(this int value) {
            MyReturnCode id = MyReturnCode.Undefined;
            WrapErr.ToErrReport(9999, () => { return String.Format(""); }, () => {
                id = (MyReturnCode)Enum.Parse(typeof(MyReturnCode), value.ToString());
            });
            return id;
        }
    }


}
