using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.Commands.Admin;
using L2dotNET.model.player;
using L2dotNET.tables.admin_bypass;

namespace L2dotNET.Handlers
{
    public class AdminCommandHandler : IAdminCommandHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminCommandHandler));
        private readonly SortedList<string, AAdminCommand> _commands = new SortedList<string, AAdminCommand>();
        private ABTeleport _teleports;

        private static volatile AdminCommandHandler _instance;
        private static readonly object SyncRoot = new object();

        public static AdminCommandHandler Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AdminCommandHandler();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            _teleports = new ABTeleport();

            Register(new AdminAdmin());
            Register(new AdminAddSkill());
            Register(new AdminChat());
            Register(new AdminGiveAllSkills());
            Register(new AdminHeal());
            Register(new AdminRange());
            Register(new AdminSpawnEnchanted());
            Register(new AdminSpawnItem());
            Register(new AdminSpawnItemRange());
            Register(new AdminSpawnNpc());
            Register(new AdminTeleport());
            Register(new AdminTest());
            Register(new AdminTransform());
            Register(new AdminWhisper());

            Log.Info($"AdminAccess: loaded {_commands.Count} commands.");
        }

        public void Request(L2Player admin, string alias)
        {
            string cmd = alias;
            if (alias.Contains(" "))
                cmd = alias.Split(' ')[0];

            if (!_commands.ContainsKey(cmd))
            {
                admin.SendMessage($"Command {cmd} not exists.");
                admin.SendActionFailed();
                return;
            }

            AAdminCommand processor = _commands[cmd];
            try
            {
                processor.Use(admin, alias);
            }
            catch (Exception sss)
            {
                admin.SendMessage("Probably syntax eror.");
                Log.Error(sss);
            }
        }

        public void Register(AAdminCommand processor)
        {
            _commands.Add(processor.Cmd, processor);
        }

        public void ProcessBypass(L2Player player, int ask, int reply)
        {
            switch (ask)
            {
                case 1:
                    _teleports.ShowGroup(player, reply);
                    break;
                case 2:
                    _teleports.Use(player, reply);
                    break;
                case 3:
                    _teleports.ShowGroupList(player);
                    break;
            }
        }

        public void ProcessBypassTp(L2Player player, int x, int y, int z) { }
    }
}