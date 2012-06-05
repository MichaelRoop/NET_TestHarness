using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.Logs;


namespace Ca.Roop.TestHarness.Core
{
    /// <summary>
    /// Interface to allow disparate objects to be queried in a common manner.
    /// </summary>
    public interface IQueryable {

        /// <summary>
        /// Get the value, converted to string, associated with the columnId (also refered to as 
        /// the column header name).
        /// </summary>
        /// <param name="columnDef">Definition for a particular column</param>
        /// <param name="info">General information on columns</param>
        /// <returns>The Value associated with the key</returns>
        string GetValue(ColumnDef columnDef, LogInfo info);
    }
}
