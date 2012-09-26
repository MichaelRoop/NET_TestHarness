
namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Defines an interface to pass an Enum convertible to Int
    /// </summary>
    /// <remarks>This allows strong typing of int type arguments as Enum</remarks>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
    public interface ISpToInt {

        /// <summary>
        /// Convert the Enum to Int
        /// </summary>
        /// <returns></returns>
        int ToInt();

    }

}
