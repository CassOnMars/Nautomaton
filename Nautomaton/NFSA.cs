// <copyright file="NFSA.cs">
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
    using Nautomaton.Conditions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a Non-Deterministic Finite State Automaton.
    /// </summary>
    /// <typeparam name="Q">The base type representing states. Must implement <see cref="IComparable{Q}"/> and <see cref="IEquatable{Q}"/>.</typeparam>
    /// <typeparam name="Σ">The base type representing inputs. Must implement <see cref="IComparable{Σ}"/> and <see cref="IEquatable{Σ}"/>.</typeparam>
    /// <typeparam name="Δ">The base type representing a transition function. Use StateTable (or a deriving type) for an NFSA without ε-moves. Use EpsilonStateTable for an NFSA with ε-moves. Must implement <see cref="ITransitionFunc{Q, Σ}"/>.</typeparam>
    public class NFSA<Q, Σ, Δ> where Q : IComparable<Q>, IEquatable<Q> where Σ : IComparable<Σ>, IEquatable<Σ> where Δ : ITransitionFunc<Q, Σ>, new()
    {
        /// <summary>
        /// Defines the _states
        /// </summary>
        private ISet<Q> _states;

        /// <summary>
        /// Defines the _inputs
        /// </summary>
        private ISet<Σ> _inputs;

        /// <summary>
        /// Defines the _transitions
        /// </summary>
        private Δ _transitions;

        /// <summary>
        /// Defines the _initial
        /// </summary>
        private Q _initial;

        /// <summary>
        /// Defines the _final
        /// </summary>
        private ISet<Q> _final;

        /// <summary>
        /// Initializes a new instance of the <see cref="NFSA{Q, Σ, Δ}"/> class,
        /// creating an empty <see cref="SortedSet{Q}"/> for states, an empty
        /// <see cref="SortedSet{Σ}"/> for inputs, and an instance of Δ.
        /// </summary>
        public NFSA()
        {
            this._states = new SortedSet<Q>();
            this._inputs = new SortedSet<Σ>();
            this._transitions = new Δ();
            this._final = new SortedSet<Q>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NFSA{Q, Σ, Δ}"/> class,
        /// and creates an instance of Δ.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="inputs"></param>
        /// <param name="initial"></param>
        /// <param name="final"></param>
        public NFSA(ISet<Q> states, ISet<Σ> inputs, Q initial, ISet<Q> final)
        {
            this._states = states;
            this._inputs = inputs;
            this._initial = initial;
            this._final = final;
            this._transitions = new Δ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NFSA{Q, Σ, Δ}"/> class,
        /// using traditional 5-tuple (Q, Σ, Δ, q_0, F) representation.
        /// </summary>
        /// <param name="states">The states<see cref="ISet{Q}"/></param>
        /// <param name="inputs">The inputs<see cref="ISet{Σ}"/></param>
        /// <param name="transitions">The transitions<see cref="Δ"/></param>
        /// <param name="initial">The initial<see cref="Q"/></param>
        /// <param name="final">The final<see cref="ISet{Q}"/></param>
        public NFSA(ISet<Q> states, ISet<Σ> inputs, Δ transitions, Q initial, ISet<Q> final)
        {
            // TODO: If the info needed to describe the NFSA is already present
            // and there are no expectations of change, this could just as well
            // be immutable, and so we should offer immutable versions of this
            // class.
            this._states = states;
            this._inputs = inputs;
            this._transitions = transitions;
            this._initial = initial;
            this._final = final;
        }

        #region NFSA Set Construction methods

        /// <summary>
        /// Adds a state to the set.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>A boolean value indicating whether or not the addition was
        /// successful.</returns>
        public bool AddState(Q state)
        {
            return this._states.Add(state);
        }

        /// <summary>
        /// Removes a state from the set. Sets the initial state to the first
        /// remaining state, or if empty, null if a reference type, or default
        /// if a value type.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>A boolean value indicating whether or not the removal was
        /// successful.</returns>
        public bool RemoveState(Q state)
        {
            var toRemove = this._transitions.GetTransitions().Where(t => t.state.Equals(state) || t.condition.State.Equals(state));

            if (this._initial.Equals(state))
            {
                this._initial = this._states.FirstOrDefault(s => !s.Equals(state));
            }

            if (this._final.Contains(state))
            {
                this.RemoveFinal(state);
            }

            return toRemove.All(t => this._transitions.Remove(t.condition, t.state)) && this._states.Remove(state);
        }

        /// <summary>
        /// Adds an input to the set.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A boolean value indicating whether or not the addition was
        /// successful.</returns>
        public bool AddInput(Σ input)
        {
            return this._inputs.Add(input);
        }

        /// <summary>
        /// Removes an input from the set.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A boolean value indicating whether or not the removal was
        /// successful.</returns>
        public bool RemoveInput(Σ input)
        {
            var toRemove = this._transitions.GetTransitions().Where(t => !(t.condition is Epsilon<Q, Σ>) && t.condition.Input.Equals(input));

            return toRemove.All(t => this._transitions.Remove(t.condition, t.state)) && this._inputs.Remove(input);
        }

        /// <summary>
        /// Adds a transition to the transition function.
        /// </summary>
        /// <param name="condition">The tuple of state and input.</param>
        /// <param name="state">The resulting state of the provided condition.</param>
        /// <returns>A boolean value indicating whether or not the addition was
        /// successful.</returns>
        public bool AddTransition(ITransitionCondition<Q, Σ> condition, Q state)
        {
            return this._states.Contains(condition.State) && (condition is Epsilon<Q, Σ> || this._inputs.Contains(condition.Input)) && this._transitions.Add(condition, state);
        }

        /// <summary>
        /// Removes a transition from the transition function.
        /// </summary>
        /// <param name="condition">The tuple of state and input.</param>
        /// <param name="state">The resulting state of the provided condition.</param>
        /// <returns>A boolean value indicating whether or not the removal was
        /// successful.</returns>
        public bool RemoveTransition(ITransitionCondition<Q, Σ> condition, Q state)
        {
            return this._transitions.Remove(condition, state);
        }

        /// <summary>
        /// Sets the initial state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>A boolean value indicating whether or not the setting was
        /// successful.</returns>
        public bool SetInitial(Q state)
        {
            if (this._states.Contains(state))
            {
                this._initial = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a state to the set of final states.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>A boolean value indicating whether or not the addition
        /// was successful.</returns>
        public bool AddFinal(Q state)
        {
            if (this._states.Contains(state))
            {
                return this._final.Add(state);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a state from the set of final states.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>A boolean value indicating whether or not the removal
        /// was successful.</returns>
        public bool RemoveFinal(Q state)
        {
            return this._final.Remove(state);
        }

        #endregion

        /// <summary>
        /// Retrieves a copied list of states from the set, retaining order.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Q}"/> representation of the set.</returns>
        public IEnumerable<Q> GetStates()
        {
            return this._states.ToList();
        }

        /// <summary>
        /// Retrieves a copied list of inputs from the set, retaining order.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Σ}"/> representation of the set.</returns>
        public IEnumerable<Σ> GetInputs()
        {
            return this._inputs.ToList();
        }

        /// <summary>
        /// Retrieves the initial state.
        /// </summary>
        /// <returns>The initial state.</returns>
        public Q GetInitial()
        {
            return this._initial;
        }

        /// <summary>
        /// Retrieves a copied list of states from the set of final states.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Q}"/> representation of the set.</returns>
        public IEnumerable<Q> GetFinal()
        {
            return this._final.ToList();
        }

        /// <summary>
        /// Retrieves a list of transitions from the state transition function.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Σ}"/> representation of the
        /// transitions.</returns>
        public IEnumerable<(ITransitionCondition<Q, Σ> condition, Q state)> GetTransitions()
        {
            return this._transitions.GetTransitions();
        }

        /// <summary>
        /// Evaluates a given sequence on the NFSA. Caution is advised that
        /// implementations of <typeparamref name="Δ"/> may differ in
        /// performance, and optimal worst-case evaluation of any NFSA
        /// approaches O(sq²), s = |sequence|, q = |Q|. Alternatively,
        /// you may construct a <see cref="DFSA"/> from this instance.
        /// </summary>
        /// <param name="sequence">An input sequence.</param>
        /// <returns>A set of terminal states, if any are met.</returns>
        public ISet<Q> Evaluate(IEnumerable<Σ> sequence)
        {
            ISet<Q> initialSet = new SortedSet<Q> { this._initial };

            return sequence
                .Aggregate(
                    initialSet,
                    (states, input) => states
                        .Select(s => this._transitions.Move(s, input))
                        .SelectMany(s => s)
                        .Where(s => this._states.Contains(s))
                        .ToHashSet()
                )
                .Where(s => this._final.Contains(s))
                .ToHashSet();
        }
    }
}
