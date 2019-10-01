using SpStateMachine.Interfaces;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Definition of a class to satisfy the ISpToInt but act as non converter.
    /// This allows the use of the State Machine architecture and satisfy the
    /// argument requirements while just being a passthrough
    /// parameter
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpIntToInt : ISpToInt {

        #region Data

        /// <summary>Int value passed in on construction</summary>
        private int val;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor in private scope to prevent usage
        /// </summary>
        private SpIntToInt() { 
        }
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="val">The int value to pass throug</param>
        public SpIntToInt(int val) {
            this.val = val;
        }

        #endregion

        #region ISpToInt Members

        /// <summary>
        /// Pass through the held int
        /// </summary>
        /// <returns></returns>
        public int ToInt() {
            return this.val;
        }

        #endregion

    }
}
