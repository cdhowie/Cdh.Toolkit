using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        int MaxArguments { get; }

        void Execute(ICommandContext context, IList<string> arguments);
    }
}
