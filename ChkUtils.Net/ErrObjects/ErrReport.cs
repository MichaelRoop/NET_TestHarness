using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using ChkUtils.Net.ExceptionFormating;
using ChkUtils.Net.ExceptionParsers;

namespace ChkUtils.Net.ErrObjects {
    /// <summary>
    /// Object to hold error information
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    [DataContract]
    public class ErrReport {

        #region Data

        private int code = 0;

        private string atMethod = "";

        private string atClass = "";

        private string msg = "";

        private StringBuilder stackTrace = new StringBuilder();

        DateTime stamp = DateTime.Now;

        private static string ERR_REPORT_PARSE_TOKEN = "##_ErrReport_##_Parse_##_Token_##";

        private static string TIME_STAMP_FORMAT = "dd/MM/yyyy HH:mm:ss.fff";

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

        /// <summary>
        /// The time stamp
        /// </summary>
        [DataMember]
        public DateTime TimeStamp {
            get {
                return this.stamp;
            }
            set {
                this.stamp = value;
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
            this.InitialiseStackTraceInfo(atException);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="method">Originating stack frame method</param>
        /// <param name="msg">Error message</param>
        /// <param name="atException">Originating Exception</param>
        public ErrReport(int code, ErrorLocation location, string msg, Exception atException)
            : this(code, location.ClassName, location.MethodName, msg, atException) {
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
        public ErrReport(int code, ErrorLocation location, string msg)
            : this(code, location.ClassName, location.MethodName, msg, null) {
        }

        #endregion

        #region Static Methods


        public static bool TryParse(string str, out ErrReport err) {
            err = null;
            if (ErrReport.IsErrReportString(str)) {
                try {
                    ErrReport tmp = ErrReport.NewErrReport(ErrReport.ExtractParsable(str));
                    if (tmp.AtClass != "ErrReport") {
                        err = tmp;
                    }
                }
                catch {
                }
            }
            return err != null;
        }


        /// <summary>
        /// Validate if the string is to product of a ReportError.ToString() dump
        /// </summary>
        /// <param name="str">The string to validate</param>
        /// <returns>true if string is not null and contains the parse token, otherwise false</returns>
        public static bool IsErrReportString(string str) {
            if (str != null) {
                return str.Contains(ErrReport.ERR_REPORT_PARSE_TOKEN);
            }
            return false;
        }


        /// <summary>
        /// Create an error report from the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ErrReport NewErrReport(string str) {
            if (str == null) {
                return new ErrReport(1, "ErrReport", "NewErrReport", "null parse string");
            }

            if (!ErrReport.IsErrReportString(str)) {
                return new ErrReport(1, "ErrReport", "NewErrReport", "parse string does not come from ErrReport.ToString()");
            }


            try {
                // Cut out the string portion
                string parseString = ErrReport.ExtractParsable(str);

                string[] parts = parseString.Split(new string[] { ErrReport.ERR_REPORT_PARSE_TOKEN }, StringSplitOptions.None);
                if (parts.Length != 8) {
                    throw new Exception(string.Format("{0} parts - 8 parts required", parts.Length));
                }

                return new ErrReport() {
                    Code = ErrReport.StringToCode(ErrReport.GetPart(parts, 1)),
                    AtClass = ErrReport.GetPart(parts, 2),
                    AtMethod = ErrReport.GetPart(parts, 3),
                    Msg = ErrReport.GetPart(parts, 4),
                    StackTrace = ErrReport.GetPart(parts, 5),
                    TimeStamp = ErrReport.StringToTimeStamp(ErrReport.GetPart(parts, 6))
                };
            }
            catch (Exception e) {
                return new ErrReport(1, "ErrReport", "NewErrReport", string.Format("Failed to parse errReport string '{0}' - Msg '{1}'", str, e.Message), e);
            }
        }



        /// <summary>
        /// Get the parsed part and validate
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetPart(string[] parts, int index) {
            if (parts == null) {
                throw new Exception("Parts list string array is null");
            }

            if (index >= parts.Length) {
                throw new Exception(string.Format("index {0} to string part exceeds parts number {1}", index, parts.Length));
            }

            string part = parts[index];
            if (part == null) {
                throw new Exception(string.Format("Part at index {0} is null", index));
            }
            return part;
        }


        /// <summary>
        /// Parser support to parse date time string including milliseconds or throw exception
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <returns></returns>
        private static DateTime StringToTimeStamp(string str) {
            try {
                return DateTime.ParseExact(str, ErrReport.TIME_STAMP_FORMAT, CultureInfo.InvariantCulture);
            }
            catch {
                throw new Exception(string.Format("The timestamp could not be parsed from '{0}'", str));
            }
        }


        /// <summary>
        /// Parse support to parse string to code or throw exception
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <returns>The code</returns>
        private static int StringToCode(string str) {
            int tmpCode;
            if (!Int32.TryParse(str, out tmpCode)) {
                throw new Exception(string.Format("The int error code could not be parsed from '{0}'", str));
            }
            return tmpCode;
        }


        /// <summary>
        /// Extract the portion of the string that is parsable as an ErrReport
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ExtractParsable(string str) {
            int firstPos = str.IndexOf(ErrReport.ERR_REPORT_PARSE_TOKEN);
            int lastPos = str.LastIndexOf(ErrReport.ERR_REPORT_PARSE_TOKEN);
            return str.Substring(firstPos, lastPos - firstPos + ErrReport.ERR_REPORT_PARSE_TOKEN.Length);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reset the Error report to initial state of success with no messages
        /// </summary>
        public void Reset() {
            this.code = 0;
            this.atMethod = "";
            this.atClass = "";
            this.msg = "";
            this.stackTrace = new StringBuilder();
            this.stamp = DateTime.Now;
        }


        /// <summary>
        /// Maintains class and method information and only resets anything from prior error
        /// </summary>
        public void ResetErrorInfoOnly() {
            this.code = 0;
            this.msg = "";
            this.stackTrace = new StringBuilder();
            this.stamp = DateTime.Now;
        }


        /// <summary>
        /// Transfer the values from another report
        /// </summary>
        /// <param name="report"></param>
        public void Reset(ErrReport report) {
            this.Code = report.Code;
            this.AtClass = report.AtClass;
            this.AtMethod = report.AtMethod;
            this.Msg = report.Msg;
            this.StackTrace = report.StackTrace;
            this.TimeStamp = report.TimeStamp;
        }


        /// <summary>
        /// Parse Exception to stack trace string and store in report
        /// </summary>
        /// <param name="e"></param>
        public void InitialiseStackTraceInfo(Exception e) {
            // Translate any exception information to string but do not store the exception. This allows the 
            // object to be serialized and passed to a FaultException that can used to traverse WCF boundries
            try {
                ExceptionFormaterFactory.Get().FormatException(ExceptionParserFactory.Get(e), stackTrace);
            }
            catch (Exception ee) {
                System.Diagnostics.Debug.WriteLine(string.Format("Exception caught from the exception formater - {0} - {1} {2}", e.Message, ee.Message, ee.StackTrace));
            }
        }


        /// <summary>
        /// Create a string with parse token so that an ErrReport can be generated from the string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}",
                ErrReport.ERR_REPORT_PARSE_TOKEN,
                this.Code,
                this.AtClass,
                this.AtMethod,
                this.Msg,
                this.StackTrace,
                this.TimeStamp.ToString(ErrReport.TIME_STAMP_FORMAT, CultureInfo.InvariantCulture));
        }

        #endregion

    }
}
