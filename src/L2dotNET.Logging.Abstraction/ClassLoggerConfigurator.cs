using System;
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
    }
}
