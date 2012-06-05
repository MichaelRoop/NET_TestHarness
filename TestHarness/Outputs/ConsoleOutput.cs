///--------------------------------------------------------------------------------------
/// @file	console.cs
/// @brief	Output class to output logger information to the console.
///
/// @author		Michael Roop
/// @date		2010
/// @version	1.0
///
/// Copyright 2010 Michael Roop
///--------------------------------------------------------------------------------------
using System;
using System.IO;


namespace Ca.Roop.TestHarness.Outputs {

/// <summary>Class for outputing logging information to the console.</summary>
public class ConsoleOutput : IOutputable {

    private StreamWriter writer = null;


    /// <see cref="IOutputable.InitOutput."/>
    public bool InitOutput() {
        writer = new StreamWriter(Console.OpenStandardOutput());
        writer.AutoFlush = true;
        return true; 
    }


    /// <see cref="IOutputable.CloseOutput."/>
    public void CloseOutput() {
        if (writer != null) {
            writer.Close();
        }
    }

    /// <see cref="IOutputable.Write"/>
    public bool Write(string str) {
        writer.WriteLine(str);
        return true;
    }


    /// <see cref="IOutputable.Exists."/>
    public bool Exists() {
        // Not applicable.
        return false;
    }

};

}

