// <copyright file="SaveModifiers.cs" company="Chris Trout">
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
    /// Modifies the expected behavior of persistence services.
    /// </summary>
    [Flags]
    public enum SaveModifiers
    {
        /// <summary>
        /// Do not alter standard save functionality, meaning inserts or updates are expected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Change the Save function to create an item, if it doesn't exist.
        /// </summary>
        CreateIfNotExists = 1,

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the item already exists.
        /// </summary>
        ThrowExceptionIfExists = 2,

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the item doesn't already exist.
        /// </summary>
        ThrowExceptionIfDoesNotExist = 4
    }

}
