﻿//
// HelpCommand.cs
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

namespace Cdh.Toolkit.CommandService.Commands
{
    public class HelpCommand : ICommand
    {
        public HelpCommand() { }

        #region ICommand Members

        public string Name
        {
            get { return "help"; }
        }

        public string Description
        {
            get { return "Displays this information."; }
        }

        public int MaxArguments
        {
            get { return 0; }
        }

        public void Execute(ICommandContext context, IList<string> arguments)
        {
            int maxNameLength = context.Service.Commands.Select(i => i.Name.Length).Max();

            foreach (var i in context.Service.Commands.OrderBy(i => i.Name))
                context.NormalWriter.WriteLine("{0,-" + maxNameLength + "} - {1}", i.Name, i.Description);
        }

        #endregion
    }
}
