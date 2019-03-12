using System;
namespace JWTDecoder.Helpers
{
    internal static class DateTimeHelpers
    {
        internal static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        internal static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
