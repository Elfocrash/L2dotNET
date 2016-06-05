using System;
using System.IO;
using System.Xml;
using log4net;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Managers
{
    public class ZoneManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ZoneManager));

        private static volatile ZoneManager instance;
        private static readonly object syncRoot = new object();

        public ZoneManager() { }

        public void Initialize()
        {
            L2WorldRegion[,] worldRegions = L2World.Instance.GetWorldRegions();

            try
            {
                XmlDocument doc = new XmlDocument();
                //int fileCounter = 0;
                string[] xmlFilesArray = Directory.GetFiles(@"data\xml\zones\");
                for (int i = 0; i < xmlFilesArray.Length; i++) { }
            }
            catch (Exception e)
            {
                log.Error($"ZoneManager: {e.Message}");
                return;
            }

            //int size = 0;
        }

        public static ZoneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ZoneManager();
                        }
                    }
                }

                return instance;
            }
        }
    }
}