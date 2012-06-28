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
            return this.ExecOnEntry(msg);
        }

        public ISpMessage OnTick(ISpMessage msg) {
            return this.ExecOnTick(msg);
        }

        public ISpMessage OnExit(ISpMessage msg) {
            return this.ExecOnExit(msg);
        }

        protected virtual ISpMessage ExecOnEntry(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }

        protected virtual ISpMessage ExecOnExit(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }

        protected virtual ISpMessage ExecOnTick(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


    }
}
