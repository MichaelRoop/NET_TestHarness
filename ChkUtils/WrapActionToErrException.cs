using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ChkUtils {

    /// <summary>
    /// Partial class implementation with catch action to ErrReportException
    /// </summary>
    public static partial class WrapErr {




        public static void ActionOnly(int code, string msg, Action action) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(WrapErr.GetErrReport(code, msg, e));


                //Console.WriteLine("-----------------------------------------------------------------------");
                //ErrReport report = GetErrReport(code, msg, e);

                //Console.WriteLine("-----------------------------------------------------------------------");
                //Console.WriteLine("Error:{0} Called from:{1}.{2} - {3}", report.Code, report.AtClass, report.AtMethod, report.Msg);


                ////throw new ErrReportException (code, atClass, atMethod, msg, e);

                //Console.WriteLine("-----------------------------------------------------------------------");
                //Console.WriteLine("Stack trace from exception");

                //Console.WriteLine("StackFrame Method.MethodBase:{0}", new StackTrace().GetFrame(1).GetMethod().Name);


                //StackTrace trace = new StackTrace(e, true);
                //for (int i =0; i < trace.FrameCount; i++) {
                //    // Note that high up the call stack, there is only one stack frame.
                //    StackFrame sf = trace.GetFrame(i);
                //    //                                        Console.WriteLine ("{0} - {1} - {2}", sf.GetFileLineNumber (), sf.GetFileName (), sf.GetMethod ());
                //    Console.WriteLine("{0} - {1} - {2}", sf.GetFileLineNumber(), GetFileName(sf), sf.GetMethod());

                //    //Console.WriteLine (); 
                //    //Console.WriteLine ("High up the call stack, Method: {0}", sf.GetMethod ());
                //    //Console.WriteLine ("High up the call stack, Line Number: {0}", sf.GetFileLineNumber ());
                //}
                //Console.WriteLine("-----------------------------------------------------------------------");
            }
        }


        public static string GetFileName(StackFrame frame) {
            // Strip off the path
            string name = frame.GetFileName();
            if (name == null) {
                name = "NoFileName";
            }

            if (name.Length > 0) {
                int pos = name.LastIndexOf('\\');
                if (name.Length > 0 && pos != -1 && pos < (name.Length - 1)) {
                    name = name.Substring(pos + 1);
                }
            }
            return name;
        }

        public static string GetMethodName(StackFrame frame) {
            MethodBase method = frame.GetMethod();
            //Console.WriteLine(method.Name);
            //Console.WriteLine(method.
            
            return method.Name;
        }


        public static string GetClassName(StackFrame frame) {
            MethodBase method = frame.GetMethod();
            //Console.WriteLine(method.Name);
            //Console.WriteLine(method.

            return method.DeclaringType.Name;
        }




        public static void ActionOnly(int code, string atClass, string atMethod, string msg, Action action) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(code, atClass, atMethod, msg, e);
            }
        }

        public static void ActionFinaly(int code, string atClass, string atMethod, string msg, Action action, Action finallyAction) {
            try {
                action.Invoke();
            }
            catch (ErrReportException) {
                throw;
            }
            catch (Exception e) {
                throw new ErrReportException(code, atClass, atMethod, msg, e);
            }
            finally {
                WrapErr.SafeAction(finallyAction);
            }
        }


    }
}
