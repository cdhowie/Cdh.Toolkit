using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cdh.Toolkit.Cron
{
    public static class RangeCollectionExtensions
    {
        private static readonly Regex rangeRegex = new Regex(
@"^

(
    (?<single>[0-9]{1,2})   # A single number by itself
    |
    (
        (
            \*              # The entire range
            |
            ((?<start>[0-9]{1,2})-(?<end>[0-9]{1,2}))   # A specific range
        )
        (/(?<step>[0-9]{1,2}))?                         # Optional step
    )
)

$",
            RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);

        private static void ThrowBadRangeSpecifier(string specifier)
        {
            throw new ArgumentException("specifier: Invalid range specifier: " + specifier);
        }

        public static void AddRangeSpecifier(this RangeCollection self, string specifier)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (specifier == null)
                throw new ArgumentNullException("specifier");

            string[] list = specifier.Split(',');

            RangeCollection staging = new RangeCollection(self.Minimum, self.Maximum);

            foreach (string i in list)
            {
                Match match = rangeRegex.Match(i);

                if (!match.Success)
                    ThrowBadRangeSpecifier(specifier);

                if (match.Groups["single"].Success)
                {
                    staging.Add(int.Parse(match.Groups["single"].Value));
                }
                else
                {
                    int start = self.Minimum;
                    int end = self.Maximum;
                    int step = 1;

                    if (match.Groups["step"].Success)
                        step = int.Parse(match.Groups["step"].Value);

                    if (match.Groups["start"].Success)
                    {
                        start = int.Parse(match.Groups["start"].Value);
                        end = int.Parse(match.Groups["end"].Value);
                    }

                    if (end < start)
                        ThrowBadRangeSpecifier(specifier);

                    for (int j = start; j <= end; j += step)
                        staging.Add(j);
                }
            }

            foreach (int i in staging)
                self.Add(i);
        }
    }
}
