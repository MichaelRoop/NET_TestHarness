using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils {

    public static partial class WrapErr {



        public static void ToErrReport(out ErrReport report, int code, string msg, Action action) {
            try {
                action.Invoke();
                report = new ErrReport();
            }
            catch (ErrReportException e) {
                report = e.Report;
            }
            catch (Exception e) {
                report = WrapErr.GetErrReport(code, msg, e);
            }
        }

    }
}
