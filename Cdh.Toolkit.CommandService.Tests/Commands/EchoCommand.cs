using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Tests.Commands
{
    public class EchoCommand : ICommand
    {
        #region ICommand Members

        public string Name
        {
            get { return "echo"; }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public int MaxArguments
        {
            get { return int.MaxValue; }
        }

        public void Execute(ICommandContext context, IList<string> arguments)
        {
            context.NormalWriter.WriteLine(string.Join(" ", arguments.ToArray()));
        }

        #endregion
    }
}
