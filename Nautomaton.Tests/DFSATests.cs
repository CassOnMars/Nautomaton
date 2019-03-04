// <copyright file="DFSATests.cs">
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

namespace Tests
{
    using Nautomaton;
    using Nautomaton.Conditions;
    using Nautomaton.StateTables;
    using NUnit.Framework;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="DFSATests" />
    /// </summary>
    public class DFSATests
    {
        [Test]
        public void PowersetConstructionFromABasicNFSAProcessesSuccessfully()
        {
            var nfsa = new NFSA<int, char, EpsilonStateTable<int, char>>();

            nfsa.AddState(0);
            nfsa.AddState(1);
            nfsa.AddState(2);
            nfsa.AddState(3);

            nfsa.AddInput('a');
            nfsa.AddInput('m');
            nfsa.AddInput('n');
            nfsa.AddInput('o');
            nfsa.AddInput('t');
            nfsa.AddInput('u');

            nfsa.AddTransition(new Condition<int, char>(0, 'a'), 1); // 
            nfsa.AddTransition(new Condition<int, char>(1, 'u'), 2); //   .-------------ε------------v     
            nfsa.AddTransition(new Condition<int, char>(2, 't'), 3); //   .---a----v        .---t----v
            nfsa.AddTransition(new Condition<int, char>(3, 'o'), 2); //   0<-.     1---u--->2<---o---3
            nfsa.AddTransition(new Condition<int, char>(2, 'm'), 0); //   |  '------m-------'---n----^
            nfsa.AddTransition(new Condition<int, char>(0, 'a'), 2); //   '--------a--------^
            nfsa.AddTransition(new Condition<int, char>(2, 'n'), 3); //
            nfsa.AddTransition(new Epsilon<int, char>(0), 3);

            nfsa.AddFinal(3);

            nfsa.SetInitial(0);

            var evaluation = nfsa.Evaluate("automaton");
            Assert.IsTrue(evaluation.Contains(3));
            Assert.IsTrue(evaluation.Count == 1);

            var dfsa = DFSA<IComparableSet<int>, char, StateTable<IComparableSet<int>, char>>.FromNFSA<int, char, EpsilonStateTable<int, char>, StateTable<IComparableSet<int>, char>>(nfsa);

            Assert.IsTrue(dfsa.GetFinal().Count() == 2);
            Assert.IsTrue(dfsa.GetFinal().First().Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(dfsa.GetFinal().Last().Equals(new ComparableSet<int> { 0, 3 }));

            Assert.IsTrue(dfsa.GetInitial().Equals(new ComparableSet<int> { 0, 3 }));

            var states = dfsa.GetStates().ToList();
            Assert.IsTrue(states.Count() == 4);
            Assert.IsTrue(states[0].Equals(new ComparableSet<int> { 2 }));
            Assert.IsTrue(states[1].Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(states[2].Equals(new ComparableSet<int> { 0, 3 }));
            Assert.IsTrue(states[3].Equals(new ComparableSet<int> { 1, 2 }));

            var transitions = dfsa.GetTransitions().ToList();
            Assert.IsTrue(transitions.Count() == 10);
            Assert.IsTrue(transitions[0].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 0, 3 }, 'a')) && transitions[0].state.Equals(new ComparableSet<int> { 1, 2 }));
            Assert.IsTrue(transitions[1].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 0, 3 }, 'o')) && transitions[1].state.Equals(new ComparableSet<int> { 2 }));
            Assert.IsTrue(transitions[2].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 1, 2 }, 'u')) && transitions[2].state.Equals(new ComparableSet<int> { 2 }));
            Assert.IsTrue(transitions[3].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 1, 2 }, 't')) && transitions[3].state.Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(transitions[4].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 1, 2 }, 'm')) && transitions[4].state.Equals(new ComparableSet<int> { 0, 3 }));
            Assert.IsTrue(transitions[5].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 1, 2 }, 'n')) && transitions[5].state.Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(transitions[6].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 2 }, 't')) && transitions[6].state.Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(transitions[7].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 2 }, 'm')) && transitions[7].state.Equals(new ComparableSet<int> { 0, 3 }));
            Assert.IsTrue(transitions[8].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 2 }, 'n')) && transitions[8].state.Equals(new ComparableSet<int> { 3 }));
            Assert.IsTrue(transitions[9].condition.Equals(new Condition<IComparableSet<int>, char>(new ComparableSet<int> { 3 }, 'o')) && transitions[9].state.Equals(new ComparableSet<int> { 2 }));
        }
    }
}
