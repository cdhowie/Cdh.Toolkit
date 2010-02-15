using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Commands
{
    public class HelpCommand : ICommand
    {
        public HelpCommand() { }

        #region ICommand Members

        public string Name
        {
            get { return "help"; }
        }

        public string Description
        {
            get { return "Displays this information."; }
        }

        public int MaxArguments
        {
            get { return 0; }
        }

        public void Execute(ICommandContext context, IList<string> arguments)
        {
            int maxNameLength = context.Service.Commands.Select(i => i.Name.Length).Max();

            foreach (var i in context.Service.Commands.OrderBy(i => i.Name))
                context.NormalWriter.WriteLine("{0,-" + maxNameLength + "} - {1}", i.Name, i.Description);
        }

        #endregion
    }
}
