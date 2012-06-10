using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils.ExceptionParsers {

    public class DefaultExceptionParser : ExceptionParserBase {

        public DefaultExceptionParser(Exception e)
            : base(e) {
        }

        protected override void AddExtraInfo(Exception e) {
            // Default one will not attempt to get any extra info
        }
    }
}
