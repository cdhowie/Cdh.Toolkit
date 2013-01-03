//
// ServiceManager.cs
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

using Cdh.Toolkit.Collections;

namespace Cdh.Toolkit.Services
{
    public class ServiceManager
    {
        private SynchronizedCollection<IService> services =
            new SynchronizedCollection<IService>(new HashSet<IService>(), EnumerateBehavior.Lock);

        public ICollection<IService> Services { get; private set; }

        public ServiceManager()
        {
            Services = new ReadOnlyCollection<IService>(services);
        }

        public void RegisterService(IService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            services.Add(service);
        }

        public void RegisterAndStartService(IService service)
        {
            RegisterService(service);
            service.Start();
        }

        public void UnregisterService(IService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            services.Remove(service);
        }

        public T GetService<T>() where T : IService
        {
            return services.OfType<T>().SingleOrDefault();
        }

        public T RequireService<T>() where T : IService
        {
            var service = GetService<T>();

            if (service == null) {
                throw new InvalidOperationException(string.Format(
                    "Required service type {0} not registered.", typeof(T).FullName));
            }

            return service;
        }
    }
}
