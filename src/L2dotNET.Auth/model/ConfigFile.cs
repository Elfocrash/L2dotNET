using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace L2dotNET.Auth
{
    class ConfigFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigFile));
        private FileInfo File;
        public SortedList<string, string> _topics;

        public ConfigFile(string Path)
        {
            File = new FileInfo(Path);
            _topics = new SortedList<string, string>();
            Reload();
        }

        public void Reload()
        {
            using (StreamReader reader = new StreamReader(File.FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (line.Length == 0)
                        continue;

                    if (line.StartsWith(";"))
                        continue;

                    if (line.Split('=').Length == 2)
                        _topics.Add(line.Split('=')[0].Trim(), line.Split('=')[1].Trim());
                }
            }

            log.Info($"Config file { File.Name } loaded with { _topics.Count } parameters.");
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
                return defaultprop;
            }

            return ret == null ? defaultprop : ret;
        }
    }
}
