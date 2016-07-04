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
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"Date: {CurrentTime}\r\n");
            strBuilder.Append($"OS: {Environment.OSVersion}\r\n");
            strBuilder.Append($"Environment version: {Environment.Version}\r\n");
            strBuilder.Append($"Processors count: {Environment.ProcessorCount}\r\n");
            strBuilder.Append($"Working set: {Environment.WorkingSet} bytes\r\n");
            strBuilder.Append($"Domain name: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            strBuilder.Append($"Service Uptime: {ProcessUptimeAsString}\r\n");
            strBuilder.Append(Environment.NewLine);
            return strBuilder.ToString();
        }

        private static readonly DateTime Year1970 = new DateTime(1970, 1, 1);

        public static int CurrentSeconds()
        {
            TimeSpan ts = Year1970 - DateTime.Now;
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

        /// <summary> 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringToCompare"></param>
        /// <returns></returns>
        public static bool StartsWithMatchCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCulture);
        }

        public static bool EndsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EndsWithMatchCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCulture);
        }
    }
}