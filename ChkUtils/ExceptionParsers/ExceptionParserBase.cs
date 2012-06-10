using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ChkUtils.ExceptionParsers {

    public abstract class ExceptionParserBase : IExceptionParser {

        #region Data

        ExceptionInfo info = null;

        List<ExceptionExtraInfo> extraInfo = new List<ExceptionExtraInfo>();

        List<string> stackFrames = new List<string>();

        #endregion


        private ExceptionParserBase() {
        }


        public ExceptionParserBase(Exception e) {
            this.info = new ExceptionInfo(e);
            this.AddExtraInfo(e);
            this.AddStackFrames(e);
        }


        public ExceptionInfo GetInfo() {
            return this.info;
        }


        public List<ExceptionExtraInfo> GetExtraInfoInfo() {
            return this.extraInfo;
        }


        public List<string> GetStackFrames(bool reversed) {
            return this.stackFrames;
        }


        protected abstract void AddExtraInfo(Exception e);


        private void AddStackFrames(Exception e) {

            StackTrace trace = new StackTrace(e, true);
            for (int i =0; i < trace.FrameCount; i++) {
                // Note that high up the call stack, there is only one stack frame.
                StackFrame sf = trace.GetFrame(i);
                //                                        Console.WriteLine ("{0} - {1} - {2}", sf.GetFileLineNumber (), sf.GetFileName (), sf.GetMethod ());
                //Console.WriteLine("{0} - {1} - {2}", sf.GetFileLineNumber(), WrapErr.GetFileName(sf), sf.GetMethod());

                this.stackFrames.Add(String.Format("\t{0} : Line:{1} - {2}.{3}", 
                    WrapErr.GetFileName(sf), sf.GetFileLineNumber(), WrapErr.GetClassName(sf), sf.GetMethod()));


                //Console.WriteLine (); 
                //Console.WriteLine ("High up the call stack, Method: {0}", sf.GetMethod ());
                //Console.WriteLine ("High up the call stack, Line Number: {0}", sf.GetFileLineNumber ());
            }
            Console.WriteLine("-----------------------------------------------------------------------");



        }



    }
}
