using System.Linq;
using log4net;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Tables
{
    public sealed class IdFactory
    {
        private readonly IServerService _serverService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(IdFactory));

        private static volatile IdFactory _instance;
        private static readonly object SyncRoot = new object();

        public const int IdMin = 0x10000000,
                         IdMax = 0x7FFFFFFF;

        private int _currentId = 1;

        public IdFactory(IServerService serverService)
        {
            _serverService = serverService;
        }


        public int NextId()
        {
            _currentId++;
            return _currentId;
        }

        public void Initialize()
        {
            
            _currentId = _serverService.GetPlayersItemsObjectIdList().DefaultIfEmpty(IdMin).Max();
            Log.Info($"Used IDs {_currentId}.");
        }
    }
}