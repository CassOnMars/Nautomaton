// <copyright file="EpsilonStateTable.cs">
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

namespace Nautomaton.StateTables
{
    using Nautomaton.Conditions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EpsilonStateTable{Q, Σ}" />
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Σ"></typeparam>
    public class EpsilonStateTable<Q, Σ> : ITransitionFunc<Q, Σ> where Q : IComparable<Q>, IEquatable<Q> where Σ : IComparable<Σ>, IEquatable<Σ>
    {
        /// <summary>
        /// Defines the _internalTable
        /// </summary>
        private Dictionary<ITransitionCondition<Q, Σ>, ISet<Q>> _internalTable = new Dictionary<ITransitionCondition<Q, Σ>, ISet<Q>>();

        /// <inheritdoc/>
        public bool Add(ITransitionCondition<Q, Σ> condition, Q state)
        {
            if (!this._internalTable.TryGetValue(condition, out ISet<Q> set))
            {
                return this._internalTable.TryAdd(condition, new SortedSet<Q> { state });
            }

            return set.Add(state);
        }

        /// <inheritdoc/>
        public IEnumerable<(ITransitionCondition<Q, Σ> condition, Q state)> GetTransitions()
        {
            return this._internalTable.SelectMany(kvp => kvp.Value.Select(v => (kvp.Key, v))).ToList();
        }

        /// <inheritdoc/>
        public ISet<Q> Move(Q state, Σ input)
        {
            if (!this._internalTable.TryGetValue(new Condition<Q, Σ>(state, input), out ISet<Q> set))
            {
                return new SortedSet<Q> { };
            }

            return set;
        }

        /// <inheritdoc/>
        public bool Remove(ITransitionCondition<Q, Σ> condition, Q state)
        {
            if (!this._internalTable.TryGetValue(condition, out ISet<Q> set))
            {
                return false;
            }

            return set.Remove(state) && (set.Count != 0 || this._internalTable.Remove(condition));
        }
    }
}
