using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Services
{
	public interface IService
	{
		void Start();
		void Stop();

		bool IsRunning { get; }
	}
}
