using System;
using System.Collections.Generic;

namespace ChkUtils.Net.Interfaces {

    /// <summary>Interface to calls that must be compiled in OS specific libraries for parsing the stack</summary>
    public interface IStackTools {

        /// <summary>Walk through stack until first class that is not to be ignored and whose method does not have the <>
        /// </summary>
        /// <param name="typeToIgnore">Type to ignore as you travel the stack</param>
        /// <returns>The class containing class and method information</returns>
        ErrorLocation FirstNonWrappedMethod(Type typeToIgnore);


        /// <summary>Walk through stack until you encounter the first class that is not to be ignored</summary>
        /// <param name="typesToIgnore">Types to ignore as you travel the stack</param>
        /// <returns>The class containing class and method information</returns>
        ErrorLocation FirstNonWrappedMethod(Type[] typesToIgnore);


        /// <summary>Returns list with the stack trace except the leading entries which are to be ignored</summary>
        /// <param name="typeToIgnore">The type to ignore on first entries</param>
        /// <param name="fromLevel">The stack level to start at</param>
        /// <returns>A list with entries past the initial wraping type class</returns>
        List<string> FirstNonWrappedTraceStack(Type typeToIgnore, int fromLevel);


        /// <summary>Returns list with the stack trace except the leading entries which are to be ignored</summary>
        /// <param name="typeToIgnore">The type to ignore on first entries</param>
        /// <param name="ex">Exception to trace from</param>
        /// <param name="fromLevel">The stack level to start at</param>
        /// <returns>A list with entries past the initial wraping type class</returns>
        List<string> FirstNonWrappedTraceStack(Type typeToIgnore, Exception ex, int fromLevel);


        /// <summary>Drill down through inner exceptions to find inner Exception type T</summary>
        /// <typeparam name="T">The exception type to find</typeparam>
        /// <param name="e">The top level exception</param>
        /// <param name="onComplete">
        /// invoked when drill down is complete. If found the first parameter will be set true and the second 
        /// parameter will be the exception
        /// </param>
        void FindNestedExceptionType<T>(Exception e, Action<bool, T> onComplete) where T : Exception;
    }
}
