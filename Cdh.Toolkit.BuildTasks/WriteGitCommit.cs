//
// WriteGitCommit.cs
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
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace Cdh.Toolkit.BuildTasks
{
    public class WriteGitCommit : Task
    {
        public string OutputFile { get; private set; }

        [Required]
        public string Repository { get; private set; }

        public WriteGitCommit()
        {
            OutputFile = "git-commit";
        }

        private static readonly Regex validationRegex = new Regex(@"^[0-9a-f]{40}$");

        public override bool Execute()
        {
            string headRef;
            using (var headFile = File.OpenText(Path.Combine(Repository, "HEAD")))
                headRef = headFile.ReadLine();

            string head;

            if (headRef.StartsWith("ref: ")) {
                string refPath = headRef.Substring(5);
                string refFilePath = refPath.Replace('/', Path.DirectorySeparatorChar);

                try {
                    using (var refFile = File.OpenText(Path.Combine(Repository, refFilePath)))
                        head = refFile.ReadLine();
                } catch (FileNotFoundException) {
                    // Maybe the ref is packed?

                    using (var packedRef = File.OpenText(Path.Combine(Repository, "packed-refs"))) {
                        string line;

                        head = null;

                        while ((line = packedRef.ReadLine()) != null) {
                            if (line.StartsWith("#")) {
                                continue;
                            }

                            var parts = line.Split(new[] { ' ' }, 2);

                            if (parts.Length == 2 && refPath.Equals(parts[1])) {
                                head = parts[0];
                                break;
                            }
                        }

                        if (head == null) {
                            throw new ApplicationException(string.Format("Unable to find ref {0} in packed refs.", headRef));
                        }
                    }
                }
            } else {
                head = headRef;
            }

            if (!validationRegex.IsMatch(head)) {
                throw new ApplicationException(string.Format("Located commit ID \"{0}\" is not valid.", head));
            }

            // Avoid rewriting the file if it exists and has exactly the same content.  This avoids unnecessary rebuilds.
            try {
                if (File.ReadAllText(OutputFile) == head) {
                    return true;
                }
            } catch {
                // On any error just continue and try to replace the file.
            }

            File.WriteAllText(OutputFile, head);

            return true;
        }
    }
}

