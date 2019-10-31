using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DI {
    public class DummyDI {

        #region static instances for singletons

        private static ISpMsgFactory msgFactoryInstance = null;

        #endregion

        #region Static constructors for singletons

        public static ISpMsgFactory MsgFactoryInstance {
            get {
                if (msgFactoryInstance == null) {
                    msgFactoryInstance = new SpMsgFactory(new DemoMsgProvider());
                }
                return msgFactoryInstance;
            }
        }



        #endregion


    }
}
