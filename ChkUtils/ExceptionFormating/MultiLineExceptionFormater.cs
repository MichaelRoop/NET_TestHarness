using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils.ExceptionParsers;

namespace ChkUtils.ExceptionFormating {

    public class MultiLineExceptionFormater : IExceptionOutputFormater {

        public void FormatException(IExceptionParser parser, StringBuilder target) {

            // Get the first level parser for first level exception
            //IExceptionParser parser = ExceptionParserFactory.Get(e);
            while (parser != null) {
                // Exception Name and message
                target.AppendLine(String.Format("{0} : {1}", parser.Info.Name, parser.Info.Msg));

                // Extra info items added one per line
                parser.ExtraInfo.ForEach(
                    item => target.AppendLine(String.Format("{0}={1}", item.Name, item.Value)));

                // Stack trace items one per line
                parser.GetStackFrames(true).ForEach(
                    item => target.AppendLine(item));

                // Recurse to inner parser for inner exception
                parser = parser.InnerParser;
            }

        }


        //public void FormatException(IExceptionParser parser, StringBuilder target) {
        //    throw new NotImplementedException();
        //}
    }
}
