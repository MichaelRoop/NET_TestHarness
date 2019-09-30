
namespace ChkUtils.Net.ExceptionFormating {

    /// <summary>
    /// Formater factory for the Exception stack trace information
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class ExceptionFormaterFactory {

        #region Data 

        private static IExceptionOutputFormater defaultFormater = new MultiLineExceptionFormater();
        //private static IExceptionOutputFormater defaultFormater = new SingleLineExceptionFormater(); 

        private static object formaterLock = new object();

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the formater object that will be returned by the factory
        /// </summary>
        /// <param name="formater">The formater object to return</param>
        public static void SetFormater(IExceptionOutputFormater formater) {
            lock (ExceptionFormaterFactory.formaterLock) {
                ExceptionFormaterFactory.defaultFormater = formater;
            }
        }

        /// <summary>
        /// Get the formater object
        /// </summary>
        /// <returns>The current default formater object</returns>
        public static IExceptionOutputFormater Get() {
            lock (ExceptionFormaterFactory.formaterLock) {
                return ExceptionFormaterFactory.defaultFormater;
            }
        }

        #endregion

    }
}
