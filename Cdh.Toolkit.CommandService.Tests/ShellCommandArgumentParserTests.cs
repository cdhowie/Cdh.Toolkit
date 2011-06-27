//
// ShellCommandArgumentParserTests.cs
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
    [TestFixture]
    public class ShellCommandArgumentParserTests : CommonArgumentParserTests
    {
        protected override ICommandArgumentParser Parser
        {
            get { return ShellCommandArgumentParser.Instance; }
        }

        [Test]
        public void QuotedArguments()
        {
            var expected = new[] { "hello", "there world", " how are you? " };
            var input = "hello \"there world\" ' how are you? '";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void MaxOverflow()
        {
            var expected = new[] { "this", "is", "a", "test", "with many arguments" };
            var input = "this   is a   test with many   arguments ";

            DoTest(expected, input, 5);
        }

        [Test]
        public void QuotedMaxOverflow()
        {
            var expected = new[] { "this", "is", "a", "test", "with many   arguments" };
            var input = "this   is a   test with 'many   arguments' ";

            DoTest(expected, input, 5);
        }

        [Test]
        public void InnerQuotedArgument()
        {
            var expected = new[] { "test", "of", "inner quoted", "arguments" };
            var input = "test of in'ner quot'ed arguments";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void EscapeSequences()
        {
            var expected = new[] {
                "'\"", "'\"", "hello\nthere\nworld", @"embedded\backslash", "embedded space",
                @"failed\escape"
            };

            var input = @"'\'\""' ""\'\"""" 'hello\nthere\rworld' embedded\\backslash embedded\ space failed\escape";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void EmptyQuotedParameters()
        {
            var expected = new[] {
                "", "foo", "", "bar baz", ""
            };

            var input = @"  ''  foo """"   'bar baz' """"";

            DoTest(expected, input, int.MaxValue);
        }

        [Test]
        public void BareNestedQuotes()
        {
            var expected = new[] {
                "'", "''", "\"", "\"\""
            };

            var input = @"""'"" ""''"" '""' '""""'";

            DoTest(expected, input, int.MaxValue);
        }
    }
}
