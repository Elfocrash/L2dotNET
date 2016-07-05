using System;
using System.IO;
using log4net;
using L2dotNET.GameService.Model.Zones;
using L2dotNET.GameService.Model.Zones.Classes;
using L2dotNET.GameService.Model.Zones.Forms;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Tables
{
    class ZoneTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ZoneTable));
        private static volatile ZoneTable _instance;
        private static readonly object SyncRoot = new object();

        public static ZoneTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ZoneTable();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            int ctx = 0,
                cta = 0;
            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\areadata_cur.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    if (line.StartsWithIgnoreCase("//"))
                    {
                        continue;
                    }

                    if (line.StartsWithIgnoreCase("area_begin"))
                    {
                        L2Zone zone = null;
                        ZoneTemplate template = new ZoneTemplate();
                        string[] d = line.Split('\t');

                        for (int i = 1; i < d.Length; i++)
                        {
                            if (d[i].EqualsIgnoreCase("area_end"))
                            {
                                continue;
                            }

                            string param = d[i].Split('=')[0];
                            string val = d[i].Substring(param.Length + 1);

                            switch (param)
                            {
                                case "name":
                                    template.Name = val;
                                    break;
                                case "map_no":
                                    template.MapNo = val;
                                    break;
                                case "type":
                                {
                                    template.Type = (ZoneTemplate.ZoneType)Enum.Parse(typeof(ZoneTemplate.ZoneType), val);

                                    switch (template.Type)
                                    {
                                        case ZoneTemplate.ZoneType.BattleZone:
                                            zone = new battle_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.PeaceZone:
                                            zone = new peace_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.Water:
                                            zone = new water();
                                            break;
                                        case ZoneTemplate.ZoneType.NoRestart:
                                            zone = new no_restart();
                                            break;
                                        case ZoneTemplate.ZoneType.SsqZone:
                                            zone = new ssq_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.MotherTree:
                                            zone = new mother_tree();
                                            template.HpRegenBonus = 2;
                                            template.MpRegenBonus = 1;
                                            break;
                                        case ZoneTemplate.ZoneType.Damage:
                                            zone = new damage();
                                            template.DamageOnHp = 200;
                                            template.DamageOnMp = 0;
                                            break;
                                        case ZoneTemplate.ZoneType.Poison:
                                            zone = new poison();
                                            template.SetSkill("s_area_a_speed_down");
                                            break;
                                        case ZoneTemplate.ZoneType.Swamp:
                                            zone = new swamp();
                                            template.MoveBonus = -50;
                                            break;
                                        case ZoneTemplate.ZoneType.InstantSkill:
                                            zone = new instant_skill();
                                            break;
                                    }
                                }

                                    break;
                                case "affect_race":
                                    template.AffectRace = val;
                                    break;
                                case "entering_message_no":
                                    template.EnteringMessageNo = int.Parse(val);
                                    break;
                                case "leaving_message_no":
                                    template.LeavingMessageNo = int.Parse(val);
                                    break;
                                case "range":
                                    template.SetRange(val);
                                    break;
                                case "move_bonus":
                                    template.MoveBonus = int.Parse(val);
                                    break;
                                case "default_status":
                                    template.DefaultStatus = val.EqualsIgnoreCase("on");
                                    break;
                                case "event_id":
                                    template.EventId = int.Parse(val);
                                    break;
                                case "damage_on_hp":
                                    template.DamageOnHp = int.Parse(val);
                                    break;
                                case "damage_on_mp":
                                    template.DamageOnMp = int.Parse(val);
                                    break;
                                case "message_no":
                                    template.MessageNo = int.Parse(val);
                                    break;
                                case "target":
                                    template.Target = (ZoneTemplate.ZoneTarget)Enum.Parse(typeof(ZoneTemplate.ZoneTarget), val);
                                    break;
                                case "skill_prob":
                                    template.SkillProb = int.Parse(val);
                                    break;
                                case "unit_tick":
                                    template.UnitTick = int.Parse(val);
                                    break;
                                case "initial_delay":
                                    template.InitialDelay = int.Parse(val);
                                    break;
                                case "skill_list":
                                    template.SetSkillList(val);
                                    break;
                                case "skill_name":
                                    template.SetSkill(val.Substring(1).Replace("]", ""));
                                    break;
                                case "exp_penalty_per":
                                    template.ExpPenaltyPer = int.Parse(val);
                                    break;
                                case "item_drop":
                                    template.ItemDrop = val.EqualsIgnoreCase("on");
                                    break;
                            }
                        }

                        zone.Name = template.Name;
                        zone.Template = template;
                        zone.Territory = new ZoneNPoly(template.X, template.Y, template.Z1, template.Z2);
                        cta++;
                        for (int i = 0; i < template.X.Length; i++)
                        {
                            L2WorldRegion region = L2World.Instance.GetRegion(template.X[i], template.Y[i]);
                            if (region != null)
                            {
                                ctx++;
                            }
                            else
                            {
                                Log.Error($"AreaTable: null region at {template.X[i]} {template.Y[i]} for zone {zone.Name}");
                            }
                        }
                    }
                }
            }

            Log.Info("AreaTable: intercepted " + ctx + " regions with " + cta + " zones");
        }
    }
}