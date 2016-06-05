using System;
using System.IO;
using System.Xml;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.managers
{
    public class ZoneManager
    {
        private static volatile ZoneManager instance;
        private static readonly object syncRoot = new object();

        public ZoneManager()
        {

        }

        public void Initialize()
        {
            L2WorldRegion[][] worldRegions = L2World.Instance.GetWorldRegions();

            try
            {
                XmlDocument doc = new XmlDocument();
                int fileCounter = 0;
                string[] xmlFilesArray = Directory.GetFiles(@"data\xml\zones\");
                for (int i = 0; i < xmlFilesArray.Length; i++)
                {

                }
            }
            catch(Exception e)
            {
                return;
            }

            int size = 0;
            
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
