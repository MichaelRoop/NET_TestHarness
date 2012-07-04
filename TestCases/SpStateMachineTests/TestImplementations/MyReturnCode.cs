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

}
