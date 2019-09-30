using System;
using ChkUtils;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {
    
    /// <summary>Base class for an ISpStateMachine that owns, controls and disposes the wrapped object</summary>
    /// <remarks>
    /// The state machine allows only one state to be active at a time. Each state can act on the wrapped object
    /// according to what is allowed in that state
    /// </remarks>
    /// <typeparam name="T">Wraped object type constrained to a class with IDisposable Interface required</typeparam>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpMachine<T> : ISpStateMachine where T : class, IDisposable {

        #region Data

        /// <summary>The main state for the State Machine</summary>
        ISpState state = null;

        /// <summary>The object that the State Machine represents</summary>
        T wrappedObject = null;

        /// <summary>Flag to indicate if the first 'OnEntry' has been called</summary>
        private bool isStarted = false;

        #endregion

        #region ISpStateMachine Properties

        /// <summary>
        /// The current state which has all the ancestors and the actual
        /// operational state as the leaf.
        /// </summary>
        public string CurrentStateName {
            get {
                return this.state.CurrentStateName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpMachine() {
        }


        /// <summary>
        /// Constructor with DI injectable main state
        /// </summary>
        /// <param name="wrappedObject"></param>
        /// <param name="state">The state machine's main state</param>
        /// <remarks>
        /// The main state will be a super state or parallel super state implementation. You 
        /// can use a single state implementation if you only want the wrapped object to be 
        /// driven by periodic timer and have access to the messaging architecture
        /// </remarks>
        public SpMachine(T wrappedObject, ISpState state) {
            WrapErr.ChkParam(wrappedObject, "wrappedObject", 50170);
            WrapErr.ChkParam(state, "state", 50171);
            this.wrappedObject = wrappedObject;
            this.state = state;
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~SpMachine() {
            this.Dispose(false);
        }
        
        #endregion

        #region ISpStateMachine Methods


        /// <summary>
        /// Tick the current state to execute the action based on the event message
        /// </summary>
        /// <param name="eventMessage">The event message</param>
        /// <returns>The return message from the action</returns>
        public ISpEventMessage Tick(ISpEventMessage msg) {
            WrapErr.ChkDisposed(this.disposed, 50176);
            WrapErr.ChkParam(msg, "msg", 50172);

            // First tick to drive it from entry to regular
            ISpStateTransition tr = null;
            bool tmpIsStarted = this.isStarted;

            //Log.Debug("SpMachine", "Tick", String.Format("isStarted:{0}", this.isStarted));

            if (!this.isStarted) {
                // The OnEntry must be called directly from the state machine for the first state. Subsequent
                // state transitions will insure that the OnEntry for the new state is called
                this.isStarted = true;
                tr = this.state.OnEntry(msg);
            }
            else {
                tr = this.state.OnTick(msg);
            }
            WrapErr.ChkVar(tr, 50177, String.Format(
                "The State '{0}' {1} Returned a Null Transition", this.state.FullName, tmpIsStarted ? "OnTick" : "OnEntry" ));
            return tr.ReturnMessage;
        }

        #endregion
        
        #region IDisposable Members

        /// <summary>Disposed flag</summary>
        private bool disposed = false;

        /// <summary>
        /// Dispose any resources in the object
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            // Prevent finalizer call since we are releasing resources early
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposeManagedResources">
        /// If true it was called by the Dispose method rather than finalizer
        /// </param>
        private void Dispose(bool disposeManagedResources) {
            if (!disposed) {
                if (disposeManagedResources) {
                    WrapErr.ToErrorReportException(50173, () => this.DisposeManagedResources());
                }
                WrapErr.ToErrorReportException(50174, () => this.DisposeNativeResources());
            }
            this.disposed = true;
        }


        /// <summary>
        /// Dispose managed resources (those with Dispose methods)
        /// </summary>
        protected virtual void DisposeManagedResources() {
            WrapErr.ToErrorReportException(50175, () => this.wrappedObject.Dispose());
        }


        /// <summary>
        /// Dispose unmanaged native resources (InPtr, file handles)
        /// </summary>
        protected virtual void DisposeNativeResources() {
            // Nothing to cleanup
        }

        #endregion
    }
}
