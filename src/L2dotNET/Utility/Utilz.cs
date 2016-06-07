using System;
using System.Diagnostics;
using System.Text;

namespace L2dotNET.Utility
{
    public static class Utilz
    {
        public static string CurrentTime
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"); }
        }

        public static TimeSpan ProcessUptime
        {
            get { return DateTime.Now - Process.GetCurrentProcess().StartTime; }
        }

        public static string ProcessUptimeAsString
        {
            get { return DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime).ToString(); }
        }

        public static string SystemSummary()
        {
            StringBuilder m_StrBuilder = new StringBuilder();
            m_StrBuilder.Append($"Date: {CurrentTime}\r\n");
            m_StrBuilder.Append($"OS: {Environment.OSVersion}\r\n");
            m_StrBuilder.Append($"Environment version: {Environment.Version.ToString()}\r\n");
            m_StrBuilder.Append($"Processors count: {Environment.ProcessorCount}\r\n");
            m_StrBuilder.Append($"Working set: {Environment.WorkingSet} bytes\r\n");
            m_StrBuilder.Append($"Domain name: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            m_StrBuilder.Append($"Service Uptime: {ProcessUptimeAsString}\r\n");
            m_StrBuilder.Append(Environment.NewLine);
            return m_StrBuilder.ToString();
        }

        private static readonly DateTime year1970 = new DateTime(1970, 1, 1);

        public static int CurrentSeconds()
        {
            TimeSpan ts = year1970 - DateTime.Now;
            return (int)ts.TotalSeconds * -1;
        }

        /// <summary>
        /// String extension method in order
        /// to check if two strings match no matter the case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// String extension method in order
        /// to check if two strings match with the exact the case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool EqualsMatchCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCulture);
        }
    }
}