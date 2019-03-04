// <copyright file="ITransitionFunc.cs">
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

    /// <summary>
    /// Defines the <see cref="ITransitionFunc{Q, Σ}" />
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Σ"></typeparam>
    public interface ITransitionFunc<Q, Σ> where Q : IComparable<Q>, IEquatable<Q> where Σ : IComparable<Σ>, IEquatable<Σ>
    {
        /// <summary>
        /// Produces the next states provided a condition.
        /// </summary>
        /// <param name="state">The state<see cref="Q"/></param>
        /// <param name="input">The input<see cref="Σ"/></param>
        /// <returns>The <see cref="ISet{Q}"/></returns>
        ISet<Q> Move(Q state, Σ input);

        /// <summary>
        /// Adds a transition to the function.
        /// </summary>
        /// <param name="condition">The condition<see cref="ITransitionCondition{Q, Σ}"/></param>
        /// <param name="state">The state<see cref="Q"/></param>
        /// <returns>The <see cref="bool"/></returns>
        bool Add(ITransitionCondition<Q, Σ> condition, Q state);

        /// <summary>
        /// Removes a transition from the function.
        /// </summary>
        /// <param name="condition">The condition<see cref="ITransitionCondition{Q, Σ}"/></param>
        /// <param name="state">The state<see cref="Q"/></param>
        /// <returns>The <see cref="bool"/></returns>
        bool Remove(ITransitionCondition<Q, Σ> condition, Q state);

        /// <summary>
        /// Gets all the transitions in the function.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{(ITransitionCondition{Q, Σ} condition, Q state)}"/></returns>
        IEnumerable<(ITransitionCondition<Q, Σ> condition, Q state)> GetTransitions();
    }
}
