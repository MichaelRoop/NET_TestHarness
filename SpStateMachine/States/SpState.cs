using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;

namespace SpStateMachine.States {

    public class SpState<T> : ISpState {

        #region Data

        /// <summary>
        /// Holds data and method accessible to all states
        /// </summary>
        private T wrappedObject = default(T);

        #endregion

        #region Property

        /// <summary>
        /// Returns the core object which is state wrapped by the state machine
        /// </summary>
        public T This {
            get {
                return this.wrappedObject;
            }
        }

        #endregion

        #region Constructors

        private SpState() {
        }


        public SpState(T wrappedObject) {
            this.wrappedObject = wrappedObject;
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
