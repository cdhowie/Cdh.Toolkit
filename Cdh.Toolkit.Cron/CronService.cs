//
// CronService.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2010 Chris Howie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdh.Toolkit.Collections.Observable;
using Cdh.Toolkit.Collections;
using System.Threading;
using Cdh.Toolkit.Extensions.Events;

namespace Cdh.Toolkit.Cron
{
    public class CronService : IDisposable
    {
        private ObservableCollection<IJob> jobs =
            new ObservableCollection<IJob>(new HashSet<IJob>(), EnumerateBehavior.Lock);

        public IObservableCollection<IJob> Jobs
        {
            get
            {
                var value = jobs;

                if (value == null)
                    throw new ObjectDisposedException("CronService");

                return value;
            }
        }

        private Thread mainThread;

        public event EventHandler<JobExceptionEventArgs> JobException;

        public CronService()
        {
            mainThread = new Thread(MainLoop);

            try { mainThread.Priority = ThreadPriority.Highest; }
            catch { }

            mainThread.Start();
        }

        private void JobFinished(IAsyncResult result)
        {
            IJob job = (IJob)result.AsyncState;

            try
            {
                job.EndRun(result);
            }
            catch (Exception ex)
            {
                JobException.Fire(this, new JobExceptionEventArgs(job, ex));
            }
        }

        private void SleepUntilNextMinute(ref DateTime clock)
        {
            DateTime target = clock.AddMinutes(1);

            for (DateTime reference = DateTime.Now; reference < target; reference = DateTime.Now)
            {
                TimeSpan sleepDuration = target - reference;

                if (sleepDuration > TimeSpan.Zero)
                    Thread.Sleep(sleepDuration);
            }

            clock = target;
        }

        private void MainLoop()
        {
            try
            {
                DateTime clock = DateTime.Now;

                for (; ; )
                {
                    foreach (var job in Jobs)
                    {
                        if (job.ShouldRunAt(clock))
                            job.BeginRun(JobFinished, job);
                    }

                    SleepUntilNextMinute(ref clock);
                }
            }
            catch (ThreadInterruptedException) { }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread mainThread = Interlocked.Exchange(ref this.mainThread, null);

            if (mainThread == null)
                return;

            mainThread.Interrupt();
            mainThread.Join();

            jobs.Clear();
            jobs = null;

            JobException = null;
        }

        #endregion

        ~CronService()
        {
            Dispose();
        }
    }
}
