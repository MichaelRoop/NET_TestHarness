using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    public class SpStateTransition : ISpStateTransition {

        #region Data 

        bool hasTransition = false;

        ISpState nextState = null;

        ISpMessage returnMsg = null;

        #endregion


        #region ISpStateTransition Members

        public bool HasTransition {
            get { return this.hasTransition; 
            }
            set {
                this.hasTransition = value;
            }
        }

        public ISpState NextState {
            get {
                return this.nextState;
            }
            set {
                this.nextState = value;
            }
        }

        public ISpMessage ReturnMessage {
            get {
                return this.returnMsg;
            }
            set {
                this.returnMsg = value;
            }
        }

        #endregion


        #region Constructors 

        private SpStateTransition() {
        }


        public SpStateTransition(bool hasTransition, ISpState nextState, ISpMessage returnMsg) {
            this.hasTransition = hasTransition;
            this.nextState = nextState;
            this.returnMsg = returnMsg;
        }

        #endregion


    }
}
