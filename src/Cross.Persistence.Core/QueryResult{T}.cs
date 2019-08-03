// <copyright file="QueryResult{T}.cs" company="Chris Trout">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides the results of a query and the total count of the records.
    /// </summary>
    /// <typeparam name="T">The type of each item in <see cref="Results" />.</typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResult{T}" /> with the specified parameters.
        /// </summary>
        /// <param name="offset">number of records per page.</param>
        /// <param name="limit">number of the first record in the <see cref="Results" />.</param>
        /// <param name="totalCount">total count of records in the set.</param>
        /// <param name="results">paginated set.</param>
        public QueryResult(long offset, long limit, long totalCount, IEnumerable<T> results)
        {
            if (limit < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit must be greater than 10.");
            }

            this.Limit = limit;

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "offset must be greater than 0.");
            }

            this.Offset = offset;

            this.Results = results ?? throw new ArgumentNullException(nameof(results));

            if (totalCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalCount), totalCount, "totalCount must be greater than 0.");
            }

            this.TotalCount = totalCount;
        }

        /// <summary>
        /// Gets the number of records per page.
        /// </summary>
        public long Limit { get; }

        /// <summary>
        /// Gets the number of the first record in the <see cref="Results" />.
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// Gets a subset of results of a query.
        /// </summary>
        public IEnumerable<T> Results { get; }

        /// <summary>
        /// Gets the total number of records in the query.
        /// </summary>
        public long TotalCount { get; }
    }
}
