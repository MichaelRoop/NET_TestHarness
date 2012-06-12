using System;
using System.Text;
using ChkUtils.ExceptionParsers;
using System.Reflection;
using System.Runtime.Serialization;

namespace ChkUtils.ErrObjects {

    /// <summary>
    /// Object to hold error information
    /// </summary>
    /// <author>Michael Roop</author>
    [DataContract]
    public class ErrReport {

        #region Data

        private int code = 0;

        private string atMethod = "";

        private string atClass = "";

        private string msg = "";

        private StringBuilder stackTrace = new StringBuilder();

        #endregion

        #region Properties

        /// <summary>
        /// The error code
        /// </summary>
        [DataMember]
        public int Code {
            get {
                return this.code;
            }
            set {
                this.code = value;
            }
        }

        /// <summary>
        /// The originating class for the error
        /// </summary>
        [DataMember]
        public string AtClass {
            get {
                return this.atClass;
            }
            set {
                this.atClass = value;
            }
        }
        

        /// <summary>
        /// The originating class for the error
        /// </summary>
        [DataMember]
        public string AtMethod {
            get {
                return this.atMethod;
            }
            set {
                this.atMethod = value;
            }
        }

        /// <summary>
        /// The error message
        /// </summary>
        [DataMember]
        public string Msg {
            get {
                return this.msg;
            }
            set {
                this.msg = value;
            }
        }

        /// <summary>
        /// The stack trace
        /// </summary>
        [DataMember]
        public string StackTrace {
            get {
                return this.stackTrace.ToString();
            }
            set {
                this.stackTrace.Clear();
                this.stackTrace.Append(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for object with no error (success)
        /// </summary>
        public ErrReport() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Originating class</param>
        /// <param name="atMethod">Originating method</param>
        /// <param name="msg">Error message</param>
        /// <param name="atException">Originating Exception</param>
        public ErrReport(int code, string atClass, string atMethod, string msg, Exception atException) {
            this.code = code;
            this.atClass = atClass;
            this.atMethod = atMethod;
            this.msg = msg;

            // Translate any exception information to string but do not store the exception. This allows the 
            // object to be serialized and passed to a FaultException that can used to traverse WCF boundries
            if (atException != null) {
                IExceptionParser parser = ExceptionParserFactory.Get(atException);
                this.stackTrace
                    .AppendLine()
                    .AppendLine(String.Format("{0} : {1}", parser.GetInfo().Name, parser.GetInfo().Msg));

                parser.GetExtraInfoInfo().ForEach(item => this.stackTrace.AppendLine(String.Format("{0}={1}", item.Name, item.Value)));
                parser.GetStackFrames(true).ForEach(item => this.stackTrace.AppendLine(item));
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="method">Originating stack frame method</param>
        /// <param name="msg">Error message</param>
        /// <param name="atException">Originating Exception</param>
        public ErrReport(int code, MethodBase method, string msg, Exception atException)
            : this(code, method.DeclaringType.Name, method.Name, msg, atException) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="atClass">Originating class</param>
        /// <param name="atMethod">Originating method</param>
        /// <param name="msg">Error message</param>
        public ErrReport(int code, string atClass, string atMethod, string msg)
            : this(code, atClass, atMethod, msg, null) {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="method">Originating stack frame method</param>
        /// <param name="msg">Error message</param>
        public ErrReport(int code, MethodBase method, string msg)
            : this(code, method.DeclaringType.Name, method.Name, msg, null) {
        }

        #endregion
    }
}
