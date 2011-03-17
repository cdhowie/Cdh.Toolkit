using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Collections
{
	internal static class Rocks
	{
		public static bool ConvertObject<T>(object value, out T unboxed)
		{
			if (value is T) {
				unboxed = (T)value;
				return true;
			}

			unboxed = default(T);

			if (value == null && !typeof(T).IsValueType)
				return true;

			return false;
		}
	}
}
