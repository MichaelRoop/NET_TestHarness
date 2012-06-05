///--------------------------------------------------------------------------------------
/// @file	fileOutput.cs
/// @brief	Output class to output logger information to a file.
///
/// @author		Michael Roop
/// @date		2010
/// @version	1.0
///
/// Copyright 2010 Michael Roop
///--------------------------------------------------------------------------------------
using System;
using System.IO;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs;
using Ca.Roop.TestHarness.TestExceptions;


namespace Ca.Roop.TestHarness.Outputs {


/// <summary>Class for outputing logging information to a file.</summary>
public class FileOutput : IOutputable {

    LogInfo         info     = null;
    StreamWriter    writer   = null;
    String          fileName = "";

    private FileOutput() {}


    /// <summary>Constructor.</summary>
    /// <param name="info">Log metadata object.</param>
    public FileOutput(LogInfo info) {
        this.info = info;

        fileName = this.info.OutputData.Name;
        if (this.info.OutputData.IsUniqueName) {
            fileName = fileName + TestEngine.GetInstance().GetRunId();
        }
    }


    /// <see cref="IOutputable.InitOutput."/>
    public bool InitOutput() {
        this.CloseOutput();
        try {
            FileMode mode = this.info.OutputData.IsOverwrite ? FileMode.Create : FileMode.Append;
            FileStream fs = new FileStream(fileName, mode);
            writer        = new StreamWriter(fs);
            writer.AutoFlush = true;
            return fs.CanWrite;
        }
        catch (Exception e) {
            throw new InputException(e);
        }
    }


    /// <see cref="IOutputable.Write."/>
    public bool Write(string str)
    {
        try {
            writer.WriteLine(str);
        }
        catch (Exception e) {
            throw new InputException(e);
        }
        return true;
    }


    /// <see cref="IOutputable.CloseOutput."/>
    public void CloseOutput() {
        try {
            if (writer != null) {
                writer.Close();
            }
        }
        catch (Exception e) {
            throw new InputException(e);
        }
    }


    /// <see cref="IOutputable.Exists."/>
    public bool Exists() {
        return File.Exists(fileName);
    }
    
}

} // end namespace
