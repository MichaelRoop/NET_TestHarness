using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public enum MyEventType : int {

        Tick = 0,

        Start,

        Stop,

        Abort,




        // -- reponses

    }

    /// <summary>
    /// Sample use of enums to provide strong typing for event type ids
    /// </summary>
    public static class MyEventIdExtensions {

        /// <summary>
        /// Transform the id enum to int
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Int(this MyEventType self) {
            return (int)self;
        }


        public static MyEventType ToEventType(this int value) {
            MyEventType id = MyEventType.Tick;
            WrapErr.ToErrReport(9999, () => { return String.Format(""); }, () => {
                id = (MyEventType)Enum.Parse(typeof(MyEventType), value.ToString());
            });
            return id;
        }
    }




}
