///--------------------------------------------------------------------------------------
/// @file	TextScriptReader.cs
/// @brief	script reader for dev test platform.
///
/// @author		Michael Roop
/// @date		2010
/// @version	1.0
///
/// Copyright 2010 Michael Roop
///--------------------------------------------------------------------------------------
using Ca.Roop.TestHarness.Core.Test;
using Ca.Roop.TestHarness.Inputs;

namespace Ca.Roop.TestHarness.Inputs
{

    /// <summary>
    /// Interface to abstract the reading of a test script. The test script contains the 
    /// test script contains the id and arguments for each test case to run.
    /// </summary>
    /// <remarks>
    /// The derived classes can read a script file of any format from any source as long
    /// as they satisfy the interface.  In this case you could have a custom format of script
    /// file or one that used known formats such as INI or XML.
    /// </remarks>
    public interface IScriptReader
    {

        /// <summary>Opens the script.</summary>
        void Open();


        /// <summary>
        /// Extracts the metadata for the next test case to run.  If the Info.IsValid returns 
        /// false it is a non valid object indicating end of tests..
        /// </summary>
        /// <returns>The metadata to run the next test case.</returns>
        TestInfo GetNextTest();
    };

}
