//
// Extensions.cs
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
using System.Text;

namespace Cdh.Toolkit.Extensions.Enumerable
{
    public static class Extensions
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> self)
            where T : struct
        {
            Check.ArgumentIsNotNull(self, "self");

            return CreateNotNullEnumerable<T>(self);
        }

        private static IEnumerable<T> CreateNotNullEnumerable<T>(this IEnumerable<T?> self)
            where T : struct
        {
            foreach (T? i in self)
                if (i.HasValue)
                    yield return i.Value;
        }

        public static void Walk<T>(this IEnumerable<T> self)
        {
            Check.ArgumentIsNotNull(self, "self");

            using (IEnumerator<T> walker = self.GetEnumerator())
                while (walker.MoveNext()) ;
        }
        
        public static void CopyInto<T>(this IEnumerable<T> self, IList<T> list)
        {
            CopyInto<T>(self, list, 0);
        }

        public static void CopyInto<T>(this IEnumerable<T> self, IList<T> list, int index)
        {
            Check.ArgumentIsNotNull(self, "self");
            Check.ArgumentIsNotNull(list, "list");

            foreach (T item in self)
                list[index++] = item;
        }

        public static IEnumerable<T> Delimit<T>(this IEnumerable<T> self, T delimiter)
        {
            Check.ArgumentIsNotNull(self, "self");

            return CreateDelimitEnumerable<T>(self, delimiter);
        }

        private static IEnumerable<T> CreateDelimitEnumerable<T>(this IEnumerable<T> self, T delimiter)
        {
            using (var e = self.GetEnumerator())
            {
                if (!e.MoveNext())
                    yield break;

                yield return e.Current;

                while (e.MoveNext())
                {
                    yield return delimiter;
                    yield return e.Current;
                }
            }
        }

        public static string Join(this IEnumerable<string> self)
        {
            Check.ArgumentIsNotNull(self, "self");

            var sb = new StringBuilder();

            foreach (var str in self)
                if (str != null)
                    sb.Append(str);

            return sb.ToString();
        }

        public static string Join(this IEnumerable<string> self, string delimiter)
        {
            return Join(Delimit(self, delimiter));
        }
    }
}
