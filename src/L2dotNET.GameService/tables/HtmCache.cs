using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Tables
{
    public class HtmCache
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HtmCache));
        private static volatile HtmCache _instance;
        private static readonly object SyncRoot = new object();

        private List<L2Html> _htmCache;
        private List<string> _htmFiles;

        public static HtmCache Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new HtmCache();
                    }

                return _instance;
            }
        }

        public void Initialize()
        {
            _htmCache = new List<L2Html>();
            _htmFiles = DirSearch("./html/");
            BuildHtmCache();
            Log.Info($"HtmCache: Cache Built. Loaded {_htmCache.Count} files.");
        }

        public void BuildHtmCache()
        {
            foreach (string file in _htmFiles)
            {
                string content = File.ReadAllText(file, Encoding.UTF8);
                content = content.Replace("\r\n", "\n");
                _htmCache.Add(new L2Html(Path.GetFileNameWithoutExtension(file), content, file));
            }
        }

        public string GetHtmByFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            L2Html html = _htmCache.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(filename));
            return html != null ? html.Content : string.Empty;
        }

        private List<string> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(sDir));
                foreach (string d in Directory.GetDirectories(sDir))
                    files.AddRange(DirSearch(d));
            }
            catch (Exception excpt)
            {
                Log.Error(excpt.Message);
            }

            return files;
        }
    }
}