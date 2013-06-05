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

        public static IEnumerable<T> SequenceArgumentElementsAreNotNull<T>(IEnumerable<T> sequence, string paramName)
            where T : class
        {
            Check.ArgumentIsNotNull(sequence, paramName);

            return Sequence<T>(sequence, i => {
                if (i == null) {
                    throw new ArgumentException("Contains a null element.", paramName);
                }
            });
        }

        public static IEnumerable<T> Sequence<T>(IEnumerable<T> sequence, Action<T> validator)
        {
            Check.ArgumentIsNotNull(sequence, "sequence");
            Check.ArgumentIsNotNull(validator, "validator");

            return CreateSequenceEnumerable<T>(sequence, validator);
        }

        private static IEnumerable<T> CreateSequenceEnumerable<T>(IEnumerable<T> sequence, Action<T> validator)
        {
            foreach (var i in sequence) {
                validator(i);
                yield return i;
            }
        }
    }
}
