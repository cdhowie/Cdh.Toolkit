using System;
using System.Collections.Generic;

namespace Cdh.Toolkit.Extensions.StringBuilder
{
    public static class Extensions
    {
        public static void AppendStrings(this System.Text.StringBuilder self, IEnumerable<string> strings)
        {
            Check.ArgumentIsNotNull(self, "self");
            Check.ArgumentIsNotNull(strings, "strings");

            foreach (var s in strings) {
                self.Append(s);
            }
        }
    }
}

