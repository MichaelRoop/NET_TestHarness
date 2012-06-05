
using Ca.Roop.TestHarness.Engine;


namespace Ca.Roop.TestHarness.Core {

/// <summary>Abstract base class to register a set of tests to run from a loaded assembly.</summary>
public abstract class TestRegistrar : IRegisterable {

    /// <summary>Abstract method that is the invokation call from the assembly.  All of your 
    /// test cases and other IRegisterable classes are registered in the derived method by 
    /// calls to RegisterTest and RegisterRegistration Oject.
    /// 
    /// </summary>
    public abstract void RegisterTestSet();


	public void RegisterTest(ITestable testable) {
		TestEngine.GetInstance().RegisterCase(testable);
	}


    public void RegisterRegistrationObject(IRegisterable registerable) {
        registerable.RegisterTestSet();
	}

}

}
