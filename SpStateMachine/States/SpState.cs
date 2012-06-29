﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Core;
using ChkUtils;

namespace SpStateMachine.States {

    /// <summary>
    /// Base implementation of the ISpState interface
    /// </summary>
    /// <typeparam name="T">Generic type that the state represents</typeparam>
    /// <author>Michael Roop</author>
    public class SpState<T> : ISpState {

        #region Data

        /// <summary>
        /// Holds data and method accessible to all states
        /// </summary>
        private T wrappedObject = default(T);

        /// <summary>
        /// The default response if there are no responses registered
        /// </summary>
  //      private ISpMessage defaultResonse = null;

        /// <summary>
        /// Tracks the current state of the state
        /// </summary>
        private bool isEntered = false;


        int stateId = 0;


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

        #region ISpState Properties

        /// <summary>
        /// Queries if OnEntry has already been invoked. It can only
        /// be invoked once until the OnExit is called
        /// </summary>
        public bool IsEntryExcecuted {
            get {
                return this.isEntered;
            }
        }

        /// <summary>
        /// The unique state identifier
        /// </summary>
        public int StateId {
            get {
                return this.stateId;
            }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpState() {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique state id</param>
        /// <param name="wrappedObject">The generic object that the states represent</param>
        public SpState(int id, T wrappedObject) {
            this.stateId = id;
            ;
            this.wrappedObject = wrappedObject;
        }

        #endregion

        #region ISpState Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public ISpStateTransition OnEntry(ISpMessage msg) {
            WrapErr.ChkFalse(this.IsEntryExcecuted, 9999, "OnEntry Cannot be Executed More Than Once Until OnExit is Called");
            this.isEntered = true;
            return this.tempGenerator(this.ExecOnEntry(msg));
        }


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        public ISpStateTransition OnTick(ISpMessage msg) {
            return this.tempGenerator(this.ExecOnTick(msg));
        }


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        public void OnExit() {
            this.isEntered = false;
            this.ExecOnExit();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Virtual method invoked on entry. If not overriden it will return 
        /// the default return object
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpMessage ExecOnEntry(ISpMessage msg) {
            // If no override it will query to get default return
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


        /// <summary>
        /// Virtual method invoked on every tick after. If not overriden it 
        /// will return the default return object
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A transition object</returns>
        protected virtual ISpMessage ExecOnTick(ISpMessage msg) {
            return new BaseResponse(0, (BaseMsg)msg, 0, "");
        }


        /// <summary>
        /// Virtual method invoked on exit from state.
        /// </summary>
        protected virtual void ExecOnExit() {
        }

        #endregion

        // TODO - develop the transition registration and generation
        ISpStateTransition tempGenerator(ISpMessage msg) {
            return new SpStateTransition(false, null, msg);
        }



    }
}
