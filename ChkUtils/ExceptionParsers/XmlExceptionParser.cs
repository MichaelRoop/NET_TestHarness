using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace ChkUtils.ExceptionParsers {

    /// <summary>
    /// Parse out the particulars of an XmlException
    /// </summary>
    /// <author>Michael Roop</author>
    public class XmlExceptionParser : ExceptionParserBase {
        
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">The Exception to parse</param>
        public XmlExceptionParser(Exception e)
            : base(e) {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Add the extra info from XmlException
        /// </summary>
        /// <param name="e"></param>
        protected override void AddExtraInfo(Exception e) {
            if (e.GetType() == typeof(XmlException)) {
                XmlException ex = (XmlException)e;
                this.GetExtraInfoInfo().Add(new ExceptionExtraInfo("Line Number", ex.LineNumber.ToString()));
                this.GetExtraInfoInfo().Add(new ExceptionExtraInfo("Line Position", ex.LinePosition.ToString()));
                this.GetExtraInfoInfo().Add(new ExceptionExtraInfo("Source URI", ex.SourceUri));
            }
            else {
                WrapErr.SafeAction(() => 
                    Debug.WriteLine("The Exception is a {0} instead of an XmlException", e.GetType().Name));
            }
        }

        #endregion

    }
}
