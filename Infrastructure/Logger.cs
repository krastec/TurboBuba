using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TurboBuba.Infrastructure
{
    public class Logger
    {
        public static void Log(string message)
        {
            Debug.WriteLine($"[{DateTime.UtcNow:HH:mm:ss.fff}] {message}");
        }
    }
}
