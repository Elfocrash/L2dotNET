using System;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;

namespace L2dotNET.Utility
{
    public static class StringHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>Verify if the given name matches with the regular expression pattern.</summary>
        /// <param name="name">The name to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsValidName(string name, string pattern)
        {
            try
            {
                return new Regex(pattern).IsMatch(name);
            }
            catch (Exception ex)
            {
                Log.Error($"Method: {nameof(IsValidName)}. Message: \'{ex.Message}\' (Parameters: \'{nameof(name)}\' = {name}, \'{nameof(pattern)}\' = {pattern})");
            }

            return false;
        }

        /// <summary>Verify if the given name matches with the regular expression pattern.</summary>
        /// <param name="name">The name to search for a match.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsValidPlayerName(string name)
        {
            return IsValidName(name, "^[A-Za-z0-9]{1,16}$");
        }
    }
}
