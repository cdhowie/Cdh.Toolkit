using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cdh.Toolkit.CommandService.Tests
{
    public static class Extensions
    {
        public static void AssertIsEqualTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string name)
        {
            if (expected == null)
                Assert.AreSame(actual, null, name + " is null");
            else if (actual == null)
                Assert.Fail(name + " is not null");

            int index = 0;

            using (var actualE = actual.GetEnumerator())
            using (var expectedE = expected.GetEnumerator())
            {
                while (expectedE.MoveNext())
                {
                    Assert.IsTrue(actualE.MoveNext(), name + " has expected length");

                    Assert.AreEqual(expectedE.Current, actualE.Current, name + "[" + index + "]");

                    index++;
                }

                Assert.IsFalse(actualE.MoveNext(), name + " has expected length");
            }
        }
    }
}
