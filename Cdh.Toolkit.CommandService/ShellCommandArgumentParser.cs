using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Cdh.Toolkit.CommandService
{
    public sealed class ShellCommandArgumentParser : ICommandArgumentParser
    {
        public static readonly ShellCommandArgumentParser Instance = new ShellCommandArgumentParser();

        private ShellCommandArgumentParser() { }

        #region ICommandArgumentParser Members

        public IList<string> ParseArguments(string arguments, int maxArguments)
        {
            return DoParse(arguments, maxArguments).ToList();
        }

        private IEnumerable<string> DoParse(string arguments, int maxArguments)
        {
            StringBuilder argBuilder = new StringBuilder();
            char quote = '\0';
            bool escaped = false;
            bool sawQuote = false;

            bool hadMaxArgumentsSpace = false;

            maxArguments--;

            int argumentsSeen = 0;

            foreach (char c in arguments)
            {
                if (escaped)
                {
                    switch (c)
                    {
                        case 'n':
                        case 'r':
                            argBuilder.Append('\n');
                            break;

                        case '\'':
                        case '"':
                            argBuilder.Append(c);
                            break;

                        case '\\':
                            argBuilder.Append('\\');
                            break;

                        case ' ':
                            argBuilder.Append(' ');
                            break;

                        default:
                            argBuilder.Append('\\');
                            argBuilder.Append(c);
                            break;
                    }

                    escaped = false;
                }
                else if (c == ' ')
                {
                    if (quote != '\0')
                    {
                        argBuilder.Append(' ');
                    }
                    else if (argumentsSeen >= maxArguments)
                    {
                        if (!hadMaxArgumentsSpace)
                        {
                            argBuilder.Append(' ');
                            hadMaxArgumentsSpace = true;
                        }
                    }
                    else if (argBuilder.Length != 0 || sawQuote)
                    {
                        yield return argBuilder.ToString();
                        argBuilder.Length = 0;
                        sawQuote = false;

                        argumentsSeen++;
                    }
                }
                else
                {
                    hadMaxArgumentsSpace = false;

                    if (quote != '\0' && c == quote)
                    {
                        quote = '\0';
                    }
                    else if (c == '\\')
                    {
                        escaped = true;
                    }
                    else if (c == '\'' || c == '"')
                    {
                        quote = c;
                        sawQuote = true;
                    }
                    else
                    {
                        argBuilder.Append(c);
                    }
                }
            }

            if (quote != '\0')
                throw new InvalidDataException("Unable to parse arguments: unterminated " + quote);

            if (argBuilder.Length != 0 || sawQuote)
            {
                if (hadMaxArgumentsSpace)
                    argBuilder.Length--;

                yield return argBuilder.ToString();
            }
        }

        #endregion
    }
}
