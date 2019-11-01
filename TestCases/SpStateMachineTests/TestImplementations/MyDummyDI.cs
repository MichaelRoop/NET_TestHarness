using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases.SpStateMachineTests.TestImplementations {
    public class MyDummyDI {

        #region static singleton instances

        private static ISpMsgFactory msgFactoryInstance = null;

        #endregion

        #region Static properties to return singletons

        public static ISpMsgFactory MsgFactoryInstance {
            get {
                if (msgFactoryInstance == null) {
                    msgFactoryInstance = new SpMsgFactory(new MyMsgProvider());
                }
                return msgFactoryInstance;
            }
        }



        #endregion



    }
}
