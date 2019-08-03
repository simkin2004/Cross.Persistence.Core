// <copyright file="IFieldValidator{T}.cs" company="Chris Trout">
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
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the Field Validation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type against which fields will be validated.</typeparam>
    public interface IFieldValidator<T>
    {
        /// <summary>
        /// Gets the type name of <typeparamref name="T"/> against which the list of fields will be validated.
        /// </summary>
        Type ContainingType { get; }

        /// <summary>
        /// Validate fields against the fields in <typeparamref name="T"/>.
        /// </summary>
        /// <param name="fields">The field names to be validated.</param>
        /// <returns><see langword="true" /> if all of the fields are supported by <typeparamref name="T"/>; otherwise <see langword="false""/>.</returns>
        Task<bool> ValidateFieldsAsync(IEnumerable<string> fields);
    }
}
