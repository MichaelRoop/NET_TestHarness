
using System;
using Ca.Roop.TestHarness.TestExceptions;

namespace Ca.Roop.Utilities {

/// <summary>
/// 
/// </summary>
public class TryHelper {


    /// <summary>
    /// Wraps a method with one parameter in a try and catch block.  It will return the default 
    /// value of the TParam on throw, otherwise the return value of the 
    /// </summary>
    /// <typeparam name="TParam">The methods parameter type.</typeparam>
    /// <typeparam name="TReturn">The methods return type.</typeparam>
    /// <param name="action">Method with one parameter of type TParam and return value of TReturn.</param>
    /// <param name="arg1">Method parameter of type TParam.</param>
    /// <returns>The default value of TReturn on throw or the result of the action.</returns>
    public static TReturn EatException<TParam, TReturn>(Func<TParam, TReturn> action, TParam arg1) {
        TReturn ret = default(TReturn);
        try {
            ret = action(arg1);
        }
        catch (Exception) {
            // Eat exception.
        }
        return ret;
    }


    /// <summary>
    /// Wraps a method or code block in a try, catches the exception and repackages it as an 
    /// InputException.
    /// </summary>
    /// <param name="action">The method or code block to wrap.</param>
    public static void WrapToInputException(Action action) {
        try {
            action.Invoke();
        }
        catch (Exception e) {
            throw new InputException(e.GetType().Name + "\n" + e.Message, e.InnerException);
        }
    }


    /// <summary>
    /// Wraps a method or code block in a try, catches the exception and eats it.
    /// </summary>
    /// <param name="action">The method or code block to wrap.</param>
    public static void EatException(Action action) {
        try {
            action.Invoke();
        }
        catch (Exception) {
            // Eat exception.
        }
    }


}

}
