using System;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace L2dotNET.Logging.Abstraction
{
    public static class ClassLoggerConfigurator
    {
        public static void ConfigureClassLogger(string logFilePath)
        {
            var configuration = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget();
            configuration.AddTarget("console", consoleTarget);

            var fileTarget = new AsyncTargetWrapper(new FileTarget()
            {
                FileName = logFilePath,
                Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}"
            });

            configuration.AddTarget("file", fileTarget);

            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${message}";
            
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = configuration;
        }

        public static void Halt(this Logger log, string msg, bool includeStackTrace = true)
        {
            log.Error(msg);

            if (includeStackTrace)
            {
                log.Error(Environment.StackTrace);
            }

            log.Info("Press ENTER to exit...");
            Console.Read();
            Environment.Exit(0);
        }

        public static void ErrorTrace(this Logger log, Exception ex, string msg = null, bool includeStackTrace = true)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(msg ?? "ErrorTrace: ");
            sb.AppendLine($"Message: {ex.Message}");

            Exception innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.AppendLine($"Inner Message: {innerEx.Message}");
                innerEx = innerEx.InnerException;
            }

            if (includeStackTrace)
            {
                sb.AppendLine(ex.StackTrace ?? ex.InnerException?.StackTrace ?? "Stack trace is empty");
            }

            log.Error(sb.ToString());
        }
    }
}
