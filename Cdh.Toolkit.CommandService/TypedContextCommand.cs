using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public abstract class TypedContextCommand<TContext> : ICommand
        where TContext : ICommandContext
    {
        #region ICommand Members

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract int MaxArguments { get; }

        void ICommand.Execute(ICommandContext context, IList<string> arguments)
        {
            Execute((TContext)context, arguments);
        }

        public abstract void Execute(TContext context, IList<string> arguments);

        #endregion
    }
}
