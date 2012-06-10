using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChkUtils {

    public class ExceptionExtraInfo {

        #region Data

        private string name = "";

        private string value = "";

        #endregion

        #region Properties

        public string Name {
            get { 
                return this.name; 
            }
        }

        public string Value {
            get {
                return this.value;
            }
        }


        #endregion

        #region Constructors

        public ExceptionExtraInfo(string name, string value) {
            this.name = name;
            this.value = value;
        }

        #endregion


    }
}
