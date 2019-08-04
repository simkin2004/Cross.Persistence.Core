// <copyright file="QueryResultTests.cs" company="Chris Trout">
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

namespace Cross.Persistence.Core.Tests
{
    using Cross.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlQueryBuilderTests
    {
        [TestMethod]
        public void Returns_Valid_SqlQueryBuilder_From_Constructor()
        {
            // arrange
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = new SqlQueryBuilder();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_DeleteSqlStatement_From_BuildDeleteCommand_With_No_Filter()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                         .AddTableName("dbo.Applications")
                                         .AddAvailableFields(new List<string>() { "ApplicationID", "Description" });


            var expectedSql = "DELETE FROM dbo.Applications;";

            // act
            var result = sqlQueryBuilder.BuildDeleteCommand();

            // assert
            Assert.AreEqual(expectedSql, result);
        }

        [TestMethod]
        public void Returns_DeleteSqlStatement_From_BuildDeleteCommand_With_Multiple_Filters()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                         .AddTableName("dbo.Applications")
                                         .AddAvailableFields(new List<string>() { "ApplicationID", "Description" })
                                         .AddFilters(new Dictionary<string, object>() { { "ApplicationID", Guid.NewGuid() }, { "Description", "Hi There!" } });

            var expectedSql = "DELETE FROM dbo.Applications WHERE ApplicationID = @applicationID AND Description = @description;";

            // act
            var result = sqlQueryBuilder.BuildDeleteCommand();

            // assert
            Assert.AreEqual(expectedSql, result);
        }

        [TestMethod]
        public void Returns_DeleteSqlStatement_From_BuildDeleteCommand_With_One_Filter()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                         .AddTableName("dbo.Applications")
                                         .AddAvailableFields(new List<string>() { "ApplicationID", "Description" })
                                         .AddFilters(new Dictionary<string, object>() { { "ApplicationID", Guid.NewGuid() } });

            var expectedSql = "DELETE FROM dbo.Applications WHERE ApplicationID = @applicationID;";

            // act
            var result = sqlQueryBuilder.BuildDeleteCommand();

