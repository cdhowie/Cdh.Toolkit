using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Cdh.Toolkit.Extensions.Xml.Linq
{
	public static class XAttributeExtensions
	{
		public static T ValueAs<T>(this XAttribute attribute, string name, bool required)
		{
			if (attribute == null)
				return Common.GetDefault<T>(required, name);

			return (T)Convert.ChangeType(attribute.Value, typeof(T));
		}

		public static T ValueAs<T>(this XAttribute attribute, string name)
		{
			return ValueAs<T>(attribute, name, false);
		}

		public static T RequiredValueAs<T>(this XAttribute attribute, string name)
		{
			return ValueAs<T>(attribute, name, true);
		}
	}
}
