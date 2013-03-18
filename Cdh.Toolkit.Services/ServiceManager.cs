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
using System.Threading;
using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;

namespace Cdh.Toolkit.Services
{
    public class ServiceManager : IServiceManager
    {
        protected ReaderWriterLockSlim Lock { get; private set; }

        protected ICollection<IService> ServiceCollection { get; private set; }

        public ICollection<IService> Services { get; private set; }

        private Dictionary<Type, IService> serviceCache = new Dictionary<Type, IService>();

        public ServiceManager()
        {
            Lock = new ReaderWriterLockSlim();

            ServiceCollection = new HashSet<IService>();

            Services = new ReadOnlyCollection<IService>(new SynchronizedCollection<IService>(ServiceCollection, EnumerateBehavior.Lock, Lock));
        }

        public virtual void RegisterService(IService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            using (Lock.Write()) {
                ServiceCollection.Add(service);

                PurgeCache();
            }
        }

        public virtual void UnregisterService(IService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            using (Lock.Write()) {
                ServiceCollection.Remove(service);

                PurgeCache();
            }
        }

        protected void PurgeCache()
        {
            serviceCache.Clear();
        }

        public virtual IEnumerable<T> GetServices<T>() where T : IService
        {
            using (Lock.Read()) {
                return ServiceCollection.OfType<T>().ToList();
            }
        }

        public virtual T GetService<T>() where T : IService
        {
            using (Lock.Read()) {
                IService cachedService;
                if (serviceCache.TryGetValue(typeof(T), out cachedService)) {
                    return (T)cachedService;
                }

                var service = GetServices<T>().SingleOrDefault();

                serviceCache[typeof(T)] = service;

                return service;
            }
        }
    }
}
