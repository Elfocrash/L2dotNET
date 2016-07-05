using System;
using System.IO;
using log4net;
using L2dotNET.GameService.Model.Zones;
using L2dotNET.GameService.Model.Zones.Classes;
using L2dotNET.GameService.Model.Zones.Forms;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Tables
{
    class ZoneTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ZoneTable));
        private static volatile ZoneTable _instance;
        private static readonly object SyncRoot = new object();

        public static ZoneTable Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ZoneTable();
                    }

                return _instance;
            }
        }

        public void Initialize()
        {
       
        }
    }
}