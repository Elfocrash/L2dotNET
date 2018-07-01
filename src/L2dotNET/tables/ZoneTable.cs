using NLog;

namespace L2dotNET.Tables
{
    class ZoneTable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static volatile ZoneTable _instance;
        private static readonly object SyncRoot = new object();

        public static ZoneTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ZoneTable();
                }

                return _instance;
            }
        }

        public void Initialize() { }
    }
}