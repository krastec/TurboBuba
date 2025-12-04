using System;


namespace TurboBuba.Infrastructure
{
    public class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss.fff}] {message}");
        }

        public static void Debug(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss.fff}] DEBUG: {message}");
        }

        public static void Warn(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss.fff}] WARNING: {message}");
        }
        public static void Error(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss.fff}] ERROR: {message}");
        }
    }
}
