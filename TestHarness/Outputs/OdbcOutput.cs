using System.Data.SqlClient;
using System.Text;
using Ca.Roop.TestHarness.Logs;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.Utilities;


namespace Ca.Roop.TestHarness.Outputs {

/// <summary>Implements the IOutputable as an ODBC compliant output.</summary>
public class OdbcOutput : IOutputable {

    SqlConnection   connection          = null;	
    LogInfo         info                = null;

    // Hidden in private scope.
    private OdbcOutput() { }


    /// <summary>Constructor.</summary>
    /// <param name="info">Log info used to intialise the output.</param>
    public OdbcOutput(LogInfo info) {
        this.info = info;
    }


    /// <see cref="IOutputable.InitOutput."/>
    public bool InitOutput() {
        // TODO - look at modifying so that we can load any combination of possible credentials.
        TryHelper.WrapToInputException(delegate() {
            connection = new SqlConnection(
                this.info.OutputData.GetCredential(CredentialType.CONNECTION_STRING));
            connection.Open();
        });
        return true;
    }


    /// <see cref="IOutputable.CloseOutput."/>
    public void CloseOutput() {
        TryHelper.EatException(delegate() {
            if (this.connection != null) {
                this.connection.Close();
            }
        });
    }


    /// <see cref="IOutputable.Write."/>
    public bool Write(string str) {
        try {
            using (SqlCommand command = new SqlCommand(str,connection)) {
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }
        catch (SqlException e) {
           StringBuilder sb = new StringBuilder();
            sb.Append("\n").Append("Faild SQL statement:").Append(str).Append("\n")
                .Append(e.Message);
			throw new InputException(sb.ToString(), e.InnerException);
		}
        return true;
    }


    /// <see cref="IOutputable.Exists."/>
    public bool Exists() {
        // Ignore for ODBC. Do DROP against a non existent table or CREATE on an existing 
        // one providing you catch and eat the exception.
        return false;
    }

}

}
