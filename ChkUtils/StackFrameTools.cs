using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ChkUtils {

    /// <summary>
    /// Common Methods to work through an exception stack frame
    /// </summary>
    public static class StackFrameTools {

        /// <summary>
        /// Get the file name with the path stripped off from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The file name</returns>
        public static string FileName(StackFrame frame) {
            if (frame == null) {
                Debug.WriteLine("WrapErr.GetFileName : Null frame");
                return "NoFileName";
            }

            string name = frame.GetFileName();
            if (name == null) {
                Debug.WriteLine("WrapErr.GetFileName : Null name in frame");
                return "NoFileName";
            }

            // Strip off the path
            if (name.Length > 0) {
                int pos = name.LastIndexOf('\\');
                if (name.Length > 0 && pos != -1 && pos < (name.Length - 1)) {
                    name = name.Substring(pos + 1);
                }
            }
            return name;
        }


        /// <summary>
        /// Get the method name from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The method name</returns>
        public static string MethodName(StackFrame frame) {
            try {
                return frame.GetMethod().Name;
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetMethodName : Exception {0} getting method name:{1}", e.GetType().Name, e.Message));
                return "NoMethodName";
            }
        }


        /// <summary>
        /// Get the line number from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The method name</returns>
        public static int Line(StackFrame frame) {
            try {
                return frame.GetFileLineNumber();
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetLine : Exception {0} getting line number:{1}", e.GetType().Name, e.Message));
                return 0;
            }
        }


        /// <summary>
        /// Get the class name from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The class name</returns>
        public static string ClassName(StackFrame frame) {
            try {
                return frame.GetMethod().DeclaringType.Name;
            }
            catch (Exception e) {
                Debug.WriteLine(String.Format("WrapErr.GetClassName : Exception {0} getting class name:{1}", e.GetType().Name, e.Message));
                return "NoClassName";
            }
        }


        /// <summary>
        /// Experimental method to walk through stack until you encounter the first non ErrWrap class method that does not have the <>
        /// </summary>
        /// <returns></returns>
        public static MethodBase FirstNonWrappedMethod(Type typeToIgnore) {
            // Go at least one up to ignore this method.
            int index = 1;
            StackTrace st = new StackTrace();

            MethodBase mb = st.GetFrame(index).GetMethod();
            while (true) {

                if ((typeToIgnore == null) || mb.DeclaringType.Name != typeToIgnore.Name && !mb.Name.Contains("<")) {
                    return mb;
                }

                ++index;
                StackFrame sf = st.GetFrame(index);
                if (sf == null) {
                    return mb;
                }

                MethodBase tmp = sf.GetMethod();
                if (tmp == null) {
                    return mb;
                }
                mb = tmp;
            }
        }



    }
}
