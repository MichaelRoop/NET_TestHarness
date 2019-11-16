using LogUtils;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    class S_ExitDoneOnEntry : MyState {
        
        public S_ExitDoneOnEntry(ISpState<MyMsgId> parent, MyStateID id, MyDataClass dataClass) 
            : base(parent, id, dataClass) {
        }


        protected override ISpEventMessage ExecOnEntry(ISpEventMessage msg) {
            // Will declare done on entry as a result
            Log.Warning(8, "-----------------------------------------------------------------------------------");
            Log.Info("S_ExitDoneOnEntry", "ExecOnEntry", "*** Booting out the done message on entry ***");
            Log.Warning(8, "-----------------------------------------------------------------------------------");
            return new MyDoneMsg();
        }

    }
}
