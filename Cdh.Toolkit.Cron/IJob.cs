using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Cron
{
    public interface IJob
    {
        JobLockType LockType { get; }

        void Run();

        IAsyncResult BeginRun(AsyncCallback callback, object state);
        void EndRun(IAsyncResult result);

        bool ShouldRunAgain { get; }
        bool ShouldRunAt(DateTime time);
        DateTime? CalculateNextRun();
    }
}
