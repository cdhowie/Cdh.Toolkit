using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Cron
{
    public class JobExceptionEventArgs : EventArgs
    {
        public IJob Job { get; private set; }
        public Exception Exception { get; private set; }

        public JobExceptionEventArgs(IJob job, Exception exception)
        {
            Job = job;
            Exception = exception;
        }
    }
}
