using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Commands
{
    public class QuitCommand : ICommand
    {
        public QuitCommand() { }

        #region ICommand Members

        public string Name
        {
            get { return "quit"; }
        }

        public string Description
        {
            get { return "Quits."; }
        }

        public int MaxArguments
        {
            get { return 0; }
        }

        public void Execute(ICommandContext context, IList<string> arguments)
        {
            context.Service.FireTerminatedByUser();
        }

        #endregion
    }
}
