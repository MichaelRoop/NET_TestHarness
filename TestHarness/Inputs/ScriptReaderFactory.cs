
using Ca.Roop.TestHarness.Engine;

namespace Ca.Roop.TestHarness.Inputs {

/// <summary>Factory to build script readers.</summary>
public class ScriptReaderFactory {


    /// <summary>Retrieve the derived test Script Reader based on metadata.</summary>
    /// <param name="info">The metadata for building the script reader.</param>
    /// <returns></returns>
    public static IScriptReader Get(TestSetInfo info) {

        // Removed deprecated ScriptReader type based on initialiser data.  XML reader only for now.
        IScriptReader r = new XmlFileScriptReader(info);
        r.Open();
        return r;
    }

}

}
