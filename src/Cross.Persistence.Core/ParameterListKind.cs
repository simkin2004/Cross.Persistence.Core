// <copyright file="ParameterListKind.cs" company="Chris Trout">
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

    /// <summary>
    /// Alters the Parameter List query builder process for specific purposes.
    /// </summary>
    [Flags]
    public enum ParameterListKind
    {
        /// <summary>
        /// Builds a standard parameter list without a leading " WHERE " or " AND " text.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Include leading " WHERE " text when building the parameter list.
        /// </summary>
        IncludeWhereClause = 1,

        /// <summary>
        /// Include leading " AND " text when building the parameter list.
        /// </summary>
        /// <remarks>
        /// Used when other parameters have been defined prior to this parameter list is built.
        /// </remarks>
        IncludeLeadingAnd = 2
    }
}
