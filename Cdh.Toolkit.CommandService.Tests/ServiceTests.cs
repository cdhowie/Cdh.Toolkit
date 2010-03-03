using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cdh.Toolkit.CommandService.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        private TestService service;

        public ServiceTests()
        {
            service = new TestService();
        }

        [Test]
        public void EchoCommand()
        {
            bool passed = false;
            string[] commandArgs = null;

            EventHandler<LineWrittenEventArgs> lineReader = (o, args) =>
            {
                string expected = string.Join(" ", commandArgs);

                Assert.AreEqual(expected, args.Line, "args.Line");
                passed = true;
            };

            service.ConsoleLineWritten += lineReader;

            try
            {
                commandArgs = new[] { "this", "is", "a", "test" };

                service.ExecuteCommand("echo", commandArgs);
                Assert.IsTrue(passed, "lineReader received expected line");

                passed = false;
                service.ExecuteCommandLine("echo this is a test");
                Assert.IsTrue(passed, "lineReader received expected line");
            }
            finally
            {
                service.ConsoleLineWritten -= lineReader;
            }
        }
    }
}
