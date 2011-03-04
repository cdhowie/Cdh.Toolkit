using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Extensions.Xml.Linq
{
	internal static class Common
	{
		public static T GetDefault<T>(bool required, string name)
		{
			if (required)
				throw new InvalidDataException("Required value \"" + name + "\" not present.");

			return default(T);
		}
	}
}
