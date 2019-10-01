using SpStateMachine.Interfaces;

namespace SpStateMachine.Converters {

    /// <summary>
    /// Implementation of ISpIdConverter to convert from int ids
    /// to a string int. See the samples for a true int to enum
    /// string converter
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public class SpIntToIntConverter : ISpIdConverter {

        #region Static Singleton Support

        // Just used as a shortcut. Should use DI

        private static SpIntToIntConverter instance = null;

        public static SpIntToIntConverter Instance {
            get {
                if (SpIntToIntConverter.instance == null) {
                    SpIntToIntConverter.instance = new SpIntToIntConverter();
                }
                return SpIntToIntConverter.instance;
            }
        }

        #endregion

        #region ISpIdConverter Members

        /// <summary>
        /// Convert the type id to string
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        public string StateId(int id) {
            return id.ToString();
        }


        /// <summary>
        /// Convert the event id to string
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        public string EventId(int id) {
            return id.ToString();
        }


        /// <summary>
        /// Convert the message id to string
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        public string MsgTypeId(int id) {
            return id.ToString();
        }

        #endregion

    }
}
