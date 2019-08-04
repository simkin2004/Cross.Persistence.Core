// <copyright file="DatabaseOptionsTests.cs" company="Chris Trout">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Data.Common;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class DatabaseOptionsTests
    {
        [TestMethod]
        public void Returns_Valid_Instance_From_Full_Constructor()
        {
            // arrange
            var connectionString = "ConnectionString";
            var providerFactory = new Mock<DbProviderFactory>().Object;
            var queryBuilder = new SqlQueryBuilder();

            // act
            var result = new DatabaseOptions(connectionString, providerFactory, queryBuilder);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(connectionString, result.ConnectionString);
            Assert.AreEqual(providerFactory, result.ProviderFactory);
            Assert.AreEqual(queryBuilder, result.QueryBuilder);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_Constructor_Where_ConnectionString_Is_Null()
        {
            // arrange
            var connectionString = null as string;
            var providerFactory = new Mock<DbProviderFactory>().Object;
            var queryBuilder = new SqlQueryBuilder();

            var expectedParamName = "connectionString";

            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => new DatabaseOptions(connectionString, providerFactory, queryBuilder));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_Constructor_Where_ProviderFactory_Is_Null()
        {
            // arrange
            var connectionString = "ConnectionString";
            var providerFactory = null as DbProviderFactory;
            var queryBuilder = new SqlQueryBuilder();

            var expectedParamName = "providerFactory";

            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => new DatabaseOptions(connectionString, providerFactory, queryBuilder));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_Constructor_Where_QueryBuilder_Is_Null()
        {
            // arrange
            var connectionString = "ConnectionString";
            var providerFactory = new Mock<DbProviderFactory>().Object;
            var queryBuilder = null as SqlQueryBuilder;

            var expectedParamName = "queryBuilder";

            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => new DatabaseOptions(connectionString, providerFactory, queryBuilder));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }
    }
}
