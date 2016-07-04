using System.Linq;
using log4net;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Tables
{
    sealed class IdFactory
    {
        [Inject]
        public IServerService ServerService
        {
            get { return GameServer.Kernel.Get<IServerService>(); }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(IdFactory));

        private static volatile IdFactory _instance;
        private static readonly object SyncRoot = new object();

        public const int IdMin = 0x10000000,
                         IdMax = 0x7FFFFFFF;

        private int _currentId = 1;

        public static IdFactory Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new IdFactory();
                    }

                return _instance;
            }
        }

        public int NextId()
        {
            _currentId++;
            return _currentId;
        }

        public void Initialize()
        {
            _currentId = ServerService.GetPlayersObjectIdList().DefaultIfEmpty(IdMin).Max();

            Log.Info($"idfactory: used ids {_currentId}");
        }
    }
}