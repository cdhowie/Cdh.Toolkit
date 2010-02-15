using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public static class Extensions
    {
        public static IEnumerable<T> ResolveName<T>(this IEnumerable<T> e, string name, Func<T, string> nameSelector)
        {
            return e.Where(i => nameSelector(i).ToLowerInvariant().StartsWith(name.ToLowerInvariant()));
        }
    }
}
