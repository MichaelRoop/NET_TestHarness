using System;
using System.Runtime.Serialization;

namespace Ca.Roop.TestHarness.TestExceptions
{
    public class InputException : System.Exception
    {

        public InputException() : base() {}
        public InputException(string msg) : base(msg) {}
        public InputException(string msg, Exception inner) : base(msg,inner) {}
        public InputException(Exception e) : base(e.Message,e) { }


        // Needed for serialisation for remote exceptions thrown.
        protected InputException(SerializationInfo info, StreamingContext context) : base() {}

    }
}
