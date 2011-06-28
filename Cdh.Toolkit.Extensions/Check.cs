using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Extensions
{
    public static class Check
    {
        public static void ArgumentIsNotNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException("paramName");
        }
    }
}
