using System.Collections.Generic;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Models.npcs;
using L2dotNET.Services.Contracts;
using Ninject;
using System.Linq;

namespace L2dotNET.tables
{
    public class SpawnTable
    {
        [Inject]
        public IServerService ServerService => GameServer.Kernel.Get<IServerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(SpawnTable));
        private static volatile SpawnTable _instance;
        private static readonly object SyncRoot = new object();
        

        public static SpawnTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new SpawnTable();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            List<SpawnlistContract> spawnsList = ServerService.GetAllSpawns();

            spawnsList.ForEach((spawn) =>
            {
                L2Spawn l2Spawn =
                new L2Spawn(NpcTable.Instance.GetTemplate(spawn.TemplateId))
                {
                    Location = new SpawnLocation(spawn.LocX, spawn.LocY, spawn.LocZ, spawn.Heading)
                };
                l2Spawn.Spawn(false);
                Spawns.Add(l2Spawn);
            });
            
            Log.Info($"SpawnTable: Spawned: {spawnsList.Count} npcs.");
           
        }

        public readonly List<L2Spawn> Spawns = new List<L2Spawn>(50000);

    }
}