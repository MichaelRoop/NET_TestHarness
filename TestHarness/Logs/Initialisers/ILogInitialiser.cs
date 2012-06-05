
namespace Ca.Roop.TestHarness.Logs.Initialisers {


/// <summary>Interface for various potential log metadata intialisers.</summary>
public interface ILogInitialiser {
	
    /// <summary>Load the log metadata.</summary>
    /// <returns>true if loaded, otherwise false.</returns>
	bool Load();


    /// <summary>Retrieve the next Log's metadata.  If the Log.IsValid returns false then you 
    /// have reached the end of the list.
    /// </summary>
    /// <returns>A Log's metadata or a non valid metadata object</returns>
    LogInfo GetNextLog();

}

}
