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
    }
}
