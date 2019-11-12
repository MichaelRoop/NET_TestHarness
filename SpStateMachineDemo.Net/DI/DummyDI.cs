using SpStateMachine.Core;
using SpStateMachine.EventListners;
using SpStateMachine.Interfaces;
using SpStateMachineDemo.Net.Core;
using SpStateMachineDemo.Net.DemoMachine;
using SpStateMachineDemo.Net.DemoMachine.IO;

namespace SpStateMachineDemo.Net.DI {
    public class DummyDI {

        #region static singleton instances

        private static ISpMsgFactory msgFactoryInstance = null;
        private static IDemoInputs<InputId> inputsInstance = null;
        private static IDemoOutputs<OutputId> outputsInstance = null;
        private static ISpEventListner listnerInstance = null;
        private static DemoMachineObj machineObjInstance = null;

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


        public static ISpEventListner EventListnerInstance {
            get {
                if (listnerInstance == null) {
                    listnerInstance = new SimpleEventListner();
                }
                return listnerInstance;
            }
        }

        public static DemoMachineObj DemoMachineObjInstance {
            get {
                if (machineObjInstance == null) {
                    machineObjInstance = new DemoMachineObj();
                }
                return machineObjInstance;
            }
        }


        #endregion

    }
}
