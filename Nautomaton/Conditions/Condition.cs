// <copyright file="Condition.cs">
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

namespace Nautomaton.Conditions
{
    using System;

    /// <summary>
    /// Defines the <see cref="Condition{Q, Σ}" />
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Σ"></typeparam>
    public class Condition<Q, Σ> : ITransitionCondition<Q, Σ> where Q : IComparable<Q>, IEquatable<Q> where Σ : IComparable<Σ>, IEquatable<Σ>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Condition{Q, Σ}"/> class.
        /// </summary>
        /// <param name="state">The state<see cref="Q"/></param>
        /// <param name="input">The input<see cref="Σ"/></param>
        public Condition(Q state, Σ input)
        {
            this.State = state;
            this.Input = input;
        }

        /// <summary>
        /// Gets the State
        /// </summary>
        public Q State { get; private set; }

        /// <summary>
        /// Gets the Input
        /// </summary>
        public Σ Input { get; private set; }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.State.GetHashCode() ^ ~this.Input.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Condition<Q, Σ> c)
            {
                return c.State.Equals(this.State) && c.Input.Equals(this.Input);
            }

            return false;
        }
    }
}
