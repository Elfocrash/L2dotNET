using System.Threading;
using System.Threading.Tasks;
using L2dotNET.Services.Contracts;
using NLog;

namespace L2dotNET.Tables
{
    public sealed class IdFactory : IInitialisable
    {
        private readonly IItemService _itemService;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public bool Initialised { get; private set; }

        // TODO: Investigate the purpose of that
        // public const int IdMin = 0x10000000;
        // public const int IdMax = 0x7FFFFFFF;

        private int _currentId = 1;

        public IdFactory(IItemService itemService)
        {
            _itemService = itemService;
        }

        public int NextId()
        {
            return Interlocked.Increment(ref _currentId);
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            _currentId = _itemService.GetMaxItemId();
            Log.Info($"Used IDs {_currentId}.");
            Initialised = true;
        }
    }
}