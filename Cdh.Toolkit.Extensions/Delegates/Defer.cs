//
// Defer.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2012 Chris Howie
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
using System.Threading;

namespace Cdh.Toolkit.Extensions.Delegates
{
    public static class Defer
    {
        public static DeferState Action(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            return new DeferState(action);
        }

        public class DeferState
        {
            private Action action;
            private List<Func<Exception, bool>> exceptionHandlers = new List<Func<Exception, bool>>();

            internal DeferState(Action action)
            {
                this.action = action;
            }

            private void Execute()
            {
                try {
                    action();
                } catch (Exception ex) {
                    var handled = false;
                    
                    foreach (var handler in exceptionHandlers) {
                        handled = handler(ex);
                        if (handled) {
                            break;
                        }
                    }
                    
                    if (!handled) {
                        throw;
                    }
                }
            }

            public void Immediately()
            {
                Action processor = Execute;

                processor.BeginInvoke(result => processor.EndInvoke(result), null);
            }

            public void For(TimeSpan timeSpan)
            {
                if (timeSpan.CompareTo(TimeSpan.Zero) <= 0) {
                    throw new ArgumentOutOfRangeException("timeSpan", timeSpan, "Must be greater than TimeSpan.Zero.");
                }

                Action processor = delegate {
                    Thread.Sleep(timeSpan);
                    Execute();
                };

                processor.BeginInvoke(result => processor.EndInvoke(result), null);
            }

            public void Until(DateTime dateTime)
            {
                var span = dateTime.ToUniversalTime() - DateTime.UtcNow;

                if (span.CompareTo(TimeSpan.Zero) <= 0) {
                    throw new ArgumentOutOfRangeException("dateTime", dateTime, "Must represent a point of time in the future.");
                }

                For(span);
            }

            public DeferState Handling<T>(Action<T> exceptionHandler)
                where T : Exception
            {
                exceptionHandlers.Add(ex => {
                    if (ex is T) {
                        exceptionHandler((T)ex);
                        return true;
                    }

                    return false;
                });

                return this;
            }

            public DeferState HandlingAll()
            {
                exceptionHandlers.Add(ex => true);

                return this;
            }
        }
    }
}

