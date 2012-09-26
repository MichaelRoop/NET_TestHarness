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
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public static class StackFrameTools {

        /// <summary>
        /// Get the file name with the path stripped off from the stack frame
        /// </summary>
        /// <param name="frame">The frame with the information</param>
        /// <returns>The file name</returns>
        public static string FileName(StackFrame frame) {
            if (frame == null) {
                //Debug.WriteLine("StackFrameTools.GetFileName : Null frame");
                return "NoFileName";
            }

            string name = frame.GetFileName();
            if (name == null) {
                //Debug.WriteLine("StackFrameTools.GetFileName : Null name in frame");
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
        /// Walk through stack until you encounter the first class that is not to be ignored and
        /// whose method does not have the <>
        /// </summary>
        /// <param name="typeToIgnore">Type to ignore as you travel the stack</param>
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


        /// <summary>
        /// Returns a list with the stack trace except the leading entries which are to be ignored
        /// </summary>
        /// <param name="typeToIgnore">The type to ignore on first entries</param>
        /// <param name="trace">The stack trace</param>
        /// <returns>A list with entries past the initial wraping type class</returns>
        public static List<string> FirstNonWrappedTraceStack(Type typeToIgnore, StackTrace trace) {
            bool firstNonWrapErr = false;
            string name = typeToIgnore.Name;

            List<string> stackFrames = new List<string>();
            for (int i =0; i < trace.FrameCount; i++) {
                StackFrame sf = trace.GetFrame(i);

                // Skip over all entries until you hit the first not to ignore
                if (!firstNonWrapErr) {
                    if (StackFrameTools.ClassName(sf) != name) {
                        firstNonWrapErr = true;
                    }
                    else {
                        continue;
                    }
                }

                stackFrames.Add(
                    String.Format("\t{0} : Line:{1} - {2}.{3}", StackFrameTools.FileName(sf), StackFrameTools.Line(sf), StackFrameTools.ClassName(sf), StackFrameTools.MethodName(sf)));
            }
            return stackFrames;
        }


    }
}
