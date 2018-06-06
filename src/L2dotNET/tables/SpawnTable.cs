using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Models.Npcs;
using L2dotNET.Services.Contracts;
using System.Linq;
using System.Timers;
using System;
using L2dotNET.Logging.Abstraction;

namespace L2dotNET.Tables
{
    public class SpawnTable : IInitialisable
    {
        private readonly IServerService _serverService;

        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();
        private Timer RespawnTimerTask;
        private readonly IdFactory _idFactory;
        private readonly Config.Config _config;
        public bool Initialised { get; private set; }

        public SpawnTable(IServerService serverService, IdFactory idFactory, Config.Config config)
        {
            _config = config;
            _serverService = serverService;
            _idFactory = idFactory;
        }

        public void Initialise()
        {
            if(Initialised) return;
            
            List<SpawnlistContract> spawnsList = _serverService.GetAllSpawns();

            spawnsList.ForEach((spawn) =>
            {
                L2Spawn l2Spawn =
                new L2Spawn(NpcTable.Instance.GetTemplate(spawn.TemplateId), _idFactory, this)
                {
                    Location = new SpawnLocation(spawn.LocX, spawn.LocY, spawn.LocZ, spawn.Heading, spawn.RespawnDelay)
                };
                l2Spawn.Spawn(false);
                Spawns.Add(l2Spawn);
            });

            Log.Info($"Spawned {spawnsList.Count} npcs.");

            if (_config.GameplayConfig.NpcConfig.Misc.AutoMobRespawn)
            {
                RespawnTimerTask = new Timer();
                RespawnTimerTask.Elapsed += new ElapsedEventHandler(RespawnTimerTick);
                RespawnTimerTask.Interval = 2000;
                RespawnTimerTask.Start();

                Log.Info($"Started RespawnTimerTask.");
            }

            Initialised = true;
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