using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCases.SpStateMachineTests.TestImplementations.Messages {

    /// <summary>
    /// Default response for ok returns
    /// </summary>
    public class MySimpleOkResponse : MyBaseResponse {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">The paired incoming message</param>
        public MySimpleOkResponse(MyBaseMsg msg)
            : this(msg, "") {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">The paired incoming message</param>
        /// <param name="status">Extra status information</param>
        public MySimpleOkResponse(MyBaseMsg msg, string status)
            : base(MyMsgType.SimpleResponse, msg, MyReturnCode.Success, status) {
        }
        
    }
}
