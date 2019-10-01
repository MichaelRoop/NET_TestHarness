
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Defines an interface to pass an int convertible to Enum
    /// </summary>
    /// <remarks>This allows strong typing of int type values as Enum</remarks>
    /// <author>Michael Roop</author>
    /// <typeparam name="T">
    /// The type of Enum to convert. The struct constraint forces the type to 
    /// be non nullable.  The implementation has to do the checking if Enum
    /// </typeparam>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpToEnum<T> where T : struct {

        /// <summary>
        /// Convert to Enum of type T
        /// </summary>
        /// <returns>Returns a type T enum</returns>
        T ToEnum();

    }

}
