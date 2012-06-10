using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils {
    /// <summary>
    /// Partial class of WrapErr with the validator Methods
    /// </summary>
    /// <author>Michael Roop</author>
    public static partial class WrapErr {

        public static void ValidateParam(int code, object obj, string name) {
            if (obj == null) {
                throw new ErrReportException(WrapErr.GetErrReport(code, String.Format("Null {0} Argument", name)));
            }
        }


        public static void ValidateTrue(bool condition, int code, string msg) {
            if (!condition) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg));
            }
        }


        public static void ValidateTrue(bool condition, int code, Func<string> msg) {
            if (!condition) {
                throw new ErrReportException(WrapErr.GetErrReport(code, WrapErr.SafeAction(msg)));
            }
        }



        public static void ValidateStr(int nullCode, int zeroLenCode, string name, string value) {
            WrapErr.ValidateParam(nullCode, value, name);
            WrapErr.ValidateTrue(value.Length > 0, zeroLenCode, () => { 
                return String.Format("String '{0}' is Zero Length", name); 
            });
        }


    }
}
