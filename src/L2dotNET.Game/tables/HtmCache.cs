using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.Compression;
using log4net;

namespace L2dotNET.Game.tables
{
    public class HtmCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HtmCache));
        private static volatile HtmCache instance;
        private static object syncRoot = new object();

        public static HtmCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new HtmCache();
                        }
                    }
                }

                return instance;
            }
        }

        private SortedList<string, string> _htms = new SortedList<string, string>();
        public string versionHeader = "dbhtml_V1", file = "";
        Stream stream;
        GZipStream gz;
        bool complete = false;
        public HtmCache()
        {

        }

        public void Initialize()
        {
            try
            {
                stream = File.OpenRead(@"html\html_total.ht");
                gz = new GZipStream(stream, CompressionMode.Decompress);
                file = "html_total.ht";
            }
            catch (System.Exception)
            {
                log.Error("html_total pack was not found!");
                return;
            }

            new Thread(readNext).Start();
        }

        public void readNext()
        {
            byte[] header = new byte[9];
            gz.Read(header, 0, header.Length);
            if (Encoding.UTF8.GetString(header) != versionHeader)
            {
                log.Error("HtmCache: html_total.ht with unknown format");
                return;
            }

            byte[] fileCnt = new byte[4];
            gz.Read(fileCnt, 0, fileCnt.Length);
            int files = BitConverter.ToInt32(fileCnt, 0);
            for (int a = 0; a < files; a++)
            {
                byte[] fnameLenByte = new byte[2];
                gz.Read(fnameLenByte, 0, fnameLenByte.Length);
                short fnameLen = BitConverter.ToInt16(fnameLenByte, 0);
                byte[] fnameByte = new byte[fnameLen];
                gz.Read(fnameByte, 0, fnameByte.Length);
                string fname = Encoding.UTF8.GetString(fnameByte);

                byte[] fdbLenByte = new byte[4];
                gz.Read(fdbLenByte, 0, fdbLenByte.Length);
                int fdbLen = BitConverter.ToInt32(fdbLenByte, 0);
                byte[] fdbByte = new byte[fdbLen];
                gz.Read(fdbByte, 0, fdbByte.Length);
                string fdb = Encoding.UTF8.GetString(fdbByte);

                _htms.Add(fname, fdb);
            }
            stream.Close();
            gz.Close();
            log.Info("HtmCache: Reading "+file+" complete. " + files + " files.");

            if (complete)
                return;

            //try
            //{
            //    stream = File.OpenRead(@"html\html_admin.ht");
            //    gz = new GZipStream(stream, CompressionMode.Decompress);
            //    file = "html_admin.ht";
            //}
            //catch (System.Exception)
            //{
            //    CLogger.error(file+" pack was not found!");
            //    return;
            //}

            //new Thread(readNext).Start();
            complete = true;
        }

        public string getHtm(string locale, string file)
        {
            //switch (locale)
            //{
            //    case "en":
            //        locale = "";
            //        break;
            //    case "ru":
            //        locale = "-" + locale;
            //        if (!File.Exists(@"html" + locale + "\\" + file))
            //            locale = "";
            //        break;
            //}

            //if (!File.Exists(@"html"+locale+"\\" + file))
            //{
            //    return "htm file " + file + " not exists";
            //}

            //if (_htms.ContainsKey(file + locale))
            //{
            //    return _htms[file + locale];
            //}

            //string text = File.ReadAllText(@"html" + locale + "\\" + file);
            //_htms.Add(file+ locale, text);
            if (_htms.ContainsKey(file))
                return _htms[file];
            else
                return "Html file was not found "+file;
        }

        public string getHtmAdmin(string locale, string file)
        {
            //switch (locale)
            //{
            //    case "en":
            //        locale = "";
            //        break;
            //    case "ru":
            //        locale = "-" + locale;
            //        if (!File.Exists(@"html_admin" + locale + "\\" + file))
            //            locale = "";
            //        break;
            //}

            //if (!File.Exists(@"html_admin" + locale + "\\" + file))
            //{
            //    return "htm file " + file + " not exists";
            //}

            //if (_htms.ContainsKey(file + locale))
            //{
            //    return _htms[file + locale];
            //}

          //  string text = File.ReadAllText(@"html_admin" + locale + "\\" + file);
          //  _htms.Add(file + locale, text);

            if (_htms.ContainsKey(file))
                return _htms[file];
            else
                return "Html file was not found " + file;
        }

        public void reload()
        {
            _htms.Clear();
        }

    }
}
