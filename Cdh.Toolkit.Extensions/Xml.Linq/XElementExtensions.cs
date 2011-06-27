//
// XElementExtensions.cs
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
