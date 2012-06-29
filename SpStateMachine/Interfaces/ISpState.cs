
using System.Collections.Generic;
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Interface for defining the state interface
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpState {

        #region Properties

        /// <summary>
        /// Queries if OnEntry has already been invoked. It can only
        /// be invoked once until the OnExit is called
        /// </summary>
        bool IsEntryExcecuted { get; }

        /// <summary>
        /// The unique state identifier
        /// </summary>
        int Id { get; }


        /// <summary>
        /// Get the full id by combining nested ids
        /// </summary>
        List<int> IdChain { get; }

        /// <summary>
        /// Get the fully resolved state name in format
        /// parent.parent.state
        /// </summary>
        string Name { get; }


        #endregion

        #region Methods

        /// <summary>
        /// Excecuted once when the state becomes the current state
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnEntry(ISpMessage msg);


        /// <summary>
        /// Called on every other period after the first
        /// </summary>
        /// <param name="msg">The incoming message</param>
        /// <returns>A state transition object</returns>
        ISpStateTransition OnTick(ISpMessage msg);


        /// <summary>
        /// Always invoked on object exit
        /// </summary>
        void OnExit();

        #endregion

    }
}
