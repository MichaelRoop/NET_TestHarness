using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    /// <summary>
    /// Defines an interface for an Enum to Int Converter
    /// </summary>
    /// <author>Michael Roop</author>
    public interface ISpToInt {

        /// <summary>
        /// Convert the Enum to Int
        /// </summary>
        /// <returns></returns>
        int ToInt();
    }

}
