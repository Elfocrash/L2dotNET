using System;
using System.Diagnostics;
using System.Text;

namespace L2dotNET.GameService.tools
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
            m_StrBuilder.AppendFormat("Date: {0}\r\n", CurrentTime);
            m_StrBuilder.AppendFormat("OS: {0}\r\n", Environment.OSVersion);
            m_StrBuilder.AppendFormat("Environment version: {0}\r\n", Environment.Version.ToString());
            m_StrBuilder.AppendFormat("Processors count: {0}\r\n", Environment.ProcessorCount);
            m_StrBuilder.AppendFormat("Working set: {0} bytes\r\n", Environment.WorkingSet);
            m_StrBuilder.AppendFormat("Domain name: {0}\r\n", AppDomain.CurrentDomain.FriendlyName);
            m_StrBuilder.AppendFormat("Service Uptime: {0}\r\n", ProcessUptimeAsString);
            m_StrBuilder.Append(Environment.NewLine);
            return m_StrBuilder.ToString();
        }

        private static DateTime year1970 = new DateTime(1970, 1, 1);
        public static int CurrentSeconds()
        {
            TimeSpan ts = year1970 - DateTime.Now;
            return (int)ts.TotalSeconds *-1;
        }
    }
}
