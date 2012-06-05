
namespace Ca.Roop.TestHarness.Core {
    
public interface IRegisterable {

    /// <summary>
    /// Override this method in your implementation class to register all the tests and other
    /// IRegisterable objects that contain test registrations.
    /// <remarks>
    /// Only those registered can be invoked by script in the TestEngine.  The method
    /// will be invoked after dynamically loading the derived class and creating an instance. 
    /// <p>
    /// You can add tests directly by calling:
    /// this.RegisterTest(new TestCaseDerivedTest());
    /// <p>
    /// Or add other IRegisterable objects so that their tests and IRegisterable objects are added.
    /// This allows a hierarchical addition of tests to the test set.
    /// this.RegisterRegistrationObject(new IRegisterableDerivedObject());
    /// </remarks>
    /// </summary>
    void RegisterTestSet();


    /// <summary>Register one test.</summary>
    /// <param name="testable">The test to register.</param>
    void RegisterTest(ITestable testable);


    /// <summary>
    /// Register another IRegisterable object to allow hierarchical registration of tests. 
    /// </summary>
    /// <param name="registerable">The IRegisterable object to register.</param>
    void RegisterRegistrationObject(IRegisterable registerable);

}

}
