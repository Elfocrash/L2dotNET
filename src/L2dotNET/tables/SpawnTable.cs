using System.Collections.Generic;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Models.npcs;
using L2dotNET.Services.Contracts;
using Ninject;
using System.Linq;
using System.Timers;
using System;

namespace L2dotNET.tables
{
    public class SpawnTable
    {
        [Inject]
        public IServerService ServerService => GameServer.Kernel.Get<IServerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(SpawnTable));
        private static volatile SpawnTable _instance;
        private static readonly object SyncRoot = new object();
        private Timer RespawnTimerTask;

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
                    Location = new SpawnLocation(spawn.LocX, spawn.LocY, spawn.LocZ, spawn.Heading, spawn.RespawnDelay)
                };
                l2Spawn.Spawn(false);
                Spawns.Add(l2Spawn);
            });

            Log.Info($"SpawnTable: Spawned {spawnsList.Count} npcs.");

            if (Config.Config.Instance.GameplayConfig.NpcConfig.Misc.AutoMobRespawn)
            {
                RespawnTimerTask = new Timer();
                RespawnTimerTask.Elapsed += new ElapsedEventHandler(RespawnTimerTick);
                RespawnTimerTask.Interval = 2000;
                RespawnTimerTask.Start();

                Log.Info($"SpawnTable: Started RespawnTimerTask.");
            }
        }

        private void RespawnTimerTick(object sender, ElapsedEventArgs e)
        {
            foreach (var kvp in RespawnDict.ToArray())
            {
                long elapsedTicks = DateTime.Now.Ticks - kvp.Value.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (elapsedSpan.TotalMilliseconds >= kvp.Key.Location.RespawnDelay)
                {
                    kvp.Key.Spawn(true);
                    DeRegisterRespawn(kvp.Key);
                }
            }
        }

        public void RegisterRespawn(L2Spawn spawn)
        {
            RespawnDict.Add(spawn,DateTime.Now);
        }

        private void DeRegisterRespawn(L2Spawn spawn)
        {
            RespawnDict.Remove(spawn);
        }

        public readonly List<L2Spawn> Spawns = new List<L2Spawn>(50000);
        private readonly Dictionary<L2Spawn,DateTime> RespawnDict = new Dictionary<L2Spawn, DateTime>();
    }
}