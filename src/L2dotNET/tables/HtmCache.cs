using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.DataContracts.GameModels;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Utility;

namespace L2dotNET.Tables
{
    public class HtmCache : IInitialisable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private List<L2Html> _htmCache;
        private List<string> _htmFiles;
        public bool Initialised { get; private set; }

        private readonly Config.Config _config;
        public HtmCache(Config.Config config)
        {
            _config = config;
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            _htmCache = new List<L2Html>();
            _htmFiles = DirSearch("./html");
            if (!_config.ServerConfig.LazyHtmlCache)
            {
                BuildHtmCache();
                Log.Info($"Cache Built. Loaded {_htmCache.Count} files.");
            }
            else
            {
                Log.Info($"Lazy Cached {_htmFiles.Count} files.");
            }

            Initialised = true;
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
            if (_config.ServerConfig.LazyHtmlCache && html == null)
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
            if (_config.ServerConfig.LazyHtmlCache && html == null)
            {
                //Fallback for non loaded files
                string predictedFile = _htmFiles.FirstOrDefault(f => f == filename);
                if (predictedFile != null)
                {
                    CacheFile(predictedFile);
                    //Try Again
                    html = _htmCache.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(Path.GetFileNameWithoutExtension(predictedFile)));
                }
            }
            return html != null ? html.Content : string.Empty;
        }

        private List<string> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            sDir += Path.AltDirectorySeparatorChar;

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