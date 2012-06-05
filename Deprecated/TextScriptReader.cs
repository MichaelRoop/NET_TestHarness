///--------------------------------------------------------------------------------------
/// @file	TextScriptReader.h
/// @brief	cross platform and char width file script reader.
///
/// @author		Michael Roop
/// @date		2010
/// @version	1.0
///
/// Copyright 2010 Michael Roop
///--------------------------------------------------------------------------------------
using System.IO;
using System.Text;
using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Core.Test;
using Ca.Roop.Utils;


namespace Ca.Roop.TestHarness.Inputs
{
    ///--------------------------------------------------------------------------------------
    ///
    /// @brief	Class to abstract the reading of a test script from a file.
    ///
    /// Each line in the file represents one test case. As the lines are read in, a 
    /// Info is created that holds the name and arguments of the test case.
    ///
    /// Each line will be broken in two parts, the test name and the arguments string.
    /// The delimiter between name and argument sections is configurable.  The default is
    /// the '$' character.
    ///
    /// The name section is obligatory.  The argument section is optional.
    ///
    /// The arguments portion will be further broken down into name and value pairs.
    ///
    /// The delimiters are configurable between arguments is configurable. The default is 
    /// the '|' character.
    ///
    ///	Each argument will follow this format:
    ///		ArgName=ArgValue.
    ///
    /// If there is no name/arguments delimiter or it is not found then the entire line will 
    /// be taken as the test name.
    ///
    /// If the first non whitespace on a line is '#' it will be considered as a comment or 
    /// inactive test case and be discarded.
    ///--------------------------------------------------------------------------------------
    public class TextScriptReader : IScriptReader {

        private string          m_filename;		///< The file name of the script.
        private FileStream      m_scriptStream;	///< The file object.
        private StreamReader    m_reader;      ///< The file reader.
        private char[]          m_nameDelimiter;///< Name delimiter.
        private char[]          m_argDelimiter;	///< Argument delimiter.

        
        /// @brief	Constructor.
	    ///
	    /// @param	filename	    The file to load the script from.
	    /// @param	nameDelimiter	Delimiter character to tokenize the name and 
	    ///							argument portion of the line.
	    /// @param	argDelimiter	Delimiter to tokenize the argument portion of the
	    ///							line into separate arguments.
	    public TextScriptReader( 
		    string filename,
		    char[] nameDelimiter,
		    char[] argDelimiter
	    ) {
	        m_filename      = filename;
	        m_nameDelimiter = nameDelimiter;
	        m_argDelimiter  = argDelimiter;
        }


	    /// @brief	Opens the file containing the script.
	    ///
	    ///	@throw	Throws a scriptException on empty file name or file not found.
	    public void Open() {
            // todo : check if both file and reader are open. Close if so.
            this.fileAssert(m_filename.Length > 0, "Empty file name" );
            m_scriptStream = new FileStream( m_filename, FileMode.Open );

            m_reader = new StreamReader(m_scriptStream);
            this.fileAssert( m_scriptStream.CanRead, "File not open" );
        }


	    /// @brief	Extracts the test information from the current test script line.
	    ///
	    ///	@throw	Throws a scriptException on syntax failure.
	    ///
	    /// @return	The populated Info for the test.  If the object has no more
	    ///			script lines the Info.isValid() will return false.
	    public Info GetNextTest() {
            this.fileAssert( m_scriptStream != null, "File not initialised" );
            this.fileAssert( m_scriptStream.CanRead, "File not open" );

            Info    info = new Info();
            string  buff = m_reader.ReadLine();

            if (buff.Length > 0) {
                info.SetNull(false);
                this.ProcessLine(info, buff);
            }
            return info;
        }


	    /// @brief	Default constructor in private scope to avoid construction.
	    private TextScriptReader() {}


	    /// @brief	Helper method to process one line of the script file.
	    ///
	    ///	@throw	Throws a scriptException on failure
	    ///
	    /// @param	testInfo	The Info to populate from the script file.
	    /// @param	str			The line as read from the file.
	    private void ProcessLine(Info testInfo, string str) {
            str.Trim();

	        // Check for empty line or line starting with # comment indicator.
	        testInfo.SetActive( str.Length > 0 && str[0] != '#');
	        if (testInfo.IsActive()) {
                Tokenizer t = new Tokenizer(str, m_nameDelimiter);
                if (t.HasToken()) {
                    testInfo.SetId( t.Get() );
                    t++;

			        // It is possible for the test to have only a name and no arguments.
                    if (t.HasToken()) {
                        this.processArgs( testInfo, t.Get() );
                    }
                }
	        }
        }


	    /// @brief	Helper method to process the arguments portion of the script line.
	    ///
	    ///	@throw	Throws a scriptException on failure
	    ///
	    /// @param	testInfo	The Info to populate from the script file.
	    /// @param	args		The args portion of the script line.
        private void processArgs(Info testInfo, string args)
        {
            Tokenizer t = new Tokenizer(args, m_argDelimiter);
            for (; t.HasToken(); t++)
            {
                this.processArg(testInfo, t.Get());
            }
        }


	    /// @brief	Helper method to process one argument string token.
	    ///
	    /// Each argument should be in the format name=value.
	    ///
	    ///	@throw	Throws a scriptException on failure
	    ///
	    /// @param	testInfo	The Info to populate from the script file.
	    /// @param	args		The arg token from the script line.
        private void processArg(Info testInfo, string arg)
        {
            char[] delimiters = {'='};
            Tokenizer t = new Tokenizer(arg, delimiters);
            this.scriptAssert(t.Number() == 2, "Invalid Argument Format", arg);
            string name = t.Get();
            t++;
            // TODO - parse the third part (type)
            testInfo.AddArgument(new Arg(name, t.Get(), ""));
        }


	    /// @brief	Wrapper to clean up checking for exception throw.
	    ///
	    ///	@throw	Throws a scriptException on failure
	    ///
	    /// @param	condition		The condition to test. If false an exception is thrown.
	    /// @param	file			The source code file where problem originates.
	    /// @param	line			The source code line where problem originates.
	    /// @param	msg				The message explaining the exception.
	    /// @param	scriptFileLine	The content of the script line being processed.
	    private void scriptAssert(bool condition, string msg, string scriptLine )
        {
            if (!condition)
            {
                StringBuilder sb = new StringBuilder(100);
                sb.Append("Invalid line in script file:");
                sb.Append(msg);
                sb.Append(scriptLine);
                throw new System.ArgumentException(sb.ToString());
            }
        }


	    /// @brief	Wrapper to clean up checking for exception throw.
	    ///
	    ///	@throw	Throws a fileException on failure
	    ///
	    /// @param	condition		The condition to test. If false an exception is thrown.
	    /// @param	file			The source code file where problem originates.
	    /// @param	line			The source code line where problem originates.
	    /// @param	msg				The message explaining the exception.
	    private void fileAssert(bool condition, string msg)
        {
            if (!condition)
            {
                throw new System.ArgumentException(msg,"Invalid file");
            }
        }

    };

}
