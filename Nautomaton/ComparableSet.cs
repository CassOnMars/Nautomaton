// <copyright file="ComparableSet.cs">
// Copyright 2019 Dustin R. Heart.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>

namespace Nautomaton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ComparableSet{Q}" />
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    public class ComparableSet<Q> : SortedSet<Q>, IComparableSet<Q> where Q : IComparable<Q>, IEquatable<Q>
    {
        /// <inheritdoc/>
        public int CompareTo(IComparableSet<Q> other)
        {
            if (this.IsSubsetOf(other) || this.IsSupersetOf(other) || this.Count != other.Count)
            {
                return this.Count - other.Count;
            }
            else
            {
                var diffIndex = Enumerable.Range(0, this.Count).First(i => this.ElementAt(i).CompareTo(other.ElementAt(i)) != 0);

                return this.ElementAt(diffIndex).CompareTo(other.ElementAt(diffIndex));
            }
        }

        /// <inheritdoc/>
        public bool Equals(IComparableSet<Q> other)
        {
            return this.SetEquals(other);
        }
    }

    /// <summary>
    /// Defines the <see cref="ComparableSetExtensions" />
    /// </summary>
    public static class ComparableSetExtensions
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{Q}"/> into a <see cref="ComparableSet{Q}"/>.
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="enumerable">The enumerable<see cref="IEnumerable{Q}"/></param>
        /// <returns>The <see cref="ComparableSet{Q}"/></returns>
        public static ComparableSet<Q> ToComparableSet<Q>(this IEnumerable<Q> enumerable) where Q : IComparable<Q>, IEquatable<Q>
        {
            var set = new ComparableSet<Q>();

            foreach (var q in enumerable)
            {
                set.Add(q);
            }

            return set;
        }
    }
}
