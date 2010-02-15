using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public class BasicCommandContext : ICommandContext
    {
        public BasicCommandContext(Service service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            Service = service;
        }

        #region ICommandContext Members

        public IConsoleWriter NormalWriter
        {
            get { return Service.NormalWriter; }
        }

        public IConsoleWriter ErrorWriter
        {
            get { return Service.ErrorWriter; }
        }

        public Service Service { get; private set; }

        #endregion
    }
}
