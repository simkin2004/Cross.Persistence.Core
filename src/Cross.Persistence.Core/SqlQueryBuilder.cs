// <copyright file="SqlQueryBuilder.cs" company="Chris Trout">
// MIT License
//
// Copyright(c) 2019 Chris Trout
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// </copyright>

namespace Cross.Persistence.Core
{
    using Cross.Core;
    using Cross.Core.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides a Builder to create SELECT, INSERT, UPDATE, and DELETE queries.
    /// </summary>
    /// <remarks>
    /// The default implementation works for Microsoft Sql Server.  Other database dialects need override parts of this implementation.
    /// </remarks>
    public class SqlQueryBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryBuilder" /> class.
        /// </summary>
        public SqlQueryBuilder()
        {
            // no op
        }

        /// <summary>
        /// Gets a list of the valid fields for this query.
        /// </summary>
        public IList<string> AvailableFields { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the SQL Statement format for DELETE statements.
        /// </summary>
        /// <remarks>
        /// <para>{0} is the Table Name.</para>
        /// <para>{1} is the WHERE Clause (filters) without WHERE keyword or leading spaces.</para>
        /// </remarks>
        public virtual string DeleteStatementFormat { get; } = "DELETE FROM {0}{1};";

        /// <summary>
        /// Gets the parameter name to use for ending row number for SELECT statements.
        /// </summary>
        public string EndingRowNumberParameterName { get; private set; } = "endingRowNumber";

        /// <summary>
        /// Gets the filter fields and the values required for the WHERE clause in SELECT, UPDATE, and DELETE statements.
        /// </summary>
        public IDictionary<string, object> Filters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the flag indicating whether or not the ORDER BY when constructing the SELECT command.
        /// </summary>
        /// <remarks>The default implementation (Microsoft SQL Server) does not require an ORDER BY clause in the SELECT statement.</remarks>
        public virtual bool IncludeOrderByClause { get; } = false;

        /// <summary>
        /// Gets the SQL Statement format for INSERT statements.
        /// </summary>
        /// <remarks>
        /// <para>{0} is the Table Name.</para>
        /// <para>{1} is the Field List from the Table.</para>
        /// <para>{2} is the Parameter List to set values in the Table.</para>
        public virtual string InsertStatementFormat { get; } = "INSERT INTO {0} ({1}) VALUES ({2});";

        /// <summary>
        /// Gets the format required by the database engine for parameters (eg "@{0}" for Microsoft SQL Server, ":{0}" for Oracle, "?" for OLEDB).
        /// </summary>
        /// <remarks>
        /// This class will use "@{0}" as the default value. Any database that does not support this parameter format should override this property.
        /// </remarks>
        public virtual string ParameterFormat { get; } = "@{0}";

        /// <summary>
        /// Gets the flag indicating whether or not Sort Order is required when constructing the SELECT command.
        /// </summary>
        /// <remarks>The default implementation (Microsoft SQL Server) requires at least one sort order in the SELECT command.
        public virtual bool RequireSortOrder { get; } = true;

        /// <summary>
        /// Gets the format for the SELECT command.
        /// </summary>
        /// <remarks>
        /// <para>{0} is the Table Name.</para>
        /// <para>{1} is the Parameter List.</para>
        /// <para>{2} is the WHERE Clause (filters) without WHERE keyword or leading spaces.</para>
        /// <para>{3} is the StartingRowNumberParameterName.</para>
        /// <para>{4} is the EndingRowNumberParameterName.</para>
        /// <para>{5} is the Sort Order.</para>
        /// <para>Every row should include a parameter named 'row_count' with the total number of records in the overall set.</para>
        /// </remarks>
        public virtual string SelectStatementFormat { get; } = "SELECT {1} FROM"
            + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
            + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";

        /// <summary>
        /// Gets the fields and what direction it should be ordered.
        /// </summary>
        public IDictionary<string, SortDirection> SortOrder { get; private set; } = new Dictionary<string, SortDirection>();

        /// <summary>
        /// Gets the name of the Starting Row Number Parameter.
        /// </summary>
        public string StartingRowNumberParameterName { get; private set; } = "startingRowNumber";

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Gets the fields that will be updated in an UPDATE statement.
        /// </summary>
        public IDictionary<string, object> UpdateFields { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the format for the UPDATE statement.
        /// </summary>
        /// <remarks>
        /// <para>{0} is the Table Name.</para>
        /// <para>{1} is the WHERE Clause (filters) without WHERE keyword or leading spaces.</para>
        public virtual string UpdateStatementFormat { get; } = "UPDATE {0} SET {1}{2};";

        /// <summary>
        /// Changes the fields that are available in this <see cref="TableName" />.
        /// </summary>
        /// <param name="availableFields">the fields available in this <see cref="TableName" />.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddAvailableFields(IList<string> availableFields)
        {
            this.AvailableFields = availableFields ?? throw new ArgumentNullException(nameof(availableFields));

            if (availableFields.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(availableFields), "AvailableFields must contain at least 1 field.");
            }

            return this;
        }

        /// <summary>
        /// Changes the Ending Row Number Parameter Name.
        /// </summary>
        /// <param name="endingRowNumberParameterName">the name of the Ending Row Number parameter.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddEndingRowNumberParameterName(string endingRowNumberParameterName)
        {
            this.EndingRowNumberParameterName = endingRowNumberParameterName ?? throw new ArgumentNullException(nameof(endingRowNumberParameterName));

            if (string.IsNullOrWhiteSpace(endingRowNumberParameterName))
            {
                throw new ArgumentWhiteSpaceException(nameof(endingRowNumberParameterName));
            }

            return this;
        }

        /// <summary>
        /// Changes the filters for SELECT, UPDATE, and DELETE commands.
        /// </summary>
        /// <param name="filters">A list of fields and values that the SELECT, UPDATE, or DELETE command is using.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddFilters(IDictionary<string, object> filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            this.Filters = new Dictionary<string, object>(filters, StringComparer.CurrentCultureIgnoreCase);
            return this;
        }

