using System;
using L2dotNET.Logging.Abstraction;

namespace L2dotNet.Logging.Abstraction
{
    public static class LoggingExtensions
    {
        public static void Exception(this ILog log, Exception ex)
        {
            log.ErrorException(ex.Message, ex);
        }
    }
}
