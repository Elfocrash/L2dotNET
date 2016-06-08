using System;
using System.Diagnostics;
using System.Text;

namespace L2dotNET.Utility
{
    public class Utilz
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
            m_StrBuilder.Append($"Environment version: {Environment.Version}\r\n");
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
    }
}