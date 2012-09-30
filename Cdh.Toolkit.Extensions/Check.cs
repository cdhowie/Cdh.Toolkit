using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Extensions
{
    public static class Check
    {
        public static void ArgumentIsNotNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        public static T ArgumentIs<T>(object value, string paramName)
        {
            Check.ArgumentIsNotNull(value, paramName);

            return OptionalArgumentIs<T>(value, paramName);
        }

        public static T OptionalArgumentIs<T>(object value, string paramName)
        {
            if (value == null)
                return default(T);

            if (value is T)
                return (T)value;

            throw new ArgumentException(
                string.Format("Expected object of type {0}, but received object of type {1}.",
                    typeof(T).FullName, value.GetType().FullName),
                paramName);
        }
    }
}
