using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cdh.Toolkit.CommandService.Tests
{
    public abstract class ArgumentParserTests
    {
        protected abstract ICommandArgumentParser Parser { get; }

        protected void DoTest(IList<string> expected, string input, int maxlen)
        {
            IList<string> actual = Parser.ParseArguments(input, maxlen);

            Assert.AreEqual(expected.Count, actual.Count, "result.Count");

            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i], "result[" + i + "]");
        }
    }
}
