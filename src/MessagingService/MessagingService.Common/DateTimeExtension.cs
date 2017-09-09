using System;

namespace MessagingService.Common
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimestamp(this DateTime dt)
        {
            long unixTime = ((DateTimeOffset)dt).ToUnixTimeSeconds();
            return unixTime;
        }
    }
}
