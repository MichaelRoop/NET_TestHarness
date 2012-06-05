using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Ca.Roop.TestHarness.TestExceptions;


namespace Ca.Roop.TestHarness.Xml {



public class XmlExceptionProcessor {

    public static TReturn WrapTry<TReturn>(Func<TReturn> func) {
        try {
            return func.Invoke();
        }
        catch (FileNotFoundException e) {
            throw new InputException(FormattedMsg(e), e);
        }
        catch (XmlSchemaValidationException e) {
            throw new InputException(FormattedMsg(e, e.SourceUri, e.LineNumber, e.LinePosition), e);
        }
        catch (XmlSchemaException e) {
            throw new InputException(FormattedMsg(e, e.SourceUri, e.LineNumber, e.LinePosition), e);
        }
        catch (XmlException e) {
            throw new InputException(FormattedMsg(e, e.SourceUri, e.LineNumber, e.LinePosition), e);
        }
        catch (Exception e) {
            throw new InputException(FormattedMsg(e),e);
        }
    }


    private static String FormattedMsg(Exception e) {
        return new 
            StringBuilder().Append(e.GetType().Name).Append("\n").Append(e.Message).ToString();
    }



    private static String FormattedMsg(Exception e, String uri, int line, int position) {
        return new StringBuilder()
                .Append(e.GetType().Name).Append("\n")
                .Append(uri).Append("\n")
                .Append(e.Message).Append("\n")
                .Append("Line ").Append(line).Append(" position ").Append(position).ToString();
    }
}

}
