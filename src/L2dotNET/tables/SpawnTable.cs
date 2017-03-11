using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.model.npcs;
using L2dotNET.Services.Contracts;
using L2dotNET.templates;
using L2dotNET.world;
using Ninject;

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
            var spawnsList = ServerService.GetAllSpawns();

            foreach (var spawn in spawnsList)
            {
                L2Spawn l2Spawn = new L2Spawn(NpcTable.Instance.GetTemplate(spawn.TemplateId));
                l2Spawn.Location = new SpawnLocation(spawn.LocX, spawn.LocY, spawn.LocZ, spawn.Heading);
                l2Spawn.Spawn(false);
            }

            Log.Info($"SpawnTable: Spawned: {spawnsList} npcs.");
        }

        public readonly List<L2Spawn> Spawns = new List<L2Spawn>();

    }
}