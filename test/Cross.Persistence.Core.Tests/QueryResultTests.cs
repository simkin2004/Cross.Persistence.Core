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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class QueryResultTests
    {
        [TestMethod]
        public void Returns_Valid_Instance_From_Constructor()
        {
            // arrange
            long limit = 10;
            long offset = 0;
            IEnumerable<object> results = new List<object>();
            long totalCount = 20;
            
            // act
            var result = new QueryResult<object>(offset, limit, totalCount, results);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(limit, result.Limit);
            Assert.AreEqual(offset, result.Offset);
            Assert.AreEqual(results, result.Results);
            Assert.AreEqual(totalCount, result.TotalCount);
        }

        [TestMethod]
        public void Throws_ArgumentNullException_From_Constructor_When_Results_Is_Null()
        {
            // arrange
            long limit = 10;
            long offset = 0;
            IEnumerable<object> results = null;
            long totalCount = 20;

            string expectedParamName = nameof(results);

            // act
            var result = Assert.ThrowsException<ArgumentNullException>(() => new QueryResult<object>(offset, limit, totalCount, results));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentOutOfRangeException_From_Constructor_When_Limit_Is_Less_Than_Ten()
        {
            // arrange
            long limit = 8;
            long offset = 0;
            IEnumerable<object> results = new List<object>();
            long totalCount = 20;

            string expectedParamName = nameof(limit);
            string expectedMessage = $"{expectedParamName} must be greater than 10.\r\nParameter name: {expectedParamName}\r\nActual value was {limit}.";

            // act
            var result = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new QueryResult<object>(offset, limit, totalCount, results));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(limit, result.ActualValue);
            Assert.AreEqual(expectedMessage, result.Message);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentOutOfRangeException_From_Constructor_When_Offset_Is_Less_Than_Zero()
        {
            // arrange
            long limit = 10;
            long offset = -1;
            IEnumerable<object> results = new List<object>();
            long totalCount = 20;

            string expectedParamName = nameof(offset);
            string expectedMessage = $"{expectedParamName} must be greater than 0.\r\nParameter name: {expectedParamName}\r\nActual value was {offset}.";

            // act
            var result = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new QueryResult<object>(offset, limit, totalCount, results));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(offset, result.ActualValue);
            Assert.AreEqual(expectedMessage, result.Message);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }

        [TestMethod]
        public void Throws_ArgumentOutOfRangeException_From_Constructor_When_TotalCount_Is_Less_Than_Zero()
        {
            // arrange
            long limit = 10;
            long offset = 0;
            IEnumerable<object> results = new List<object>();
            long totalCount = -1;

            string expectedParamName = nameof(totalCount);
            string expectedMessage = $"{expectedParamName} must be greater than 0.\r\nParameter name: {expectedParamName}\r\nActual value was {totalCount}.";

            // act
            var result = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new QueryResult<object>(offset, limit, totalCount, results));

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(totalCount, result.ActualValue);
            Assert.AreEqual(expectedMessage, result.Message);
            Assert.AreEqual(expectedParamName, result.ParamName);
        }
    }
}
