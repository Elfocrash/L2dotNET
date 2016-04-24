using System;
using System.IO;
using L2dotNET.Game.logger;
using L2dotNET.Game.model.zones;
using L2dotNET.Game.model.zones.classes;
using L2dotNET.Game.model.zones.forms;
using L2dotNET.Game.world;

namespace L2dotNET.Game.tables
{
    class ZoneTable
    {
        private static ZoneTable zm = new ZoneTable();
        public static ZoneTable getInstance()
        {
            return zm;
        }

        public ZoneTable()
        {
            int ctx = 0, cta = 0;
            StreamReader reader = new StreamReader(new FileInfo(@"scripts\areadata_cur.txt").FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("//"))
                    continue;

                if (line.StartsWith("area_begin"))
                {
                    L2Zone zone = null;
                    ZoneTemplate template = new ZoneTemplate();
                    string[] d = line.Split('\t');

                    for (int i = 1; i < d.Length; i++)
                    {
                        if (d[i].Equals("area_end"))
                            continue;

                        string param = d[i].Split('=')[0];
                        string val = d[i].Substring(param.Length + 1);

                        switch (param)
                        {
                            case "name":
                                template.Name = val;
                                break;
                            case "map_no":
                                template._map_no = val;
                                break;
                            case "type":
                                {
                                    template.Type = (ZoneTemplate.ZoneType)Enum.Parse(typeof(ZoneTemplate.ZoneType), val);

                                    switch (template.Type)
                                    {
                                        case ZoneTemplate.ZoneType.battle_zone:
                                            zone = new battle_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.peace_zone:
                                            zone = new peace_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.water:
                                            zone = new water();
                                            break;
                                        case ZoneTemplate.ZoneType.no_restart:
                                            zone = new no_restart();
                                            break;
                                        case ZoneTemplate.ZoneType.ssq_zone:
                                            zone = new ssq_zone();
                                            break;
                                        case ZoneTemplate.ZoneType.mother_tree:
                                            zone = new mother_tree();
                                            template._hp_regen_bonus = 2;
                                            template._mp_regen_bonus = 1;
                                            break;
                                        case ZoneTemplate.ZoneType.damage:
                                            zone = new damage();
                                            template._damage_on_hp = 200;
                                            template._damage_on_mp = 0;
                                            break;
                                        case ZoneTemplate.ZoneType.poison:
                                            zone = new poison();
                                            template.setSkill("s_area_a_speed_down");
                                            break;
                                        case ZoneTemplate.ZoneType.swamp:
                                            zone = new swamp();
                                            template._move_bonus = -50;
                                            break;
                                        case ZoneTemplate.ZoneType.instant_skill:
                                            zone = new instant_skill();
                                            break;
                                    }
                                }
                                break;
                            case "affect_race":
                                template._affect_race = val;
                                break;
                            case "entering_message_no":
                                template._entering_message_no = int.Parse(val);
                                break;
                            case "leaving_message_no":
                                template._leaving_message_no = int.Parse(val);
                                break;
                            case "range":
                                template.setRange(val);
                                break;
                            case "move_bonus":
                                template._move_bonus = int.Parse(val);
                                break;
                            case "default_status":
                                template.DefaultStatus = val.Equals("on");
                                break;
                            case "event_id":
                                template._event_id = int.Parse(val);
                                break;
                            case "damage_on_hp":
                                template._damage_on_hp = int.Parse(val);
                                break;
                            case "damage_on_mp":
                                template._damage_on_mp = int.Parse(val);
                                break;
                            case "message_no":
                                template._message_no = int.Parse(val);
                                break;
                            case "target":
                                template._target = (ZoneTemplate.ZoneTarget)Enum.Parse(typeof(ZoneTemplate.ZoneTarget), val);
                                break;
                            case "skill_prob":
                                template._skill_prob = int.Parse(val);
                                break;
                            case "unit_tick":
                                template._unit_tick = int.Parse(val);
                                break;
                            case "initial_delay":
                                template._initial_delay = int.Parse(val);
                                break;
                            case "skill_list":
                                template.setSkillList(val);
                                break;
                            case "skill_name":
                                template.setSkill(val.Substring(1).Replace("]", ""));
                                break;
                            case "exp_penalty_per":
                                template._exp_penalty_per = int.Parse(val);
                                break;
                            case "item_drop":
                                template._item_drop = val.Equals("on");
                                break;
                        }
                    }
                    zone.Name = template.Name;
                    zone.Template = template;
                    zone.Territory = new ZoneNPoly(template._x, template._y, template._z1, template._z2);
                    cta++;
                    for (int i = 0; i < template._x.Length; i++)
                    {
                        L2WorldRegion region = L2World.Instance.GetRegion(template._x[i], template._y[i]);
                        if (region != null)
                        {
                            ctx++;
                           // region._zoneManager.addZone(zone);
                        }
                        else
                        {
                            CLogger.error("AreaTable: null region at " + template._x[i] + " " + template._y[i] + " for zone " + zone.Name);
                        }
                    }
                }

            }

            CLogger.info("AreaTable: intercepted " + ctx + " regions with " + cta + " zones");
        }
    }
}
