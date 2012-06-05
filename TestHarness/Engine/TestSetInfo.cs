using System;

namespace Ca.Roop.TestHarness.Engine {


/// <summary>Contains the metadata necessary to run a set of tests.</summary>
public class TestSetInfo {


    /// <summary>Default constructor.</summary>
    public TestSetInfo() {
        this.IsValid        = false;
        this.AssemblyName   = "";
        this.ClassName      = "";
        this.ConfigFile     = "";
        this.ScriptFile     = "";
    }


    /// <summary>Constructor.</summary>
    /// <param name="assemblyName">DLL which contains the test set.</param>
    /// <param name="className">The class name in the Assembly derived from TestRegistrar.</param>
    /// <param name="configFile">The log config file for the test run.</param>
    /// <param name="scriptFile">The script file with the test cases and their parameters.</param>
    public TestSetInfo(
        String assemblyName,
        String className,
        String configFile,
        String scriptFile ) {

        this.IsValid        = true;
        this.AssemblyName   = assemblyName;
        this.ClassName      = className;
        this.ConfigFile     = configFile;
        this.ScriptFile     = scriptFile;
    }


    /// <summary>The valid or non valid state of the test set.</summary>
    public bool IsValid { get; private set; }


    /// The assembly name which is the path and file name of the assembly DLL
    public String AssemblyName { get; private set; }


    /// <summary>The class name to invoke from the assembly.</summary>
    public String ClassName { get; private set; }


    /// <summary>The log config file name associated with the named test set.</summary>
    public String ConfigFile { get; private set; }


    /// <summary>The test script file name associated with this test set.</summary>
    public String ScriptFile { get; private set; }

}

}
