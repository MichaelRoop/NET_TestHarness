using System;

namespace Ca.Roop.TestHarness.Outputs {

public interface IOutputable {


    /// <summary>Initialise the output object.</summary>
    /// <returns>true if successful, otherwise false.</returns>
    /// <exception cref="InputException" />
	bool InitOutput();


    /// <summary>Close the output object.</summary>
    /// <exception cref="InputException" />
    void CloseOutput();


    /// <summary>Write a string to the output object.</summary>
    /// <returns>true if successful, otherwise false.</returns>
    /// <exception cref="InputException" />
    bool Write(String str);

	
    /// <summary>
    /// Verify whether the output artifact already exists. The output artifact could be something
    /// like a file or SQL table.
    /// </summary>
    /// <returns>true if it exists,, otherwise false.</returns>
    /// <exception cref="InputException" />
    bool Exists();


}


}
