using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.Core;
using SpStateMachineDemo.Net.DemoMachine.IO;

namespace SpStateMachineDemo.Net.DI {
    public class DummyDI {

        #region static singleton instances

        private static ISpMsgFactory msgFactoryInstance = null;
        private static IDemoInputs<InputId> inputsInstance = null;
        private static IDemoOutputs<OutputId> outputsInstance = null;

        #endregion

        #region Static properties to return singletons

        public static ISpMsgFactory MsgFactoryInstance {
            get {
                if (msgFactoryInstance == null) {
                    msgFactoryInstance = new SpMsgFactory(new DemoMsgProvider());
                }
                return msgFactoryInstance;
            }
        }


        public static IDemoOutputs<OutputId> OutputsInstance {
            get {
                if (outputsInstance == null) {
                    outputsInstance = new DemoOutputs();
                }
                return outputsInstance;
            }
        }


        public static IDemoInputs<InputId> InputsInstance {
            get {
                if (inputsInstance == null) {
                    inputsInstance = new DemoInputs(OutputsInstance);
                }
                return inputsInstance;
            }
        }


        #endregion

    }
}
