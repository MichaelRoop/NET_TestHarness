
namespace SpStateMachine.Interfaces {

    /// <summary>Defines interface to pass an Enum convertible to Int/// </summary>
    /// <remarks>Allows strong typing of int type arguments as Enum</remarks>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public interface ISpToInt {

        /// <summary>Convert the Enum to Int</summary>
        /// <returns>The integer value of the enum identifier</returns>
        int ToInt();

    }

}
