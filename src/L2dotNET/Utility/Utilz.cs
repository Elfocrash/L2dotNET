using L2dotNET.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using NLog;

namespace L2dotNET.Utility
{
    public static class Utilz
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static string CurrentTime => DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

        public static TimeSpan ProcessUptime => DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);

        public static string SystemSummary()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"Date: {CurrentTime}\r\n");
            strBuilder.Append($"OS: {Environment.OSVersion}\r\n");
            strBuilder.Append($"Environment version: {Environment.Version}\r\n");
            strBuilder.Append($"Processors count: {Environment.ProcessorCount}\r\n");
            strBuilder.Append($"Working set: {Environment.WorkingSet} bytes\r\n");
            strBuilder.Append($"Domain name: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            strBuilder.Append($"Service Uptime: {ProcessUptime}\r\n");
            strBuilder.Append(Environment.NewLine);
            return strBuilder.ToString();
        }

        public static double DistanceSq(double x1, double y1, double x2, double y2)
        {
            return Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2);
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(DistanceSq(x1, y1, x2, y2));
        }

        public static double LengthSq(double x, double y)
        {
            return Math.Pow(x, 2) + Math.Pow(y, 2);
        }

        public static double Length(double x, double y)
        {
            return Math.Sqrt(LengthSq(x, y));
        }

        public static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.BaseType != typeof(object)).ToArray();
        }
        
        public static T GetEnumFromString<T>(string value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>().Where(item => item.ToString(CultureInfo.InvariantCulture).ToLower().Equals(value.Trim().ToLower())))
                return item;

            return defaultValue;
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
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

        public static Packet ToPacket(this byte[] byteArray, int extraBytes = 0)
        {
            return new Packet(1 + extraBytes, byteArray);
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

        public static SortedList<TKey, TValue> ToSortedList<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            SortedList<TKey, TValue> ret = new SortedList<TKey, TValue>();

            foreach (TSource item in source)
                ret.Add(keySelector(item), valueSelector(item));

            return ret;
        }

        public static string GetTypeLower<T>(T obj)
        {
            Type t;
            if (obj == null)
                t = typeof(T);
            else
                t = obj.GetType();
            return t.Name.ToLower();
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIp in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIp))
                        return true;

                    // is local address
                    if (localIPs.Contains(hostIp))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return false;
        }

        public static bool IsLocalIpAddress(this EndPoint host)
        {
            return IsLocalIpAddress(((IPEndPoint)host).Address.ToString());
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}