        /// <summary>
        /// Changes the SortOrder for SELECT commands.
        /// </summary>
        /// <param name="sortOrder">A list of fields and the direction in which they will be sorted.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddSortOrder(IDictionary<string, SortDirection> sortOrder)
        {
            if (sortOrder == null)
            {
                throw new ArgumentNullException(nameof(sortOrder));
            }

            this.SortOrder = new Dictionary<string, SortDirection>(sortOrder, StringComparer.CurrentCultureIgnoreCase);
            return this;
        }

        /// <summary>
        /// Changes the Starting Row Number Parameter in SELECT statements.
        /// </summary>
        /// <param name="startingRowNumberParameterName">the name of the starting row parameter.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddStartingRowNumberParameterName(string startingRowNumberParameterName)
        {
            this.StartingRowNumberParameterName = startingRowNumberParameterName ?? throw new ArgumentNullException(nameof(startingRowNumberParameterName));

            if (string.IsNullOrWhiteSpace(startingRowNumberParameterName))
            {
                throw new ArgumentWhiteSpaceException(nameof(startingRowNumberParameterName));
            }

            return this;
        }

        /// <summary>
        /// Changes the table name.
        /// </summary>
        /// <param name="tableName">the name of the table.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddTableName(string tableName)
        {
            this.TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentWhiteSpaceException(nameof(tableName));
            }

