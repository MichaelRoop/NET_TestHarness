using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using System.Timers;
using ChkUtils;
using ChkUtils.ErrObjects;

namespace SpStateMachine.PeriodicTimers {

    /// <summary>
    /// Implementation of the IPeriodicTimer that is driven by the 
    /// System.Timers.Timer class
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public sealed class WinSimpleTimer : ISpPeriodicTimer {

        #region Data

        /// <summary>The timer object</summary>
        private Timer timer = null;

        /// <summary>Access lock to the timer object</summary>
        private object timerLock = new object();

        /// <summary>Interval object with default 1 second pulse</summary>
        private TimeSpan timespan = new TimeSpan(0, 0, 1);

        /// <summary>The handler for the timer Elapsed event</summary>
        private ElapsedEventHandler onTimerWakeup = null;

        /// <summary>Disposed flag</summary>
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WinSimpleTimer() {
            this.onTimerWakeup = new ElapsedEventHandler(this.timer_Elapsed);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timespan">Timespan which determines period</param>
        public WinSimpleTimer(TimeSpan timespan)
            : this() {
                this.SetInterval(timespan);
        }

        /// <summary>
        /// Finaliser
        /// </summary>
        ~WinSimpleTimer() {
            this.Dispose(false);
        }

        #endregion
        
        #region IPeriodicTimer Events

        public event Action  OnWakeup;

        #endregion
        
        #region IPeriodicTimer Methods

        /// <summary>
        /// Set the interval in  for wakeup for the next Start
        /// </summary>
        /// <param name="interval">The interval in </param>
        public void SetInterval(TimeSpan interval) {
            WrapErr.ChkDisposed(this.disposed, 50002);
            WrapErr.ChkTrue(interval.TotalMilliseconds > 0, 50000, "The interval cannot be 0 milliseconds total");
            WrapErr.ToErrorReportException(50001, () => {
                this.timespan = interval;
            });
        }


        /// <summary>
        /// Start the periodic timer
        /// </summary>
        public void Start() {
            WrapErr.ChkDisposed(this.disposed, 50003);
            WrapErr.ToErrorReportException(50004, () => {
                lock (this.timerLock) {
                    this.Stop();
                    this.timer = new Timer(this.timespan.TotalMilliseconds);
                    this.timer.Elapsed += this.onTimerWakeup;
                    this.timer.AutoReset = true;
                    this.timer.Start();
                }
            });
        }
        

        /// <summary>
        /// Stop the periodic timer
        /// </summary>
        public void Stop() {
            WrapErr.ChkDisposed(this.disposed, 50005);
            this.DisposeTimer();
        }

        #endregion

        #region IDisposable

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
                    this.DisposeManagedResources();
                }
                this.DisposeNativeResources();
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose managed resources (those with Dispose methods)
        /// </summary>
        private void DisposeManagedResources() {
            this.DisposeTimer();
        }

        /// <summary>
        /// Dispose unmanaged native resources (InPtr, file handles)
        /// </summary>
        private void DisposeNativeResources() {
            // Nothing to cleanup
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Safely dispose of the existing timer
        /// </summary>
        private void DisposeTimer() {
            lock (this.timerLock) {
                if (this.timer != null) {
                    WrapErr.SafeAction(() => this.timer.Stop());
                    WrapErr.SafeAction(() => this.timer.Elapsed -= this.onTimerWakeup);
                    WrapErr.SafeAction(() => this.timer.Dispose());
                    this.timer = null;
                }
            }
        }


        /// <summary>
        /// Registered with the timer to fire on wakeup event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e) {
            // Trap any exceptions using the OnWakeup event handler
            ErrReport err = new ErrReport();
            WrapErr.ToErrReport(out err, 9999, "", () => {
                if (this.OnWakeup != null) {
                    this.OnWakeup();
                }
            });
            // We will have no decision based on results
        }
        
        #endregion

    }
}
