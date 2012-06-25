using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils.ExceptionFormating {

    public static class ExceptionFormaterFactory {

        private static IExceptionOutputFormater defaultFormater = new MultiLineExceptionFormater();

        private static object formaterLock = new object();


        public static void SetFormater(IExceptionOutputFormater formater) {
            lock (ExceptionFormaterFactory.formaterLock) {
                ExceptionFormaterFactory.defaultFormater = formater;
            }
        }

        public static IExceptionOutputFormater Get() {
            lock (ExceptionFormaterFactory.formaterLock) {
                return ExceptionFormaterFactory.defaultFormater;
            }
        }


    }
}
