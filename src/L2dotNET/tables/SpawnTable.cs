using System.Collections.Generic;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Models.Npcs;
using L2dotNET.Services.Contracts;
using System.Linq;
using System.Timers;
using System;

namespace L2dotNET.Tables
{
    public class SpawnTable
    {
        private readonly IServerService _serverService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(SpawnTable));
        private Timer RespawnTimerTask;
        private readonly IdFactory _idFactory;
        private readonly SpawnTable _spawnTable;
        public SpawnTable(IServerService serverService, IdFactory idFactory)
        {
            _serverService = serverService;
            _idFactory = idFactory;
        }

        public void Initialize()
        {
            List<SpawnlistContract> spawnsList = _serverService.GetAllSpawns();

            spawnsList.ForEach((spawn) =>
            {
                L2Spawn l2Spawn =
                new L2Spawn(NpcTable.Instance.GetTemplate(spawn.TemplateId), _idFactory, _spawnTable)
                {
                    Location = new SpawnLocation(spawn.LocX, spawn.LocY, spawn.LocZ, spawn.Heading, spawn.RespawnDelay)
                };
                l2Spawn.Spawn(false);
                Spawns.Add(l2Spawn);
            });

            Log.Info($"Spawned {spawnsList.Count} npcs.");

            if (Config.Config.Instance.GameplayConfig.NpcConfig.Misc.AutoMobRespawn)
            {
                RespawnTimerTask = new Timer();
                RespawnTimerTask.Elapsed += new ElapsedEventHandler(RespawnTimerTick);
                RespawnTimerTask.Interval = 2000;
                RespawnTimerTask.Start();

                Log.Info($"Started RespawnTimerTask.");
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