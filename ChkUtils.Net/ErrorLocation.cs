namespace ChkUtils.Net {

    /// <summary>Cross platform object to store type and method location of error location</summary>
    public class ErrorLocation {

        /// <summary>Class name where error originated</summary>
        public string ClassName { get; set; }


        /// <summary>Method name where error originated</summary>
        public string MethodName { get; set; }

        #region Constructors

        public ErrorLocation() {
            this.ClassName = "NALocation";
            this.MethodName = "NALocation";
        }

        public ErrorLocation(string className, string methodName) {
            this.ClassName = className;
            this.MethodName = methodName;
        }

        #endregion

    }
}
