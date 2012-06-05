using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ca.Roop.TestHarness.Core;
using Ca.Roop.TestHarness.Engine;
using Ca.Roop.TestHarness.Logs.RowBuilders;
using Ca.Roop.TestHarness.TestExceptions;
using Ca.Roop.Utilities;


namespace Ca.Roop.TestHarness.Logs.Sql {

/// <summary>
/// Builds the different SQL statements from initialisation and dynamic data.
/// </summary>
public class SqlBuilder {

    private String  insertTemplate;
    private LogInfo info = null;


    /// <summary>Retrieve the SQL CREATE statement.</summary>
    public String CreateStatement { get; private set; }


    /// <summary>Retrieve the SQL DROP statement.</summary>
    public String DropStatement { get; private set; }


    // Hidden constructor.
    private SqlBuilder() { }


    /// <summary>Constructor.</summary>
    /// <param name="info">The log intialisation data.</param>
    public SqlBuilder(LogInfo info) {
        this.info = info;
        this.BuildStatements();
    }


    /// <summary>
    /// Retrieve the Insert statement template loaded with the dynamic data.
    /// </summary>
    /// <param name="queryable">Object which provides the dynamic data.</param>
    /// <returns>A completed INSERT statement.</returns>
    public String GetInsertStatement(IQueryable queryable) {
        return this.DoReplace(
            this.insertTemplate, 
            "/&", 
            RowBuilderFactory.GetRowBuilder(queryable,info).BuildRow(), 
            "values");
    }


    private void BuildStatements() {
        foreach (StatementTemplate t in this.info.SyntaxData.StatementTemplates) {
            switch (t.Type) {
            case StatementTemplateType.CREATE:
                this.BuildCreateStatement(t.Value, this.info.ColumnDefs);
                break;
            case StatementTemplateType.DROP:
                this.BuildDropStatement(t.Value);
                break;
            case StatementTemplateType.INSERT:
                this.BuildInsertTemplate(t.Value, this.info.ColumnDefs);
                break;
            default:
                throw new InputException(InvalidEnumMessage.Get(t.Type));
            }
        }
    }


    private void BuildCreateStatement(String template, List<ColumnDef> columns) {
        this.CreateStatement = this.InsertTableName(template);
        this.CreateStatement = this.InsertColumnInfo(
            this.CreateStatement, 
            RowBuilderFactory.SqlNameAndTypeColumns(info));
    }


    private void BuildDropStatement(String template) {
        this.DropStatement = this.InsertTableName(template);
    }


    private void BuildInsertTemplate(String template, List<ColumnDef> columns) {
        this.insertTemplate = this.InsertTableName(template);
        this.insertTemplate = 
            this.InsertColumnInfo(this.insertTemplate, RowBuilderFactory.GetHeaderBuilder(info));
    }


    private String InsertColumnInfo(String template, IRowBuilder builder) {
        return this.DoReplace(template, "#", builder.BuildRow(), "field list"); 
    }


    private String InsertTableName(String template) {
        return this.DoReplace(template, "/@", this.GetTableName(), "table");
    }


    private String GetTableName() {
        if (!info.OutputData.IsUniqueName) {
            return info.OutputData.Name;
        }
        return info.OutputData.Name + TestEngine.GetInstance().GetRunId();
    }


    private String DoReplace(String template, String token, String substitution, String target) {
        Regex reg = new Regex("[" + token + "]");
        if (reg.IsMatch(template)) {
            return reg.Replace(template, substitution);
        }
        throw new InputException(
            "Template:" + template 
            + " does not contain the '" + Regex.Replace(token, @"[//]", String.Empty) 
            + "' " + target + " substitution token");
    }

}

}
