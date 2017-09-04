using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using L2dotNET.Models.GameModels;
using L2dotNET.Utility;

namespace L2dotNET.tables
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
                if (_instance != null)
                    return _instance;

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
            if (!Config.Config.Instance.ServerConfig.LazyHtmlCache)
            {
                BuildHtmCache();
                Log.Info($"HtmCache: Cache Built. Loaded {_htmCache.Count} files.");
            }
            else
            {
                Log.Info($"HtmCache : Lazy Cached {_htmFiles.Count} files.");
            }
        }

        public void BuildHtmCache()
        {
            foreach (string file in _htmFiles)
            {
                CacheFile(file);
            }
        }

        public string GetHtmByFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            L2Html html = _htmCache.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(filename));
            if (Config.Config.Instance.ServerConfig.LazyHtmlCache && html == null)
            {
                //Fallback for non loaded files
                string predictedFile = _htmFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == filename);
                if (predictedFile != null)
                {
                    CacheFile(predictedFile);
                    html = _htmCache.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(filename));
                }
            }

            return html != null ? html.Content : string.Empty;
        }

        public void CacheFile(string filepath)
        {
            string content = File.ReadAllText(filepath, Encoding.UTF8);
            content = content.Replace("\r\n", "\n");
            _htmCache.Add(new L2Html(Path.GetFileNameWithoutExtension(filepath), content, filepath));
        }

        public string GetHtmByFilepath(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            L2Html html = _htmCache.FirstOrDefault(x => x.Filepath.EqualsIgnoreCase(filename));
            if (Config.Config.Instance.ServerConfig.LazyHtmlCache && html == null)
            {
                //Fallback for non loaded files
                string predictedFile = _htmFiles.FirstOrDefault(f => f == filename);
                if (predictedFile != null)
                {
                    CacheFile(predictedFile);
                    //Try Again
                    html = _htmCache.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(filename));
                }
            }
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