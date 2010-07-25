using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cdh.Toolkit.Services
{
	public abstract class ThreadedService : IService
	{
		private object sync = new object();

        protected object RunningLock {
            get { return sync; }
        }

		protected bool ThreadRunning { get; private set; }

		private Thread thread = null;

		#region IService Members

		public virtual void Start()
		{
			lock (sync)
			{
				if (IsRunning || ThreadRunning)
					return;

				thread = new Thread(ThreadEntryPoint);
				IsRunning = true;
				ThreadRunning = true;

				thread.Start();
			}
		}

		public virtual void Stop()
		{
			lock (sync)
			{
				if (ThreadRunning == false)
					return;

				ThreadRunning = false;
				thread.Interrupt();
				thread.Join();

				thread = null;
				IsRunning = false;
			}
		}

        private void ThreadEntryPoint()
        {
            try { ServiceLoop(); }
            catch (ThreadInterruptedException) { }
            finally {
                lock (sync) {
                    thread = null;
                    ThreadRunning = false;
                    IsRunning = false;

                    Cleanup();
                }
            }
        }

		protected abstract void ServiceLoop();

        protected virtual void Cleanup()
        {
        }

		public bool IsRunning { get; private set; }

		#endregion
	}
}
