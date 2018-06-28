using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Tables
{
    public sealed class IdFactory : IInitialisable
    {
        private readonly IServerService _serverService;

        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();
        public bool Initialised { get; private set; }

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

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            // TODO: Fix that after itemService would be reviewed
            //_currentId = _serverService.GetPlayersItemsObjectIdList().DefaultIfEmpty(IdMin).Max(); -- this is so fckng stupid
            Log.Info($"Used IDs {_currentId}.");
            Initialised = true;
        }
    }
}