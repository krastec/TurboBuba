using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Infrastructure
{
    public class TimeUtils
    {
        public static long GetCurrentUnixTimestampMillis()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
