using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cdh.Toolkit.CommandService.Tests
{
    [TestFixture]
    public class DefaultCommandArgumentParserTests : CommonArgumentParserTests
    {
        protected override ICommandArgumentParser Parser
        {
            get { return DefaultCommandArgumentParser.Instance; }
        }

        [Test]
        public void TwoArgumentsWithPaddingAndMax()
        {
            var expected = new[] { "hello", "there  world   " };
            var input = " hello   there  world   ";

            DoTest(expected, input, 2);
        }
    }
}
