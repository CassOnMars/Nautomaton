// <copyright file="NFSATests.cs">
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

    /// <summary>
    /// Defines the <see cref="NFSATests" />
    /// </summary>
    public class NFSATests
    {
        [Test]
        public void EvaluationOfABasicNFSAProcessesSuccessfully()
        {
            var nfsa = new NFSA<int, char, StateTable<int, char>>();

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
            nfsa.AddTransition(new Condition<int, char>(1, 'u'), 2); //        
            nfsa.AddTransition(new Condition<int, char>(2, 't'), 3); //   .---a----v        .---t----v
            nfsa.AddTransition(new Condition<int, char>(3, 'o'), 2); //   0<-.     1---u--->2<---o---3
            nfsa.AddTransition(new Condition<int, char>(2, 'm'), 0); //   |  '------m-------'---n----^
            nfsa.AddTransition(new Condition<int, char>(0, 'a'), 2); //   '--------a--------^
            nfsa.AddTransition(new Condition<int, char>(2, 'n'), 3); //   

            nfsa.AddFinal(3);

            nfsa.SetInitial(0);

            var evaluation = nfsa.Evaluate("automaton");
            Assert.IsTrue(evaluation.Contains(3));
            Assert.IsTrue(evaluation.Count == 1);
        }
    }
}
