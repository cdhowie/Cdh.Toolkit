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
