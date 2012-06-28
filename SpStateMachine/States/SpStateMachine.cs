using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;

namespace SpStateMachine.States {


    public class SpStateMachine<T> : ISpStateMachine {

        #region Data

        ISpState state = null;

        T statifiedObject = default(T);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpStateMachine() {
        }


        /// <summary>
        /// Constructor with DI injectable main state
        /// </summary>
        /// <param name="state">The state machine's main state</param>
        /// <remarks>
        /// This is usually a super state or parallel super state that will in turn tick its current state
        /// </remarks>
        public SpStateMachine(T statifiedObject, ISpState state) {
            this.statifiedObject = statifiedObject;
            this.state = state;
        }

        #endregion

        #region SpStateMachine

        public ISpMessage Tick(ISpMessage msg) {
            WrapErr.ChkParam(msg, "msg", 9999);
            return this.state.OnTick(msg);
        }

        #endregion

    }
}
