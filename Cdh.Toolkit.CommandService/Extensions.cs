using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public static class Extensions
    {
        public static ICollection<T> ResolveName<T>(this IEnumerable<T> e, string name, Func<T, string> nameSelector)
        {
            name = name.ToLowerInvariant();
            List<T> candidates = new List<T>();

            foreach (T i in e)
            {
                string iName = nameSelector(i).ToLowerInvariant();

                if (iName == name)
                {
                    candidates.Clear();
                    candidates.Add(i);
                    break;
                }

                if (iName.StartsWith(name))
                    candidates.Add(i);
            }

            return candidates;
        }
    }
}
