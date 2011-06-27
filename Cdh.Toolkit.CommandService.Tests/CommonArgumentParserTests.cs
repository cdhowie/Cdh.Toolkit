//
// CommonArgumentParserTests.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2010 Chris Howie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
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
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cdh.Toolkit.CommandService.Tests
{
    public abstract class CommonArgumentParserTests : ArgumentParserTests
    {
        [Test]
        public void ZeroArguments()
        {
            var expected = new string[0];
            var input = "";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void ZeroArgumentsWithPadding()
        {
            var expected = new string[0];
            var input = "   ";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void OneArgument()
        {
            var expected = new[] { "foobar" };
            var input = "foobar";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void OneArgumentWithPadding()
        {
            var expected = new[] { "foobar" };
            var input = "  foobar  ";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void TwoArguments()
        {
            var expected = new[] { "hello", "world" };
            var input = "hello world";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void TwoArgumentsWithPadding()
        {
            var expected = new[] { "hello", "world" };
            var input = "  hello   world ";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void TwoArgumentsWithMax()
        {
            var expected = new[] { "hello", "there world" };
            var input = "hello there world";

            DoTest(expected, input, 2);
        }
    }
}
