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

        private List<string> commandLog = new List<string>();

        public ServiceTests()
        {
            service = new TestService();
            service.CommandLogWriter.LineWritten += CommandLogWritten;
        }

        private void CommandLogWritten(object sender, LineWrittenEventArgs args)
        {
            commandLog.Add(args.Line);
        }

        private void AssertCommandLogEquals(params string[] commands)
        {
            commandLog.AssertIsEqualTo(commands, "commandLog");
            commandLog.Clear();
        }

        private void AssertCommandLogIsEmpty()
        {
            Assert.AreEqual(commandLog.Count, 0, "commandLog.Count");
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

        private void RunAmbiguousCommandTest(string commandName, params string[] expected)
        {
            try
            {
                service.ExecuteCommand(commandName);

                Assert.Fail("Expected AmbiguousCommandException");
            }
            catch (AmbiguousCommandException ex)
            {
                Assert.AreEqual(expected.Length, ex.Commands.Count, "Commands.Count");

                foreach (string name in expected)
                    Assert.IsTrue(ex.Commands.Contains(name), "commands contains " + name);
            }
        }

        [Test]
        public void AmbiguousCompletionWithExactMatch()
        {
            service.ExecuteCommand("ambig-foo");
            AssertCommandLogEquals("ambig-foo");
        }

        [Test]
        public void AmbiguousCompletion()
        {
            RunAmbiguousCommandTest("ambig-fo", "ambig-foo", "ambig-foobar");
            AssertCommandLogIsEmpty();
        }

        [Test]
        public void UnambiguousCompletion()
        {
            service.ExecuteCommand("ambig-b");
            AssertCommandLogEquals("ambig-bar");
        }
    }
}
