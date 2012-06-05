using System;
using System.Net.Mail;
using System.Text;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs;
using Ca.Roop.TestHarness.TestExceptions;

namespace Ca.Roop.TestHarness.Outputs {


/// <summary>Class for outputing logging information to an email.</summary>
class EmailOutput : IOutputable {

    private String address  ="";
    private int port        = 25;
    private bool isHtml     = true; // TODO set this in crentials
    private String from     = "";
    private String to       = "";
    private String subject  = "";
    private String user     = "";
    private String password = "";
    private bool closed     = false;
    private StringBuilder msg = null;

    private EmailOutput() {}


    /// <summary>Constructor.</summary>
    /// <param name="info">Log metadata object.</param>
    public EmailOutput(LogInfo info) {
        // Process the metadata on construction to fail before any tests are run on critical error.
        this.msg = new StringBuilder(200);
        this.address    = info.OutputData.GetCredential(CredentialType.ADDRESS);
        this.from       = info.OutputData.GetCredential(CredentialType.EMAIL_FROM);
        this.to         = info.OutputData.GetCredential(CredentialType.EMAIL_TO);
        this.subject    = info.OutputData.GetOptionalCredential(CredentialType.EMAIL_SUBJECT, "");
        this.user       = info.OutputData.GetOptionalCredential(CredentialType.USER, "");
        this.password   = info.OutputData.GetOptionalCredential(CredentialType.PASSWORD, "");

        if (this.subject.Length == 0) {
            this.subject = "Test Result " + TestEngine.GetInstance().GetRunId();
        }

        String tmp = info.OutputData.GetOptionalCredential(CredentialType.PORT, "");
        if (tmp.Length > 0) {
            try {
                this.port = Int32.Parse(tmp);
            }
            catch(Exception) {
                throw new InputException(
                    "Cannot convert PORT output credential:" + tmp + " into a port number");
            }
        }
    }


    /// <see cref="IOutputable.InitOutput."/>
    public bool InitOutput() {
        closed = false;
        // Not applicable.
        return true;
    }


    public void CloseOutput() {

        // Normally the CloseOutput can be called redundantly.  Since we are using it to send 
        // everything in one email we need to track the state.
        if (!this.closed) {

            MailMessage mail = new MailMessage(from, to, subject, msg.ToString());
            mail.IsBodyHtml = isHtml;

            SmtpClient smtp = new SmtpClient(address, port);

            // TODO What are the DefaultNetworkCredentials?
            //smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            smtp.UseDefaultCredentials = false;
            if (this.user.Length == 0 && this.password.Length == 0) {
                // Since UseDefaultCredentials is set fault it will send anonymously.
            }
            else {
                // TODO Add option for the Domain argument.
                smtp.Credentials = new System.Net.NetworkCredential(user, password);
            }

            try {
                // We cannot us SendAsync to avoid the send being aborted by the app ending.
                smtp.Send(mail);
                this.closed = true;
                this.msg.Remove(0, this.msg.Length);
            }
            catch (Exception e) {
                throw new InputException("Failed to send email. Details:" + e.Message);
            }
        }
    }


    public bool Write(string str) {
        msg.Append(str);
        msg.Append(this.isHtml ? "<br>" : "\n\r");
        return true;
    }


    public bool Exists() {
        // Not applicable.
        return true;
    }

}

}
