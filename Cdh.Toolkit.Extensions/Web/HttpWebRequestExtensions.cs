using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.IO;

namespace Cdh.Toolkit.Extensions.Web
{
    public static class HttpWebRequestExtensions
    {
        public static void PostWwwFormUrlencoded(this HttpWebRequest request, NameValueCollection form)
        {
            Check.ArgumentIsNotNull(request, "request");
            Check.ArgumentIsNotNull(form, "form");

            var encoding = new UTF8Encoding(false);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=" + encoding.WebName;

            using (var writer = new StreamWriter(request.GetRequestStream(), encoding)) {
                writer.Write(form.ToQueryString());
            }
        }
    }
}

