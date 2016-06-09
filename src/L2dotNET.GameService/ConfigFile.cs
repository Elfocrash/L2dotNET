using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace L2dotNET.GameService
{
    class ConfigFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigFile));

        private readonly FileInfo File;
        public SortedList<string, string> _topics;

        public ConfigFile(string Path)
        {
            File = new FileInfo(Path);
            _topics = new SortedList<string, string>();
            reload();
        }

        public void reload()
        {
            StreamReader reader = new StreamReader(File.FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (line.Length == 0)
                    continue;

                if (line.StartsWith(";", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                _topics.Add(line.Split('=')[0], line.Split('=')[1]);
            }

            log.Info($"Config file {File.Name} loaded with {_topics.Count} parameters.");
        }

        public string getProperty(string value, string defaultprop)
        {
            string ret;
            try
            {
                ret = _topics[value];
            }
            catch
            {
                ret = null;
                log.Info($"config: error, parameter {value} was not found");
            }

            return ret ?? defaultprop;
        }
    }
}