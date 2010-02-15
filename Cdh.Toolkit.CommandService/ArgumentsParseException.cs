using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.CommandService
{
    public class ArgumentsParseException : Exception
    {
        public ArgumentsParseException(string message) : base(message) { }
        public ArgumentsParseException(string message, Exception innerException) : base(message) { }
    }
}
