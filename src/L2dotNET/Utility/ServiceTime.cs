using System;
using System.Diagnostics;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Provides access to some service time related properties.
    /// </summary>
    public static class ServiceTime
    {
        /// <summary>
        /// Gets current time as string.
        /// </summary>
        public static string CurrentTime => DateTime.Now.ToString("d/M/yyyy H:mm:ss.ffff");

        /// <summary>
        /// Gets service uptime as <see cref="TimeSpan"/>.
        /// </summary>
        public static TimeSpan ServiceUptime => DateTime.Now - Process.GetCurrentProcess().StartTime;

        /// <summary>
        /// Gets service uptime as string.
        /// </summary>
        public static string ServiceUptimeAsString => DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime).ToString();
    }
}