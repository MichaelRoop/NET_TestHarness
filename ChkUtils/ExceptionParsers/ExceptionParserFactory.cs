using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils.ExceptionParsers {

    public static class ExceptionParserFactory {

        public static IExceptionParser Get(Exception e) {

            // Add check for null exception

            if (e.GetType() == typeof(Exception)) {
                return new DefaultExceptionParser(e);
            }

            return new DefaultExceptionParser(e);

        }


    }

}

