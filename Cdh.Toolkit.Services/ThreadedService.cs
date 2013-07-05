//
// ThreadedService.cs
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
using System.Threading;

namespace Cdh.Toolkit.Services
{
    public abstract class ThreadedService : IService
    {
        private object sync = new object();

        protected object RunningLock {
            get { return sync; }
        }

        protected volatile bool threadRunning = false;

        protected bool ThreadRunning
        {
            get { return threadRunning; }
        }

        private Thread thread = null;

        protected abstract bool IsBackgroundService { get; }

        #region IService Members

        public virtual void Start()
        {
            lock (sync)
            {
                if (isRunning || threadRunning)
                    return;

                thread = new Thread(ThreadEntryPoint);
                thread.IsBackground = IsBackgroundService;
                isRunning = true;
                threadRunning = true;

                thread.Start();
            }
        }

        public virtual void Stop()
        {
            Thread threadCopy;

            lock (sync)
            {
                if (!threadRunning)
                    return;

                threadCopy = thread;

                threadRunning = false;
                StopRequested();
            }

            threadCopy.Join();
        }

        protected virtual void StopRequested()
        {
            thread.Interrupt();
        }

        private void ThreadEntryPoint()
        {
            try { ServiceLoop(); }
            catch (ThreadInterruptedException) { }
            finally {
                lock (sync) {
                    thread = null;
                    threadRunning = false;
                    isRunning = false;

                    // I don't like nesting "try" but we need to make sure anyone blocked in WaitForStop() is released.
                    try {
                        Cleanup();
                    } finally {
                        Monitor.PulseAll(sync);
                    }
                }
            }
        }

        protected abstract void ServiceLoop();

        protected virtual void Cleanup()
        {
        }

        private volatile bool isRunning = false;

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public void WaitForStop()
        {
            if (!isRunning) { return; }

            lock (sync) {
                while (isRunning) {
                    Monitor.Wait(sync);
                }
            }
        }

        #endregion
    }
}
