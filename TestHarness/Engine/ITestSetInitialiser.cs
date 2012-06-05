
namespace Ca.Roop.TestHarness.Engine {

/// <summary>
/// Interface to create appropriate initialiser for a test set.
/// </summary>
public interface ITestSetInitialiser {

    /// <summary>
    /// Retrieves the next TestSetInfo. Returns TestSetInfo.IsValid() == false on end of list.
    /// </summary>
    /// <returns></returns>
    TestSetInfo GetNextSet();
}

}
