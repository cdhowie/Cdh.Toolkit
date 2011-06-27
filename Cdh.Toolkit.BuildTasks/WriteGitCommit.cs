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

        public override bool Execute()
        {
            using (var outputFile = File.CreateText(OutputFile)) {
                string head;
                using (var headFile = File.OpenText(Path.Combine(Repository, "HEAD")))
                    head = headFile.ReadLine();
   
                if (head.StartsWith("ref: ")) {
                    string refPath = head.Substring(5).Replace('/', Path.DirectorySeparatorChar);

                    using (var refFile = File.OpenText(Path.Combine(Repository, refPath)))
                        head = refFile.ReadLine();
                }

                outputFile.Write(head);
            }

            return true;
        }
    }
}

