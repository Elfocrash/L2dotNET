using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Command;
using L2dotNET.GameService.Commands;
using L2dotNET.GameService.tables.admin_bypass;

namespace L2dotNET.GameService.Handlers
{
    public class AdminCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminCommandHandler));
        private readonly SortedList<string, AAdminCommand> commands = new SortedList<string, AAdminCommand>();
        private ABTeleport Teleports;

        private static volatile AdminCommandHandler instance;
        private static readonly object syncRoot = new object();

        public static AdminCommandHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AdminCommandHandler();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            Teleports = new ABTeleport();

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

            log.Info("AdminAccess: loaded " + commands.Count + " commands.");
        }

        public void request(L2Player admin, string alias)
        {
            string cmd = alias;
            if (alias.Contains(" "))
                cmd = alias.Split(' ')[0];

            if (!commands.ContainsKey(cmd))
            {
                admin.sendMessage("Command " + cmd + " not exists.");
                admin.sendActionFailed();
                return;
            }

            AAdminCommand processor = commands[cmd];
            try
            {
                processor.Use(admin, alias);
            }
            catch (Exception sss)
            {
                admin.sendMessage("Probably syntax eror.");
                log.Error(sss);
            }
        }

        private void Register(AAdminCommand processor)
        {
            commands.Add(processor.Cmd, processor);
        }

        public AdminCommandHandler() { }

        public void ProcessBypass(L2Player player, int ask, int reply)
        {
            switch (ask)
            {
                case 1: // телепорт группы
                    Teleports.ShowGroup(player, reply);
                    break;
                case 2: // телепорт из группы по ячейке
                    Teleports.Use(player, reply);
                    break;
                case 3: // список телепорт групп
                    Teleports.ShowGroupList(player);
                    break;
            }
        }

        public void ProcessBypassTp(L2Player player, int x, int y, int z)
        {
            player.teleport(x, y, z);
        }
    }
}