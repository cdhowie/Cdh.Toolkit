//
// RangeCollectionExtensions.cs
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
