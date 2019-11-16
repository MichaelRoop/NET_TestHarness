using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations.SuperStates.ExitSS {
    public class SS_A2 : MySuperState {

        ISpState<MyMsgId> stDone = null;

        public SS_A2(ISpState<MyMsgId> parent, MyStateID id, MyDataClass dataClass) 
            : base(parent, id, dataClass) {

            // This super state will exit on entry of first state
            this.stDone = this.AddSubState(new S_ExitDoneOnEntry(this, MyStateID.S_A1_ExitEntry, dataClass));
            this.stDone.ToExitOnResult(MyMsgId.RespDone);

            this.SetEntryState(this.stDone);
        }
    }
}
