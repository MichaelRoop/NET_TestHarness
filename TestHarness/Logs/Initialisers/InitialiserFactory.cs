
using Ca.Roop.TestHarness.Engine;

namespace Ca.Roop.TestHarness.Logs.Initialisers {


/// <summary>Factory class to abstract the creation of ILogInitialiser objects.</summary>
public class InitialiserFactory {

    /// <summary>
    /// Creates the appropriate ILogInitialiser derived object.
    /// </summary>
    /// <param name="info">The log metadata object.</param>
    /// <returns>An constructed ILogInitialiser derived object.</returns>
	public static ILogInitialiser Create(TestSetInfo info) {
        // Removed deprectated intialisers. Only XML remains.
        return new XmlLogInitialiser(info);
    }

}

}
