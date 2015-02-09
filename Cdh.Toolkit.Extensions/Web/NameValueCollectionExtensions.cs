using System;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using Cdh.Toolkit.Extensions.Enumerable;

namespace Cdh.Toolkit.Extensions.Web
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection self)
        {
            Check.ArgumentIsNotNull(self, "self");

            return (
                from key in self.AllKeys
                from value in self.GetValues(key)
                select HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value)
            ).Join("&");
        }
    }
}

