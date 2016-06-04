using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.tables.admin;
using L2dotNET.GameService.tables.admin_bypass;

namespace L2dotNET.GameService.tables
{
    public class AdminAccess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminAccess));
        private SortedList<string, _adminAlias> _commands = new SortedList<string, _adminAlias>();
        private ABTeleport Teleports;

        private static volatile AdminAccess instance;
        private static object syncRoot = new object();

        public static AdminAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AdminAccess();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            Teleports = new ABTeleport();

            register(new AA_actorcount());
            register(new AA_addbookmark());
            register(new AA_allgm());
            register(new AA_announce());
            register(new AA_attack());
            register(new AA_camera());
            register(new AA_char_stop());
            register(new AA_check_bot());
            register(new AA_create_pet());
            register(new AA_declare_clan_war());
            register(new AA_declare_truce());
            register(new AA_defend());
            register(new AA_delannounce());
            register(new AA_delskill());
            register(new AA_diet());
            register(new AA_disband());
            register(new AA_disband2());
            register(new AA_earthquake());
            register(new AA_escape());
            register(new AA_event());
            register(new AA_getbookmark());
            register(new AA_gmliston());
            register(new AA_gmon());
            register(new AA_gmspeed());
            register(new AA_healthy());
            register(new AA_hennaequip());
            register(new AA_hennaunequip());
            register(new AA_hide());
            register(new AA_home());
            register(new AA_kick());
            register(new AA_killme());
            register(new AA_killnpc());
            register(new AA_load_event());
            register(new AA_load_npcsettings());
            register(new AA_npccount());
            register(new AA_ns());
            register(new AA_polymorph());
            register(new AA_qmove());
            register(new AA_quiet());
            register(new AA_range());
            register(new AA_recall());
            register(new AA_reset_alliance_name());
            register(new AA_reset_castle_owner());
            register(new AA_reset_clan_leader());
            register(new AA_reset_clan_name());
            register(new AA_reset_skill());
            register(new AA_ride_wyvern());
            register(new AA_send_gmroom());
            register(new AA_sendhome());
            register(new AA_servername());
            register(new AA_serverstat());
            register(new AA_set_castle_owner());
            register(new AA_set_door_status());
            register(new AA_set_hero());
            register(new AA_set_interval_announce());
            register(new AA_set_nobless());
            register(new AA_set_pledge_level());
            register(new AA_set_quick_siege());
            register(new AA_set_siege());
            register(new AA_set_siege_end());
            register(new AA_set_siege_period());
            register(new AA_set_skill_all());
            register(new AA_set_zzaga_hero());
            register(new AA_setai());
            register(new AA_setannounce());
            register(new AA_setbuilder());
            register(new AA_setclass());
            register(new AA_setparam());
            register(new AA_setquest());
            register(new AA_setskill());
            register(new AA_show_castle_info());
            register(new AA_showparty());
            register(new AA_snoop());
            register(new AA_ssq());
            register(new AA_ssq_info());
            register(new AA_stoplogin());
            register(new AA_stopsay());
            register(new AA_summon());
            register(new AA_summon2());
            register(new AA_summon3());
            register(new AA_telbookmark());
            register(new AA_teleport());
            register(new AA_teleport_to_npc());
            register(new AA_teleportto());
            register(new AA_test());
            register(new AA_tradeoff());
            register(new AA_tradeon());
            register(new AA_transform());
            register(new AA_undying());
            register(new AA_whisper());
            register(new AA_who());

            register(new AA_chat());
            register(new AA_spawn());
            log.Info("AdminAccess: loaded " + _commands.Count + " commands.");
        }

        public void request(L2Player admin, string alias)
        {
            string cmd = alias;
            if (alias.Contains(" "))
                cmd = alias.Split(' ')[0];

            if (!_commands.ContainsKey(cmd))
            {
                admin.sendMessage("Command " + cmd + " not exists.");
                admin.sendActionFailed();
                return;
            }

            _adminAlias processor = _commands[cmd];
            try
            {
                processor.use(admin, alias);
            }
            catch(Exception sss)
            {
                admin.sendMessage("Probably syntax eror.");
                log.Error(sss);
            }
        }

        private void register(_adminAlias processor)
        {
            _commands.Add(processor.cmd, processor);
        }

        public AdminAccess()
        {
            
        }

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
