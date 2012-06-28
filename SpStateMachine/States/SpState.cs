using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;

namespace SpStateMachine.States {

    public class SpState<T> : ISpState {

        #region Data

        T statifiedObject = default(T);

        #endregion

        #region Constructors

        private SpState() {
        }


        public SpState(T statifiedObject) {
            this.statifiedObject = statifiedObject;
        }

        #endregion

        public ISpMessage OnEntry(ISpMessage msg) {
            return this.ExecOnOnEntry(msg);
        }

        public ISpMessage OnTick(ISpMessage msg) {
            return this.ExecOnOnTick(msg);
        }

        public ISpMessage OnExit(ISpMessage msg) {
            return this.ExecOnOnExit(msg);
        }

        protected virtual ISpMessage ExecOnOnEntry(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }

        protected virtual ISpMessage ExecOnOnExit(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }

        protected virtual ISpMessage ExecOnOnTick(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


    }
}
