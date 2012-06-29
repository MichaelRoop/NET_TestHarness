using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using ChkUtils;

namespace SpStateMachine.States {
    
    /// <summary>
    /// Base class for an ISpStateMachine that owns the wrapped object and disposes
    /// it on Dispose
    /// </summary>
    /// <typeparam name="T">Wraped object type with IDisposable Interface required</typeparam>
    /// <author>Michael Roop</author>
    public class SpStateMachine<T> : ISpStateMachine where T : IDisposable {

        #region Data

        /// <summary>The main state for the State Machine</summary>
        ISpState state = null;

        /// <summary>The object that the State Machine represents</summary>
        T wrappedObject = default(T);

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
        /// <param name="wrappedObject"></param>
        /// <param name="state">The state machine's main state</param>
        /// <remarks>
        /// The main state will be a super state or parallel super state implementation. You 
        /// can use a single state implementation if you only want the wrapped object to be 
        /// driven by periodic timer and have access to the messaging architecture
        /// </remarks>
        public SpStateMachine(T wrappedObject, ISpState state) {
            this.wrappedObject = wrappedObject;
            this.state = state;
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~SpStateMachine() {
            this.Dispose(false);
        }
        
        #endregion

        #region SpStateMachine

        public ISpMessage Tick(ISpMessage msg) {
            WrapErr.ChkParam(msg, "msg", 9999);
            // TODO - figure out if this will work
            return this.state.OnTick(msg).ReturnMessage;
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
                    WrapErr.SafeAction(() => this.DisposeManagedResources());
                }
                WrapErr.SafeAction(() => this.DisposeNativeResources());
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose managed resources (those with Dispose methods)
        /// </summary>
        protected virtual void DisposeManagedResources() {
            this.wrappedObject.Dispose();
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
