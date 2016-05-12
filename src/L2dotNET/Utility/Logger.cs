using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Logger source.
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// Source unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Source is logger.
        /// </summary>
        Logger,

        /// <summary>
        /// Source is current service.
        /// </summary>
        Service,

        /// <summary>
        /// Source is related to inner network.
        /// </summary>
        InnerNetwork,

        /// <summary>
        /// Source is related to outer network.
        /// </summary>
        OuterNetwork,

        /// <summary>
        /// Source is <see cref="L2.Net.Network.Services.Firewall"/> object.
        /// </summary>
        Firewall,
        /// <summary>
        /// <see cref="L2.Net.Network.Services.Listener"/> class.
        /// </summary>
        Listener,
        /// <summary>
        /// Debug message.
        /// </summary>
        Debug,
        /// <summary>
        /// Data provider message.
        /// </summary>
        DataProvider,
        /// <summary>
        /// Data provider in shadow mode.
        /// </summary>
        DataProviderShadow,
        /// <summary>
        /// Geodata engine message.
        /// </summary>
        Geodata,
        /// <summary>
        /// Geodata engine message in shadow mode.
        /// </summary>
        GeodataShadow,
        /// <summary>
        /// World message.
        /// </summary>
        World,
        /// <summary>
        /// World message in shadow mode.
        /// </summary>
        WorldShadow,
        /// <summary>
        /// Scripts compiler message.
        /// </summary>
        ScriptsCompiler
    }

    /// <summary>
    /// Represents simple logger class.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Common logs directory.
        /// </summary>
        private static readonly string CommonLogsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        /// <summary>
        /// Output logs directory.
        /// </summary>
        private static readonly string OutLogsDirectory = Path.Combine(CommonLogsDirectory, "out");

        /// <summary>
        /// Exceptions logs directory.
        /// </summary>
        private static readonly string ExceptionsLogDirectory = Path.Combine(CommonLogsDirectory, "ex");

        /// <summary>
        /// Common output stream writer.
        /// </summary>
        private static StreamWriter Output;

        /// <summary>
        /// Writes data to console and common output file.
        /// </summary>
        /// <param name="append">True, if current message must continue previous.</param>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public static void Write(bool append, string format, params object[] args)
        {
            string s = String.Format(format, args);

            if (!append)
                s = FormatOutputString(s);

            Console.Write(s);
        }

        /// <summary>
        /// Appends <see cref="Environment.NewLine"/> value to data, writes it to console and common output file.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public static void EndWrite(string format, params object[] args)
        {
            Write(true, String.Format(format + Environment.NewLine, args));
        }

        /// <summary>
        /// Writes line to console and common output file.
        /// </summary>
        /// <param name="src">Logs <see cref="Source"/>.</param>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public static void WriteLine(Source src, string format, params object[] args)
        {
            format = String.Format(format, args);
            string s = String.Format("[{0}] {1}", src, format);

            switch (src)
            {
                case Source.DataProviderShadow:
                case Source.GeodataShadow:
                    break;
                default:
                    WriteLine(s);
                    break;
            }
        }

        /// <summary>
        /// Writes line to console and common output file.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public static void WriteLine(string format, params object[] args)
        {
            string s = args.Length > 0 ? String.Format(format, args) : format;

            s = FormatOutputString(s);

            Console.WriteLine(s);

            WriteOutputLine(s);
        }

        /// <summary>
        /// Stores <see cref="System.Exception"/> data and shows message to console.
        /// </summary>
        /// <param name="e">Occurred <see cref="System.Exception"/>.</param>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public static void Exception(Exception e, string format, params object[] args)
        {
            string s = String.Format(format, args);
            s = String.Format("{0}{1}{2}", s, Environment.NewLine, FormatException(e));
            s = FormatOutputString(s);

            Console.WriteLine(s);

            s = String.Format("{0}{1}{2}", GetSystemSummary(), Environment.NewLine, s);

            WriteException(e.GetType().ToString(), s);
        }

        /// <summary>
        /// Stores <see cref="System.Exception"/> data and shows message on console.
        /// </summary>
        /// <param name="e">Occurred <see cref="System.Exception"/>.</param>
        public static void Exception(Exception e)
        {
            Exception(e, String.Empty);
        }

        /// <summary>
        /// Formats <see cref="System.Exception"/> object.
        /// </summary>
        /// <param name="e"><see cref="System.Exception"/> to format data about.</param>
        /// <returns>Formatted <see cref="System.Exception"/> data.</returns>
        private static string FormatException(Exception e)
        {
            if (e == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} occurred on {1}{2}", e.GetType(), ServiceTime.CurrentTime, Environment.NewLine);

            if (!String.IsNullOrEmpty(e.Message))
                sb.AppendFormat("Message: {0}{1}", e.Message, Environment.NewLine);
            if (!String.IsNullOrEmpty(e.StackTrace))
                sb.AppendFormat("StackTrace: {0}{1}", e.StackTrace, Environment.NewLine);
            if (e.InnerException != null)
            {
                sb.AppendLine("Inner exception data:");

                if (!String.IsNullOrEmpty(e.InnerException.Message))
                    sb.AppendFormat("\tMessage: {0}{1}", e.InnerException.Message, Environment.NewLine);

                if (!String.IsNullOrEmpty(e.InnerException.StackTrace))
                    sb.AppendFormat("\tStackTrace: {0}{1}", e.InnerException.StackTrace, Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets system information.
        /// </summary>
        /// <returns>Formatted system summary.</returns>
        private static string GetSystemSummary()
        {
            StringBuilder m_StrBuilder = new StringBuilder();
            m_StrBuilder.AppendFormat("Date: {0}\r\n", ServiceTime.CurrentTime);
            m_StrBuilder.AppendFormat("OS: {0}\r\n", Environment.OSVersion);
            m_StrBuilder.AppendFormat("Environment version: {0}\r\n", Environment.Version.ToString());
            m_StrBuilder.AppendFormat("Processors count: {0}\r\n", Environment.ProcessorCount);
            m_StrBuilder.AppendFormat("Working set: {0} bytes\r\n", Environment.WorkingSet);
            m_StrBuilder.AppendFormat("Domain name: {0}\r\n", AppDomain.CurrentDomain.FriendlyName);
            m_StrBuilder.AppendFormat("Service Uptime: {0}\r\n", ServiceTime.ServiceUptimeAsString);
            m_StrBuilder.Append(Environment.NewLine);
            return m_StrBuilder.ToString();
        }

        /// <summary>
        /// Initializes logger.
        /// </summary>
        public static void Initialize()
        {
            EnsureDirectiries();
            Output = new StreamWriter(Path.Combine(OutLogsDirectory, String.Format("{0}.log", ServiceTime.CurrentTime.ToString().Replace(":", "-").Replace("/", "-"))), true);
            WriteLine(Source.Logger, "Initialized.");
        }

        /// <summary>
        /// Writes message to common output file.
        /// </summary>
        /// <param name="s">Message to write.</param>
        private static void WriteOutputLine(string s)
        {
            if (Output != null)
            {
                Output.WriteLine(s);
                Output.Flush();
            }
        }

        /// <summary>
        /// Writes message to common output file.
        /// </summary>
        /// <param name="s">Message to write.</param>
        private static void WriteOutput(string s)
        {
            if (Output != null)
            {
                Output.Write(s);
                Output.Flush();
            }
        }

        /// <summary>
        /// Verifies that used directories exist.
        /// </summary>
        private static void EnsureDirectiries()
        {
            if (!Directory.Exists(CommonLogsDirectory))
                Directory.CreateDirectory(CommonLogsDirectory);
            if (!Directory.Exists(OutLogsDirectory))
                Directory.CreateDirectory(OutLogsDirectory);
            if (!Directory.Exists(ExceptionsLogDirectory))
                Directory.CreateDirectory(ExceptionsLogDirectory);
        }

        /// <summary>
        /// Writes <see cref="System.Exception"/> data to it's file.
        /// </summary>
        /// <param name="type">String representation of <see cref="System.Exception"/> <see cref="System.Type"/>.</param>
        /// <param name="data"><see cref="System.Exception"/> data.</param>
        private static void WriteException(string type, string data)
        {
            String fp = String.Format("{0}-{1}.ex", type, ServiceTime.CurrentTime.ToString().Replace(":", "-").Replace("/", "-"));

            using (StreamWriter sw = new StreamWriter(Path.Combine(ExceptionsLogDirectory, fp), true, Encoding.Unicode))
            {
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// Appends date and time to provided <see cref="string"/>.
        /// </summary>
        /// <param name="s"><see cref="string"/> to format.</param>
        /// <returns>Formatted <see cref="string"/> object.</returns>
        private static string FormatOutputString(string s)
        {
            return String.Format("{0} > {1}", ServiceTime.CurrentTime, s);
        }
    }
}
