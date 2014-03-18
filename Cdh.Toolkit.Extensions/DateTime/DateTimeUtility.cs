using System;

namespace Cdh.Toolkit.Extensions.DateTime
{
    public static class DateTimeUtility
    {
        private static readonly System.DateTime unixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static System.DateTime FromUnixTimestamp(double timestamp)
        {
            return unixEpoch.AddSeconds(timestamp);
        }

        public static double ToUnixTimestamp(this System.DateTime date)
        {
            return (date - unixEpoch).TotalSeconds;
        }
    }
}

