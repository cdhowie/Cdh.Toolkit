using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService.Formatting
{
    public enum Alignment
    {
        Unspecified,
        Left,
        Center,
        Right
    }

    internal static class AlignmentExtensions
    {
        public static Alignment Or(this Alignment current, Alignment fallback)
        {
            return current == Alignment.Unspecified ? fallback : current;
        }
    }
}