            // assert
            Assert.AreEqual(expectedSql, result);
        }

        [TestMethod]
        public void Returns_InsertSqlStatement_From_BuildInsertCommand()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                         .AddTableName("dbo.Applications")
                                         .AddAvailableFields(new List<string>() { "ApplicationID", "Description" });


            var expectedSql = "INSERT dbo.Applications (ApplicationID, Description) VALUES(@applicationID, @description);";

            // act
            var result = sqlQueryBuilder.BuildInsertCommand();

            // assert
            Assert.AreEqual(expectedSql, result);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddAvailableFields_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>() { "One", "Two" };
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddAvailableFields(availableFields);

            // assert
            Assert.AreEqual(sqlQueryBuilder, result);
            Assert.AreEqual(availableFields, result.AvailableFields);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddEndingRowNumberParameterName_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "lastNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddEndingRowNumberParameterName(endingRowNumberParameterName);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddFilters_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>() { {"applicationID", Guid.NewGuid() }};
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddFilters(filters);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(filters["applicationID"], result.Filters["applicationID"]);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddSortOrder_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>() { { "applicationID", SortDirection.Ascending } };
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddSortOrder(sortOrder);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(sortOrder["applicationID"], result.SortOrder["applicationID"]);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddStartingRowNumberParameterName_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "starting";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddStartingRowNumberParameterName(startingRowNumberParameterName);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_AddTableName_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = "dbo.Applications";
            var updateFields = new Dictionary<string, object>();
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddTableName(tableName);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Returns_SqlQueryBuilder_Instance_From_UpdateFields_When_Value_Provided()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();
            var deleteStatementFormat = "DELETE FROM {0}{1};";
            var endingRowNumberParameterName = "endingRowNumber";
            var filters = new Dictionary<string, object>();
            var insertStatementFormat = "INSERT INTO {0} ({1}) VALUES ({2});";
            var parameterNameFormat = "@{0}";
            var selectStatementFormat = "SELECT {1} FROM "
                                        + " (SELECT COUNT() as row_count, ROW_NUMBER() OVER({5}) AS row_no, {1} FROM {0}) as subSelect"
                                        + " WHERE subSelect.row_no >= {3} AND subSelect.row_num < {4}{2};";
            var sortOrder = new Dictionary<string, SortDirection>();
            var startingRowNumberParameterName = "startingRowNumber";
            var tableName = null as string;
            var updateFields = new Dictionary<string, object>() { { "applicationID", 1 } };
            var updateStatementFormat = "UPDATE {0} SET {1}{2};";

            // act
            var result = sqlQueryBuilder.AddUpdateFields(updateFields);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(availableFields.Count, result.AvailableFields.Count);
            Assert.AreEqual(deleteStatementFormat, result.DeleteStatementFormat);
            Assert.AreEqual(endingRowNumberParameterName, result.EndingRowNumberParameterName);
            Assert.AreEqual(filters.Count, result.Filters.Count);
            Assert.AreEqual(insertStatementFormat, result.InsertStatementFormat);
            Assert.AreEqual(parameterNameFormat, result.ParameterFormat);
            Assert.AreEqual(selectStatementFormat, result.SelectStatementFormat);
            Assert.AreEqual(sortOrder.Count, result.SortOrder.Count);
            Assert.AreEqual(startingRowNumberParameterName, result.StartingRowNumberParameterName);
            Assert.AreEqual(tableName, result.TableName);
            Assert.AreEqual(updateFields.Count, result.UpdateFields.Count);
            Assert.AreEqual(updateFields["applicationID"], result.UpdateFields["applicationID"]);
            Assert.AreEqual(updateStatementFormat, result.UpdateStatementFormat);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddAvailableFields_When_AvailableFields_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = null as IList<string>;

            var expectedParamName = nameof(availableFields);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddAvailableFields(availableFields));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddEndingRowNumberParameterName_When_EndingRowNumberParameterName_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var endingRowNumberParameterName = null as String;

            var expectedParamName = nameof(endingRowNumberParameterName);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddEndingRowNumberParameterName(endingRowNumberParameterName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddFilters_When_Filters_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var filters = null as IDictionary<string, object>;

            var expectedParamName = nameof(filters);

            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddFilters(filters));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddSortOrder_When_SortOrder_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var sortOrder = null as IDictionary<string, SortDirection>;

            var expectedParamName = nameof(sortOrder);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddSortOrder(sortOrder));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddStartingRowNumberParameterName_When_StartingRowNumberParameterName_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var startingRowNumberParameterName = null as String;

            var expectedParamName = nameof(startingRowNumberParameterName);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddStartingRowNumberParameterName(startingRowNumberParameterName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddTableName_When_TableName_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var tableName = null as string;

            var expectedParamName = nameof(tableName);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddTableName(tableName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_AddUpdateFields_When_UpdateFields_Is_Null()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var updateFields = null as IDictionary<string, object>;

            var expectedParamName = nameof(updateFields);
            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => sqlQueryBuilder.AddUpdateFields(updateFields));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentOutOfRangeException_From_AddAvailableFields_When_AvailableFields_Contains_No_Fields()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var availableFields = new List<string>();

            var expectedParamName = nameof(availableFields);
            var expectedMessage = $"AvailableFields must contain at least 1 field.\r\nParameter name: {expectedParamName}";
            // act
            var result = Assert.ThrowsException<ArgumentOutOfRangeException>(() => sqlQueryBuilder.AddAvailableFields(availableFields));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void Throws_ArgumentOutOfRangeException_From_AddUpdateFields_When_UpdateFields_Contains_No_Fields()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();
            var updateFields = new Dictionary<string, object>();

            var expectedParamName = nameof(updateFields);
            var expectedMessage = $"UpdateFields must contain at least 1 field.\r\nParameter name: {expectedParamName}";
            // act
            var result = Assert.ThrowsException<ArgumentOutOfRangeException>(() => sqlQueryBuilder.AddUpdateFields(updateFields));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [DataTestMethod]
        [DataRow("    ")]
        [DataRow("")]
        [DataRow("\r\n\t")]
        public void Throws_ArgumentWhiteSpaceException_From_AddEndingRowNumberParameterName_When_EndingRowNumberParameterName_Is_WhiteSpace(string endingRowNumberParameterName)
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();

            var expectedParamName = nameof(endingRowNumberParameterName);

            // act
            var result = Assert.ThrowsException<ArgumentWhiteSpaceException>(() => sqlQueryBuilder.AddEndingRowNumberParameterName(endingRowNumberParameterName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [DataTestMethod]
        [DataRow("    ")]
        [DataRow("")]
        [DataRow("\r\n\t")]
        public void Throws_ArgumentWhiteSpaceException_From_AddStartingRowNumberParameterName_When_StartingRowNumberParameterName_Is_WhiteSpace(string startingRowNumberParameterName)
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();

            var expectedParamName = nameof(startingRowNumberParameterName);

            // act
            var result = Assert.ThrowsException<ArgumentWhiteSpaceException>(() => sqlQueryBuilder.AddStartingRowNumberParameterName(startingRowNumberParameterName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [DataTestMethod]
        [DataRow("    ")]
        [DataRow("")]
        [DataRow("\r\n\t")]
        public void Throws_ArgumentWhiteSpaceException_From_AddTableName_When_TableName_Is_WhiteSpace(string tableName)
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder();

            var expectedParamName = nameof(tableName);

            // act
            var result = Assert.ThrowsException<ArgumentWhiteSpaceException>(() => sqlQueryBuilder.AddTableName(tableName));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_InvalidOperationException_From_BuildDeleteCommand_When_AvailableFields_Are_Not_Set()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                            .AddTableName("dbo.Applications");

            var expectedMessage = "AvailableFields must be specified to build an DELETE command.";

            // act
            var result = Assert.ThrowsException<InvalidOperationException>(() => sqlQueryBuilder.BuildDeleteCommand());

            // assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void Throws_InvalidOperationException_From_BuildDeleteCommand_When_Filters_Has_Invalid_Field_Names()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                            .AddTableName("dbo.Applications")
                                            .AddAvailableFields(new List<string>() { "ApplicationID", "Description" })
                                            .AddFilters(new Dictionary<string, object>() { { "RollingID", Guid.NewGuid() }, { "Name", "YourName" } });
            var expectedMessage = "Filters contains the following invalid field names: RollingID, Name.";

            // act
            var result = Assert.ThrowsException<InvalidOperationException>(() => sqlQueryBuilder.BuildDeleteCommand());

            // assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void Throws_InvalidOperationException_From_BuildDeleteCommand_When_TableName_Is_Not_Set()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                            .AddAvailableFields(new List<string>() { "ApplicationID", "Description" })
                                            .AddFilters(new Dictionary<string, object>() { { "ApplicationID", Guid.NewGuid() } });
            var expectedMessage = "TableName must be specified to build a DELETE command.";

            // act
            var result = Assert.ThrowsException<InvalidOperationException>(() => sqlQueryBuilder.BuildDeleteCommand());

            // assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void Throws_InvalidOperationException_From_BuildInsertCommand_When_AvailableFields_Are_Not_Set()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                            .AddTableName("dbo.Applications");

            var expectedMessage = "AvailableFields must be specified to build an INSERT command.";

            // act
            var result = Assert.ThrowsException<InvalidOperationException>(() => sqlQueryBuilder.BuildInsertCommand());

            // assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void Throws_InvalidOperationException_From_BuildInsertCommand_When_TableName_Is_Not_Set()
        {
            // arrange
            var sqlQueryBuilder = new SqlQueryBuilder()
                                            .AddAvailableFields(new List<string>() { "ApplicationID", "Description" });
            var expectedMessage = "TableName must be specified to build an INSERT command.";

            // act
            var result = Assert.ThrowsException<InvalidOperationException>(() => sqlQueryBuilder.BuildInsertCommand());

            // assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, result.Message);
        }



    }
}
