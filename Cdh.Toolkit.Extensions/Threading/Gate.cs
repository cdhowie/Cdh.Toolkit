//
// Gate.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2013 Chris Howie
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
using System.Threading;

namespace Cdh.Toolkit.Extensions.Threading
{
    // ManualResetEvent doesn't guarantee that all threads are released before Set() returns, so we have this...
    public class Gate
    {
        private volatile bool open;
        private object sync = new object();

        public Gate(bool open)
        {
            this.open = open;
        }

        public void Open()
        {
            lock (sync) {
                open = true;
                Monitor.PulseAll(sync);
            }
        }

        public void Close()
        {
            // No need to lock; the write is atomic and we are not pulsing.
            open = false;
        }

        public void Pulse()
        {
            lock (sync) {
                Monitor.PulseAll(sync);
            }
        }
        
        public void Wait()
        {
            Wait(null);
        }

        public bool Wait(TimeSpan? timeout)
        {
            // Try to fast-succeed without acquiring the lock.
            if (open) { return true; }

            // Gate was closed.
            lock (sync) {
                // We have to test again because the gate may have been opened while we waited for the lock.
                if (open) { return true; }

                // It wasn't open, now we wait.
                return timeout.HasValue ? Monitor.Wait(sync, timeout.Value) : Monitor.Wait(sync);
            }
        }

        public bool Wait(int timeout)
        {
            return Wait(timeout == Timeout.Infinite ? (TimeSpan?)null : TimeSpan.FromMilliseconds(timeout));
        }
    }
}

