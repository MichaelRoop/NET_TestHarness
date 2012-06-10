using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils.ExceptionParsers {

    public interface IExceptionParser {
        
        ExceptionInfo GetInfo();

        List<ExceptionExtraInfo> GetExtraInfoInfo();

        List<string> GetStackFrames(bool reversed);

    }
}
