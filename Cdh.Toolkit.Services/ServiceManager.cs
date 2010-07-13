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

		public ServiceManager() { }

		public void RegisterService(IService service)
		{
			if (service == null)
				throw new ArgumentNullException("service");

			services.Add(service);
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
	}
}