            return this;
        }

        /// <summary>
        /// Change the field values to be updated in the UPDATE command.
        /// </summary>
        /// <param name="updateFields">The field values to be updated.</param>
        /// <returns>A <see cref="SqlQueryBuilder" /> instance.</returns>
        public SqlQueryBuilder AddUpdateFields(IDictionary<string, object> updateFields)
        {
            if (updateFields == null)
            {
                throw new ArgumentNullException(nameof(updateFields));
            }

            if (updateFields.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(updateFields), "UpdateFields must contain at least 1 field.");
            }

            this.UpdateFields = new Dictionary<string, object>(updateFields, StringComparer.CurrentCultureIgnoreCase);
            return this;
        }

        /// <summary>
        /// Builds a DELETE command that is ready for execution.
        /// </summary>
        /// <returns>A DELETE SQL command.</returns>
        public string BuildDeleteCommand()
        {
            if (string.IsNullOrWhiteSpace(this.TableName))
            {
                throw new InvalidOperationException("TableName must be specified to build a DELETE command.");
            }

            if (this.AvailableFields.Count == 0)
            {
                throw new InvalidOperationException("AvailableFields must be specified to build an DELETE command.");
            }

            var invalidFields = this.ValidateFields(this.Filters.Keys);

            if (invalidFields.Count() > 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "Filters contains the following invalid field names: {0}.", string.Join(", ", invalidFields)));
            }

            string filters = this.BuildParameterList(ParameterListKind.IncludeWhereClause);

            string result = string.Format(CultureInfo.CurrentCulture, this.DeleteStatementFormat, this.TableName, filters);

            return result;
        }
        
        /// <summary>
        /// Builds an INSERT command that is ready for execution.
        /// </summary>
        /// <returns>An INSERT SQL command.</returns>
        public string BuildInsertCommand()
        {
            if (string.IsNullOrWhiteSpace(this.TableName))
            {
                throw new InvalidOperationException("TableName must be specified to build an INSERT command.");
            }

            if (this.AvailableFields.Count == 0)
            {
                throw new InvalidOperationException("AvailableFields must be specified to build an INSERT command.");
            }

            string fieldList = string.Join(", ", this.AvailableFields);
            
            // Transform field names to parameter names.
            string parameterList = string.Join(", ", this.AvailableFields.Select(x => string.Format(CultureInfo.CurrentCulture, this.ParameterFormat, x.ToCamelCase())));

            string result = string.Format(CultureInfo.CurrentCulture, this.InsertStatementFormat, this.TableName, fieldList, parameterList);

            return result;
        }

        /// <summary>
        /// Builds a SELECT command that is ready for execution.
        /// </summary>
        /// <returns>A SELECT SQL command.</returns>
        public string BuildSelectCommand()
        {
            if (string.IsNullOrWhiteSpace(this.TableName))
            {
                throw new InvalidOperationException("TableName must be specified to build a SELECT command.");
            }

            if (this.AvailableFields.Count == 0)
            {
                throw new InvalidOperationException("AvailableFields must be specified to build a SELECT command.");
            }

            if (this.RequireSortOrder && this.SortOrder.Count == 0)
            {
                throw new InvalidOperationException("SortOrder must be specified to build a SELECT command.");
            }

            bool previousError = false;
            var builder = new StringBuilder();
            var invalidFields = this.ValidateFields(this.Filters.Keys);
            
            if (invalidFields.Count() > 0)
            {
                builder.AppendFormat(CultureInfo.CurrentUICulture, "Filters contains the following invalid field names: {0}.", string.Join(", ", invalidFields));
                previousError = true;
            }

            var invalidSortFields = this.ValidateFields(this.SortOrder.Keys);
            if (invalidSortFields.Count() > 0)
            {
                if (previousError)
                {
                    builder.Append("\r\n");
                }
                builder.AppendFormat(CultureInfo.CurrentUICulture, "SortOrder contains the following invalid field names: {0}.", string.Join(", ", invalidSortFields));
                previousError = true;
            }

            if (previousError)
            {
                throw new InvalidOperationException(builder.ToString());
            }

            string parameters = string.Join(", ", this.AvailableFields);
            string filter = BuildParameterList(ParameterListKind.IncludeLeadingAnd);
            string sortOrder = BuildSortOrder();
            string start = string.Format(CultureInfo.CurrentCulture, this.ParameterFormat, this.StartingRowNumberParameterName);
            string end = string.Format(CultureInfo.CurrentCulture, this.ParameterFormat, this.EndingRowNumberParameterName);
            string result = string.Format(CultureInfo.CurrentCulture, this.SelectStatementFormat, this.TableName, parameters, filter, start, end, sortOrder);

            return result;
        }
       
        /// <summary>
        /// Builds an UPDATE SQL command that is ready for execution
        /// </summary>
        /// <returns>An UPDATE SQL command.</returns>
        public string BuildUpdateCommand()
        {
            if (string.IsNullOrWhiteSpace(this.TableName))
            {
                throw new InvalidOperationException("TableName must be specified to build an UPDATE command.");
            }

            if (this.AvailableFields.Count == 0)
            {
                throw new InvalidOperationException("AvailableFields must be specified to build an UPDATE command.");
            }

            var builder = new StringBuilder();
            bool previousError = false;

            var invalidFields = this.ValidateFields(this.Filters.Keys);
            if (invalidFields.Count() > 0)
            {
                builder.AppendFormat(CultureInfo.CurrentUICulture, "Filters contains the following invalid field names: {0}.", string.Join(", ", invalidFields));
                previousError = true;
            }

            var invalidUpdateFields = this.ValidateFields(this.UpdateFields.Keys);
            if (invalidUpdateFields.Count() > 0)
            {
                if (previousError)
                {
                    builder.Append("\r\n");
                }
                builder.AppendFormat(CultureInfo.CurrentUICulture, "UpdateFields contains the following invalid field names: {0}.", string.Join(", ", invalidUpdateFields));
                previousError = true;
            }

            if (previousError)
            {
                throw new InvalidOperationException(builder.ToString());
            }

            string filter = this.BuildParameterList(ParameterListKind.IncludeWhereClause);

            string updateFields = this.BuildUpdateFieldsList();

            string result = string.Format(CultureInfo.CurrentCulture, this.UpdateStatementFormat, this.TableName, updateFields, filter);

            return result;
        }

        /// <summary>
        /// Build the parameter list for SELECT, UPDATE, DELETE, and INSERT commands.
        /// </summary>
        /// <param name="kind">Change behavior of the parameter lists.</param>
        /// <returns>A list of string containing the parameters.</returns>
        private string BuildParameterList(ParameterListKind kind = ParameterListKind.Default)
        {
            if (this.Filters.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            if (kind.HasFlag(ParameterListKind.IncludeWhereClause))
            {
                builder.Append(" WHERE ");
            }

            string firstFilterKey = this.Filters.Keys.First();
            foreach (string filter in this.Filters.Keys)
            {
                if (firstFilterKey != filter || kind.HasFlag(ParameterListKind.IncludeLeadingAnd))
                {
                    builder.Append(" AND ");
                }

                builder.Append(filter);
                builder.Append(" = ");
                builder.Append(string.Format(CultureInfo.CurrentCulture, this.ParameterFormat, filter.ToCamelCase()));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Build Sort Order phrases.
        /// </summary>
        /// <returns>A string representing the sort orders.</returns>
        private string BuildSortOrder()
        {
            if (this.SortOrder == null || this.SortOrder.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            if (this.IncludeOrderByClause)
            {
                builder.Append(" ORDER BY ");
            }

            var firstSortOrder = this.SortOrder.First();
            foreach (var sortOrder in this.SortOrder)
            {
                if (!sortOrder.Equals(firstSortOrder))
                {
                    builder.Append(", ");
                }

                // Appends the Field Name to the StringBuilder.
                builder.Append(sortOrder.Key);

                if (sortOrder.Value == SortDirection.Ascending)
                {
                    builder.Append(" ASC");
                }
                else
                {
                    builder.Append(" DESC");
                }
            }

            return builder.ToString();
        }
        
        /// <summary>
        /// Build the update field list for UPDATE commands.
        /// </summary>
        /// <returns>A list of fields that are being updated.</returns>
        private string BuildUpdateFieldsList()
        {
            var builder = new StringBuilder();

            string firstKey = this.UpdateFields.Keys.First();
            foreach (string filter in this.UpdateFields.Keys)
            {
                if (firstKey != filter)
                {
                    builder.Append(", ");
                }

                builder.Append(filter);
                builder.Append(" = ");
                builder.Append(string.Format(CultureInfo.CurrentCulture, this.ParameterFormat, filter.ToCamelCase()));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Validates that <paramref name="fields"/> are part of <see cref="AvailableFields />.
        /// </summary>
        /// <param name="fields">the fields to be checked.</param>
        /// <returns>A list of invalid field names.</returns>
        private IEnumerable<string> ValidateFields(ICollection<string> fields)
        {
            var results = fields.Where(x => !this.AvailableFields.Contains(x));

            return results;
        }
    }
}
