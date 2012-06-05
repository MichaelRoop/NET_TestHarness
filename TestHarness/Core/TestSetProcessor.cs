using System;
using System.IO;
using System.Reflection;
using System.Text;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Inputs;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.Utilities;
using System.Security.Permissions;

namespace Ca.Roop.TestHarness.Core {

/// <summary>Loads a list of test case libraries and executes them.</summary>
public class TestSetProcessor {

	private static String   registerMethodName = "RegisterTestSet";
	private String          fileName;

	
    /// <summary>Hidden default constructor.</summary>
	private TestSetProcessor() {}


    /// <summary>Constructor.</summary>
    /// <param name="fileName">Name of the file containing the test set definitions.</param>
	public TestSetProcessor(String fileName) {
		this.fileName = fileName;
	}	


    /// <summary>Process the list of sets.</summary>
    public void ProcessSets() {
        ITestSetInitialiser init = TestSetInitialiserFactory.Get(this.fileName);
        TestEngine e = TestEngine.GetInstance();

        TestSetInfo info = init.GetNextSet();
        while (info.IsValid) {
            this.LoadTestSetAssembly(info);
            e.GetLogEngine().loadLoggers(info);
            e.ProcessScript(ScriptReaderFactory.Get(info));
            e.PurgeAll();
            info = init.GetNextSet();
        }
    }
	
	
    /// <summary>
    /// Dynamically loads the IRegisterable class that initializes all the tests in a test set. 
    /// </summary>
    /// <param name="info">Object with the test set information for one test set.</param>
    /// <exception cref="InputException" />
    public void LoadTestSetAssembly(TestSetInfo info) {
        TryHelper.WrapToInputException(delegate() {
            // Load from fixed file and path. 

            //FileIOPermission permission =
            //    new FileIOPermission(FileIOPermissionAccess.Read,info.AssemblyName);
            //permission.Demand();

            Assembly assembly = Assembly.LoadFrom(info.AssemblyName);
            Type type = assembly.GetType(info.ClassName, true);

            type.InvokeMember(
                registerMethodName,
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                Activator.CreateInstance(type, true),
                new object[] { }
            );

        });
    }
	
	
	private void DoThrow(String className, Exception e) {
        this.DoThrow("", className, e);
    }
	
	
	private void DoThrow(String msg, String className, System.Exception e) {
		StringBuilder sb = new StringBuilder();
            sb.Append(e.GetType().Name)
            .Append("\n")
            .Append(msg)
            .Append("\n")
            .Append("loading class:")
            .Append(className);
        throw new InputException(sb.ToString(), e.InnerException);
    }

}


}
