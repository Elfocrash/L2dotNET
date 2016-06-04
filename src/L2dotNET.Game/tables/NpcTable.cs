using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.templates;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.tables
{
    class NpcTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NpcTable));
        private static volatile NpcTable instance;
        private static object syncRoot = new object();

        public static NpcTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NpcTable();
                        }
                    }
                }

                return instance;
            }
        }

        public readonly SortedList<int, NpcTemplate> _npcs = new SortedList<int, NpcTemplate>();
        public readonly SortedList<string, NpcVid> npcVids = new SortedList<string, NpcVid>();

        public void Initialize()
        {
            FileInfo file = new FileInfo(@"scripts\npc_vid.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    NpcVid vid = new NpcVid();
                    vid.cl = pt[0];

                    for (byte ord = 1; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "coll":
                                vid.radius = Convert.ToDouble(value.Split(' ')[0]);
                                vid.height = Convert.ToDouble(value.Split(' ')[1]);
                                break;
                            case "spd":
                                vid.minspd = Convert.ToInt32(value.Split(' ')[0]);
                                vid.maxspd = Convert.ToInt32(value.Split(' ')[1]);
                                break;
                        }
                    }

                    npcVids.Add(vid.cl, vid);
                }
            }

            file = new FileInfo(@"scripts\npcdata.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    NpcTemplate template = new NpcTemplate();
                    template.NpcId = Convert.ToInt32(pt[0]);
                    try
                    {
                        template.Category = (TObjectCategory)Enum.Parse(typeof(TObjectCategory), pt[1]);
                    }
                    catch (Exception)
                    {
                        log.Error($"Npc category was not found { pt[1] }");
                        continue;
                    }

                    try
                    {
                        if (npcVids.ContainsKey(pt[2].Split('.')[1]))
                        {
                            NpcVid vid = npcVids[pt[2].Split('.')[1]];
                            template.CollisionRadius = vid.radius;
                            template.CollisionHeight = vid.height;
                        }
                    }
                    catch (Exception)
                    {
                        log.Error($"err2 in #{ template.NpcId } '{ pt[2] }'");
                    }

                    for (byte ord = 3; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = "";
                        if (parameter.Length == 0)
                            continue;

                        try
                        {
                            value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);
                        }
                        catch (Exception)
                        {
                            log.Error($"err in #{ template.NpcId } '{ parameter }'");
                        }

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "level":
                                template.Level = Convert.ToByte(value);
                                break;
                            case "exp":
                                template.exp = Convert.ToInt64(value);
                                break;
                            case "ex_crt_effect":
                                template.ex_crt_effect = Convert.ToInt32(value);
                                break;
                            case "unique":
                                template.unique = Convert.ToInt32(value);
                                break;
                            case "s_npc_prop_hp_rate":
                                template.s_npc_prop_hp_rate = Convert.ToDouble(value);
                                break;
                            case "race":
                                template.Race = (TObjectRace)Enum.Parse(typeof(TObjectRace), value);
                                break;
                            case "sex":
                                template.Sex = (TObjectSex)Enum.Parse(typeof(TObjectSex), value);
                                break;
                            case "skill_list":
                                template.setNpcSkills(value);
                                break;
                            case "slot_chest":
                                template.slot_chest = value;
                                break;
                            case "slot_rhand":
                                template.slot_rhand = value;
                                break;
                            case "slot_lhand":
                                template.slot_lhand = value;
                                break;
                            case "hit_time_factor":
                                template.hit_time_factor = Convert.ToDouble(value);
                                break;
                            case "hit_time_factor_skill":
                                template.hit_time_factor_skill = Convert.ToDouble(value.Split(' ')[0]);
                                break;
                            case "str":
                                template.Str = Convert.ToInt32(value);
                                break;
                            case "int":
                                template.Int = Convert.ToInt32(value);
                                break;
                            case "dex":
                                template.Dex = Convert.ToInt32(value);
                                break;
                            case "wit":
                                template.Wit = Convert.ToInt32(value);
                                break;
                            case "con":
                                template.Con = Convert.ToInt32(value);
                                break;
                            case "men":
                                template.Men = Convert.ToInt32(value);
                                break;
                            case "org_hp":
                                template.org_hp = Convert.ToDouble(value);
                                break;
                            case "org_hp_regen":
                                template.org_hp_regen = Convert.ToDouble(value);
                                break;
                            case "org_mp":
                                template.org_mp = Convert.ToDouble(value);
                                break;
                            case "org_mp_regen":
                                template.org_mp_regen = Convert.ToDouble(value);
                                break;
                            case "base_attack_type":
                                template.base_attack_type = (TObjectBaseAttackType)Enum.Parse(typeof(TObjectBaseAttackType), value);
                                break;
                            case "base_attack_range":
                                template.base_attack_range = Convert.ToInt32(value);
                                break;
                            case "base_damage_range":
                                //  template.Men = Convert.ToInt32(value);
                                break;
                            case "base_rand_dam":
                                template.base_rand_dam = Convert.ToInt32(value);
                                break;
                            case "base_physical_attack":
                                template.base_physical_attack = Convert.ToDouble(value);
                                break;
                            case "base_critical":
                                template.base_critical = Convert.ToInt32(value);
                                break;
                            case "physical_hit_modify":
                                template.physical_hit_modify = Convert.ToDouble(value);
                                break;
                            case "base_attack_speed":
                                template.base_attack_speed = Convert.ToInt32(value);
                                break;
                            case "base_reuse_delay":
                                template.base_reuse_delay = Convert.ToInt32(value);
                                break;
                            case "base_magic_attack":
                                template.base_magic_attack = Convert.ToDouble(value);
                                break;
                            case "base_defend":
                                template.base_defend = Convert.ToDouble(value);
                                break;
                            case "base_magic_defend":
                                template.base_magic_defend = Convert.ToDouble(value);
                                break;
                            case "base_attribute_attack":
                                // template.physical_hit_modify = Convert.ToDouble(value);
                                break;
                            case "base_attribute_defend":
                                //  template.physical_hit_modify = Convert.ToDouble(value);
                                break;
                            case "physical_avoid_modify":
                                template.physical_avoid_modify = Convert.ToDouble(value);
                                break;
                            case "shield_defense_rate":
                                template.shield_defense_rate = Convert.ToInt32(value);
                                break;
                            case "shield_defense":
                                template.shield_defense = Convert.ToDouble(value);
                                break;
                            case "safe_height":
                                template.safe_height = Convert.ToInt32(value);
                                break;
                            case "soulshot_count":
                                template.soulshot_count = Convert.ToInt32(value);
                                break;
                            case "spiritshot_count":
                                template.spiritshot_count = Convert.ToInt32(value);
                                break;
                            case "clan":
                                template.clan = value;
                                break;
                            case "clan_help_range":
                                template.clan_help_range = Convert.ToInt32(value);
                                break;
                            case "undying":
                                template.undying = Convert.ToInt32(value);
                                break;
                            case "can_be_attacked":
                                template.can_be_attacked = Convert.ToInt32(value);
                                break;
                            case "corpse_time":
                                template.corpse_time = Convert.ToInt32(value);
                                break;
                            case "no_sleep_mode":
                                template.no_sleep_mode = Convert.ToInt32(value);
                                break;
                            case "agro_range":
                                template.agro_range = Convert.ToInt32(value);
                                break;
                            case "passable_door":
                                template.passable_door = Convert.ToInt32(value);
                                break;
                            case "can_move":
                                template.can_move = Convert.ToInt32(value);
                                break;
                            case "flying":
                                template.flying = Convert.ToInt32(value);
                                break;
                            case "has_summoner":
                                template.has_summoner = Convert.ToInt32(value);
                                break;
                            case "targetable":
                                template.targetable = Convert.ToInt32(value);
                                break;
                            case "show_name_tag":
                                template.show_name_tag = Convert.ToInt32(value);
                                break;
                            case "event_flag":
                                template.event_flag = Convert.ToInt32(value);
                                break;
                            case "unsowing":
                                template.unsowing = Convert.ToInt32(value);
                                break;
                            case "private_respawn_log":
                                template.private_respawn_log = Convert.ToInt32(value);
                                break;
                            case "acquire_exp_rate":
                                template.acquire_exp_rate = Convert.ToDouble(value);
                                break;
                            case "acquire_sp":
                                template.acquire_sp = (int)Convert.ToDouble(value);
                                break;
                            case "acquire_rp":
                                template.acquire_rp = (int)Convert.ToDouble(value);
                                break;
                            case "fake_class_id":
                                template.fake_class_id = Convert.ToInt32(value);
                                break;
                        }
                    }

                    _npcs.Add(template.NpcId, template);
                }
            }

            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\dropdata.xml"));
            XElement ex = xml.Element("list");
            int dropitems = 0;
            foreach (var m in ex.Elements())
            {
                if (m.Name == "npc")
                {
                    NpcTemplate template = _npcs[int.Parse(m.Attribute("id").Value)];

                    foreach (var stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "drop":
                                {
                                    if (template.DropData == null)
                                        template.DropData = new DropContainer();

                                    foreach (var boxes in stp.Elements())
                                    {
                                        if (boxes.Name == "box")
                                        {
                                            DropBox box = new DropBox();
                                            box.rate = double.Parse(boxes.Attribute("rate").Value);
                                            foreach (var items in boxes.Elements())
                                            {
                                                if (items.Name == "item")
                                                {
                                                    DropItem item = new DropItem();
                                                    item.id = int.Parse(items.Attribute("id").Value);
                                                    item.min = long.Parse(items.Attribute("min").Value);
                                                    item.max = long.Parse(items.Attribute("max").Value);
                                                    item.rate = double.Parse(items.Attribute("rate").Value);
                                                    item.template = ItemTable.Instance.GetItem(item.id);
                                                    box.items.Add(item);
                                                    dropitems++;
                                                }
                                            }

                                            template.DropData.multidrop.Add(box);
                                        }
                                    }
                                }
                                break;
                            case "drop_ex":
                                {
                                    if (template.DropData == null)
                                        template.DropData = new DropContainer();

                                    foreach (var boxes in stp.Elements())
                                    {
                                        if (boxes.Name == "box")
                                        {
                                            DropBox box = new DropBox();
                                            box.rate = double.Parse(boxes.Attribute("rate").Value);
                                            foreach (var items in boxes.Elements())
                                            {
                                                if (items.Name == "item")
                                                {
                                                    DropItem item = new DropItem();
                                                    item.id = int.Parse(items.Attribute("id").Value);
                                                    item.min = long.Parse(items.Attribute("min").Value);
                                                    item.max = long.Parse(items.Attribute("max").Value);
                                                    item.rate = double.Parse(items.Attribute("rate").Value);
                                                    item.template = ItemTable.Instance.GetItem(item.id);
                                                    box.items.Add(item);
                                                    dropitems++;
                                                }
                                            }

                                            template.DropData.multidrop_ex.Add(box);
                                        }
                                    }
                                }
                                break;
                            case "spoil":
                                {
                                    if (template.DropData == null)
                                        template.DropData = new DropContainer();

                                    foreach (var boxes in stp.Elements())
                                    {
                                        if (boxes.Name == "item")
                                        {
                                            DropItem item = new DropItem();
                                            item.id = int.Parse(boxes.Attribute("id").Value);
                                            item.min = long.Parse(boxes.Attribute("min").Value);
                                            item.max = long.Parse(boxes.Attribute("max").Value);
                                            item.rate = double.Parse(boxes.Attribute("rate").Value);
                                            item.template = ItemTable.Instance.GetItem(item.id);
                                            template.DropData.spoil.Add(item);
                                            dropitems++;
                                        }
                                    }
                                }
                                break;
                            case "qdrop":
                                {
                                    if (template.DropData == null)
                                        template.DropData = new DropContainer();

                                    foreach (var boxes in stp.Elements())
                                    {
                                        if (boxes.Name == "item")
                                        {
                                            DropItem item = new DropItem();
                                            item.id = int.Parse(boxes.Attribute("id").Value);
                                            item.min = long.Parse(boxes.Attribute("min").Value);
                                            item.max = long.Parse(boxes.Attribute("max").Value);
                                            item.rate = double.Parse(boxes.Attribute("rate").Value);
                                            item.template = ItemTable.Instance.GetItem(item.id);
                                            template.DropData.qdrop.Add(item);
                                            dropitems++;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            log.Info("NpcTable: loaded #" + _npcs.Count + " NPC, #" + npcVids.Count + " VIDs, " + dropitems + " drop items.");
        }

        public NpcTable()
        {

        }

        public NpcTemplate GetNpcTemplate(int id)
        {
            if (_npcs.ContainsKey(id))
                return _npcs[id];

            return null;
        }

        public L2Object SpawnNpc(int id, int x, int y, int z, int h)
        {
            NpcTemplate template = this.GetNpcTemplate(id);

            if (template == null)
            {
                log.Info($"null template { id }");
                return null;
            }
            L2Warrior o = new L2Warrior();
            o.setTemplate(template);
            //switch (template._type)
            //{
            //    case NpcTemplate.L2NpcType.warrior:
            //    case NpcTemplate.L2NpcType.zzoldagu:
            //    case NpcTemplate.L2NpcType.herb_warrior:
            //    case NpcTemplate.L2NpcType.boss:
            //        o = new L2Warrior();
            //        ((L2Warrior)o).setTemplate(template);
            //        break;

            //    default:
            //        o = new L2Citizen();
            //        ((L2Citizen)o).setTemplate(template);
            //        break;
            //}
            o.X = x;
            o.Y = y;
            o.Z = z;
            o.Heading = h;

            o.SpawnX = x;
            o.SpawnY = y;
            o.SpawnZ = z;

            L2World.Instance.RealiseEntry(o, null, true);
            o.onSpawn();

            return o;
        }
    }

    class NpcVid
    {
        public string cl;
        public double radius;
        public double height;
        public int minspd;
        public int maxspd;
    }

    public class DropContainer
    {
        public List<DropBox> multidrop = new List<DropBox>();
        public List<DropBox> multidrop_ex = new List<DropBox>();
        public List<DropItem> spoil = new List<DropItem>();
        public List<DropItem> qdrop = new List<DropItem>();

        public void showInfo(L2Player player)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("regular drops:<br>");
            if (multidrop.Count == 0)
                sb.Append("nothing");
            else
            {
                foreach (DropBox box in multidrop)
                {
                    sb.Append("Box " + box.rate + "%<br1>");
                    foreach (DropItem dbi in box.items)
                        sb.Append(dbi.id + " (" + dbi.min + "-" + dbi.max + ") " + dbi.rate + "%<br1>");
                    sb.Append("<br>");
                }
                sb.Append("<br>");
            }

            sb.Append("spoil:<br>");
            if (spoil.Count == 0)
                sb.Append("nothing");
            else
            {
                foreach (DropItem dbi in spoil)
                    sb.Append(dbi.id + " (" + dbi.min + "-" + dbi.max + ") " + dbi.rate + "%<br1>");
                sb.Append("<br>");
            }

            sb.Append("herb drop:<br>");
            if (multidrop_ex.Count == 0)
                sb.Append("nothing");
            else
            {
                foreach (DropBox box in multidrop_ex)
                {
                    sb.Append("Box " + box.rate + "%<br1>");
                    foreach (DropItem dbi in box.items)
                        sb.Append(dbi.id + " (" + dbi.min + "-" + dbi.max + ") " + dbi.rate + "%<br1>");
                    sb.Append("<br>");
                }
                sb.Append("<br>");
            }

            sb.Append("quest related:<br>");
            if (multidrop_ex.Count == 0)
                sb.Append("nothing");
            else
            {
                foreach (DropItem dbi in qdrop)
                    sb.Append(dbi.id + " (" + dbi.min + "-" + dbi.max + ") " + dbi.rate + "%<br1>");
                sb.Append("<br>");
            }

            player.ShowHtmPlain(sb.ToString(), null);
        }

        public void roll_multidrop(L2Citizen npc, L2Character killer)
        {
            Random rn = new Random();
            foreach (DropBox box in multidrop)
            {
                if (rn.Next(1000000) <= (int)(box.rate * 10000))
                {
                    int dbcc = rn.Next(1000000);
                    int dbxc = 0;
                    DropItem rolled = null;
                    foreach (DropItem itm in box.items)
                    {
                        dbxc += (int)(box.rate * 10000);
                        if (dbcc <= dbxc)
                        {
                            rolled = itm;
                            break;
                        }
                    }

                    if (rolled == null)
                        continue;

                    if (Config.Instance.gameplayConfig.AutoLoot)
                    {
                        int count = rn.Next((int)rolled.min, (int)rolled.max);
                        ((L2Player)killer).Inventory.addItem(rolled.id, count, 0, true, true);
                    }
                    else
                    {
                        if (rolled.template.isStackable())
                        {
                            L2Item ditem = new L2Item(rolled.template);
                            ditem.Count = rn.Next((int)rolled.min, (int)rolled.max);
                            ditem.dropMe(npc.X + rn.Next(-66, 66), npc.Y + rn.Next(-66, 66), npc.Z, npc, killer, 10);
                        }
                        else
                        {
                            int count = rn.Next((int)rolled.min, (int)rolled.max);
                            for (int i = 0; i < count; i++)
                            {
                                L2Item ditem = new L2Item(rolled.template);
                                ditem.dropMe(npc.X + rn.Next(-66, 66), npc.Y + rn.Next(-66, 66), npc.Z, npc, killer, 10);
                            }
                        }
                    }
                }
            }
        }

        public void roll_spoil(L2Player caster, L2Citizen npc)
        {
            Random rn = new Random();

            List<DropItem> rolled = new List<DropItem>();

            foreach (DropItem itm in spoil)
            {
                if (rn.Next(1000000) <= (int)(itm.rate * 10000))
                    rolled.Add(itm);
            }

            if (rolled.Count == 0)
                return;

            foreach (DropItem item in rolled)
            {
                int count = rn.Next((int)item.min, (int)item.max);
                caster.Inventory.addItem(item.template, count, 0, true, true);
            }
        }

        public void roll_qdrop(L2Citizen npc, L2Character killer)
        {
            Random rn = new Random();

            List<DropItem> rolled = new List<DropItem>();

            foreach (DropItem itm in qdrop)
            {
                if (rn.Next(1000000) <= (int)(itm.rate * 10000))
                    rolled.Add(itm);
            }

            if (rolled.Count == 0)
                return;

            foreach (DropItem item in rolled)
            {
                int count = rn.Next((int)item.min, (int)item.max);
                ((L2Player)killer).Inventory.addItem(item.template, count, 0, true, true);
            }
        }

        public void roll_multidrop_ex(L2Citizen npc, L2Character killer)
        {
            Random rn = new Random();
            foreach (DropBox box in multidrop_ex)
            {
                if (rn.Next(1000000) <= (int)(box.rate * 10000))
                {
                    int dbcc = rn.Next(1000000);
                    int dbxc = 0;
                    DropItem rolled = null;
                    foreach (DropItem itm in box.items)
                    {
                        dbxc += (int)(box.rate * 10000);
                        if (dbcc <= dbxc)
                        {
                            rolled = itm;
                            break;
                        }
                    }

                    if (rolled == null)
                        continue;

                    if (Config.Instance.gameplayConfig.AutoLoot)
                    {
                        int count = rn.Next((int)rolled.min, (int)rolled.max);
                        ((L2Player)killer).Inventory.addItem(rolled.id, count, 0, true, true);
                    }
                    else
                    {
                        if (rolled.template.isStackable())
                        {
                            L2Item ditem = new L2Item(rolled.template);
                            ditem.Count = rn.Next((int)rolled.min, (int)rolled.max);
                            ditem.dropMe(npc.X + rn.Next(-66, 66), npc.Y + rn.Next(-66, 66), npc.Z, npc, killer, 10);
                        }
                        else
                        {
                            int count = rn.Next((int)rolled.min, (int)rolled.max);
                            for (int i = 0; i < count; i++)
                            {
                                L2Item ditem = new L2Item(rolled.template);
                                ditem.dropMe(npc.X + rn.Next(-66, 66), npc.Y + rn.Next(-66, 66), npc.Z, npc, killer, 10);
                            }
                        }
                    }
                }
            }
        }
    }

    public class DropBox
    {
        public double rate;
        public List<DropItem> items = new List<DropItem>();
    }

    public class DropItem
    {
        public double rate;
        public long min, max;
        public int id;
        public ItemTemplate template;
    }
}
