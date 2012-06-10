using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils {

    /// <summary>
    /// Central parser for displayable exception iformation
    /// </summary>
    public class ExceptionInfo {

        #region Data

        private string name = "";

        private string msg = "";

        #endregion

        #region Properties

        public string Name {
            get {
                return this.name;
            }
        }

        public string Msg {
            get {
                return this.msg;
            }
        }


        #endregion

        #region Constructors

        private ExceptionInfo() {
        }


        public ExceptionInfo(Exception e) {
            this.name = e.GetType().Name;
            this.msg = e.Message;
        }


        #endregion



    }

}
