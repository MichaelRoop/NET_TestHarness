using System;

namespace Ca.Roop.TestHarness.Engine {

/// <summary>
/// Factory to create appropriate initialiser for test sets.
/// </summary>
public class TestSetInitialiserFactory {

    /// <summary>Constructor.</summary>
    /// <param name="fileName">The file with the initialization data</param>
    /// <exception cref="InputException" />
    /// <returns>The properly derived initializer object.</returns>
    public static ITestSetInitialiser Get(String fileName) {

        // At this point we only have the Xml intialiser. Deprecated INI intialiser removed.
        return new XmlTestSetInitialiser(fileName);
	}

}

}
