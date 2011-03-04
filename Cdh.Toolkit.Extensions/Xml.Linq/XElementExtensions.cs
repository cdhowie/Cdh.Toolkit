using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Cdh.Toolkit.Extensions.Xml.Linq
{
	public static class XElementExtensions
	{
		public static T ValueAs<T>(this XElement element, string name, bool required)
		{
			if (element == null)
				return Common.GetDefault<T>(required, name);

			try
			{
				return (T)Convert.ChangeType(element.Value, typeof(T));
			}
			catch (Exception ex)
			{
				throw new InvalidDataException("Unable to convert data named \"" + name + "\" to the requested type.", ex);
			}
		}

		public static T ValueAs<T>(this XElement element, string name)
		{
			return ValueAs<T>(element, name, false);
		}

		public static T RequiredValueAs<T>(this XElement element, string name)
		{
			return ValueAs<T>(element, name, true);
		}

		public static T AttributeValueAs<T>(this XElement element, XName attributeName, bool required)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			return element.Attribute(attributeName).ValueAs<T>(attributeName.ToString(), required);
		}

		public static T AttributeValueAs<T>(this XElement element, XName attributeName)
		{
			return AttributeValueAs<T>(element, attributeName, false);
		}

		public static T RequiredAttributeValueAs<T>(this XElement element, XName attributeName)
		{
			return AttributeValueAs<T>(element, attributeName, true);
		}
	}
}
