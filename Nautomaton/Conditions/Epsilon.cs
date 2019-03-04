// <copyright file="Epsilon.cs">
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
    /// Defines the <see cref="Epsilon{Q, Σ}" />
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Σ"></typeparam>
    public class Epsilon<Q, Σ> : ITransitionCondition<Q, Σ> where Q : IComparable<Q>, IEquatable<Q> where Σ : IComparable<Σ>, IEquatable<Σ>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Epsilon{Q, Σ}"/> class.
        /// </summary>
        /// <param name="state">The state<see cref="Q"/></param>
        public Epsilon(Q state)
        {
            this.State = state;
        }

        /// <summary>
        /// Gets the State
        /// </summary>
        public Q State { get; private set; }

        /// <summary>
        /// Gets the Input
        /// </summary>
        public Σ Input => throw new NotImplementedException();

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.State.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Epsilon<Q, Σ> e)
            {
                return e.State.Equals(this.State);
            }

            return false;
        }
    }
}
