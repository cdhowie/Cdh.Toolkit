using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdh.Toolkit.CommandService.Tests.Commands;

namespace Cdh.Toolkit.CommandService.Tests
{
    internal class TestService : Service
    {
        public TestService()
        {
            RegisterCommand(new EchoCommand());
        }
    }
}
