using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.Outputs;

namespace Ca.Roop.TestHarness.Logs {


    /// <summary>Format outgoing data as text before writing to output.</summary>
    class TextLog : Log {


        /// <summary>Constructor.</summary>
        /// <param name="info">Log metadata.</param>
        public TextLog (LogInfo info) : base(info) {
            this.logInfo = info;
        }


        /// <see cref="ILogable.WriteSummaryEntry"/>
        public override bool WriteSummaryEntry(ILogable logable) {
            this.WriteHeader();
            bool ret = this.WriteToOutput(RowBuilderFactory.GetSummaryBuilder(logable, logInfo));
            output.CloseOutput();
            return ret;
        }


        /// <see cref="ILogable.WriteHeader"/>
        public override bool WriteHeader() {
            // If you already have an output that you are going to append, skip the header.
            if (output.Exists() && !logInfo.OutputData.IsOverwrite) {
                output.InitOutput();
                return true;
            }

            output.InitOutput();

            // TODO - Look at moving this around. Does not seem possible at this time.
            // Special case - summary to console is formated entirely differently with 'Header: Value'.
            if (logInfo.IsSummaryLogInfo() && 
                (logInfo.OutputData.Type == OutputType.CONSOLE || logInfo.OutputData.Type == OutputType.EMAIL)) {
                return output.Write("");
            }
            return this.WriteToOutput(RowBuilderFactory.GetHeaderBuilder(this.logInfo));
        }


        /// <see cref="Log.WriteEntry"/>
        protected override bool WriteEntry(Ca.Roop.TestHarness.Core.ITestable testable) {
            return this.WriteToOutput(RowBuilderFactory.GetRowBuilder(testable, logInfo));
        }


        private bool WriteToOutput(IRowBuilder builder) {
            return output.Write(builder.BuildRow());
        }

    }

}
