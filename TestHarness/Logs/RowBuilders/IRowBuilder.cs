using System;

namespace Ca.Roop.TestHarness.Logs.RowBuilders {


/// <summary>Interface to the row builder hierarchy of classes.</summary>
public interface IRowBuilder {

    /// <summary>Builds a row of data.</summary>
    /// <returns>A string representing the built row.</returns>
    String BuildRow();

}

}
