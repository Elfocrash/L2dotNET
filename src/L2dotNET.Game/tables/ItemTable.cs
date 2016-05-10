﻿using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.skills2;
using log4net;

namespace L2dotNET.Game.tables
{
    class ItemTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemTable));
        private static volatile ItemTable instance;
        private static object syncRoot = new object();

        public static ItemTable Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(syncRoot)
                    {
                        if(instance == null)
                        {
                            instance = new ItemTable();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            FileInfo file = new FileInfo(@"scripts\items_set.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    ItemSetTemplate set = new ItemSetTemplate();
                    set.armorId = Convert.ToInt32(pt[0]);

                    for (byte ord = 1; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "legs":
                                foreach (string str in value.Split(' '))
                                    set.addLeg(Convert.ToInt32(str));
                                break;
                            case "helm":
                                foreach (string str in value.Split(' '))
                                    set.addHelm(Convert.ToInt32(str));
                                break;
                            case "gloves":
                                foreach (string str in value.Split(' '))
                                    set.addGloves(Convert.ToInt32(str));
                                break;
                            case "boots":
                                foreach (string str in value.Split(' '))
                                    set.addBoot(Convert.ToInt32(str));
                                break;
                            case "shield":
                                foreach (string str in value.Split(' '))
                                    set.addShield(Convert.ToInt32(str));
                                break;

                            case "set1":
                                set.set1(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                            case "set2":
                                set.set2(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                            case "set3":
                                set.set3(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                        }
                    }

                    _sets.Add(set.armorId, set);
                }
            }

            file = new FileInfo(@"scripts\items_off.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    ItemTemplate item = new ItemTemplate();
                    item.ItemID = Convert.ToInt32(pt[1]);
                    item.Type = (ItemTemplate.L2ItemType)Enum.Parse(typeof(ItemTemplate.L2ItemType), pt[0]);

                    if (_sets.ContainsKey(item.ItemID))
                        item.SetItem = true;

                    for (byte ord = 2; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        if (parameter.Length == 0)
                            continue;

                        string value = parameter.Substring(parameter.IndexOf('{') + 1);
                        try
                        {
                            value = value.Remove(value.Length - 1);
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("eh " + pt[ord]);
                        }

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "body":
                                item.Bodypart = (ItemTemplate.L2ItemBodypart)Enum.Parse(typeof(ItemTemplate.L2ItemBodypart), value);
                                break;
                            case "armor_type":
                                item.armor_type = value;
                                break;
                            case "etcitem_type":
                                item.etcitem_type = value;
                                break;
                            case "delay_share_group":
                                item.delay_share_group = int.Parse(value);
                                break;
                            case "item_multi_skill_list":
                                item.addMultiSkills(value);
                                break;
                            case "item_skill":
                                item.addItemSkill(value);
                                break;
                            case "item_skill_enchanted_four":
                                item.addItemEnch4(value);
                                break;
                            case "recipe_id":
                                item._recipeId = int.Parse(value);
                                break;
                            case "blessed":
                                item.blessed = int.Parse(value);
                                break;
                            case "weight":
                                item.Weight = Convert.ToInt32(value);
                                break;
                            case "default_action":
                                item.default_action = value;
                                break;
                            case "consume_type":
                                item.StackType = value != "normal" ? ItemTemplate.L2ItemConsume.stackable : ItemTemplate.L2ItemConsume.normal;
                                break;
                            case "soulshot_count":
                                item.SoulshotCount = int.Parse(value);
                                break;
                            case "spiritshot_count":
                                item.SpiritshotCount = int.Parse(value);
                                break;
                            case "reduced_soulshot":
                                item.setReducingSoulShots(value);
                                break;
                            case "reduced_mp_consume":
                                item.setReducingMpConsume(value);
                                break;
                            case "immediate_effect":
                                item.immediate_effect = int.Parse(value);
                                break;
                            case "ex_immediate_effect":
                                item.ex_immediate_effect = int.Parse(value);
                                break;
                            case "drop_period":
                                item.drop_period = int.Parse(value);
                                break;
                            case "ex_drop_period":
                                item.ex_drop_period = int.Parse(value);
                                break;
                            case "duration":
                                item.Durability = Convert.ToInt32(value);
                                break;
                            case "use_skill_distime":
                                item.use_skill_distime = int.Parse(value);
                                break;
                            case "equip_reuse_delay":
                                item.equip_reuse_delay = int.Parse(value);
                                break;
                            case "default_price":
                                item.Price = Convert.ToInt32(value);
                                break;
                            case "crystal_type":
                                item.CrystallGrade = (ItemTemplate.L2ItemGrade)Enum.Parse(typeof(ItemTemplate.L2ItemGrade), value);
                                break;
                            case "crystal_count":
                                item._cryCount = Convert.ToInt64(value);
                                break;
                            case "keep_type":
                                item.keep_type = int.Parse(value);
                                break;
                            case "avoid_modify":
                                item.avoid_modify = Convert.ToInt32(value);
                                break;
                            case "pdef":
                                item.physical_defense = Convert.ToInt32(value);
                                break;
                            case "mdef":
                                item.magical_defense = Convert.ToInt32(value);
                                break;
                            case "mp_bonus":
                                item.mp_bonus = Convert.ToInt32(value);
                                break;
                            case "enchanted":
                                item.enchanted = Convert.ToInt16(value);
                                break;
                            case "patk":
                                item.physical_damage = Convert.ToInt32(value);
                                break;
                            case "random_damage":
                                item.random_damage = Convert.ToInt32(value);
                                break;
                            case "equip_condition":
                                item.setEquipCondition(value);
                                break;
                            case "item_equip_option":
                                item.setEquipOption(value);
                                break;
                            case "use_condition":
                                item.setUseCondition(value);
                                break;
                            case "base_attribute_attack":
                                item.setAttributeAttack(value);
                                break;
                            case "base_attribute_defend":
                                item.setAttributeDefend(value);
                                break;
                            case "can_move":
                                item.can_move = Convert.ToInt32(value);
                                break;
                            case "html":
                                item._htmFile = value;
                                break;
                            case "magic_weapon":
                                item.magic_weapon = Convert.ToInt32(value);
                                break;
                            case "unequip_skill":
                                item.setUnequipSkill(value);
                                break;
                            case "for_npc":
                                item.for_npc = Convert.ToInt32(value);
                                break;
                            case "weapon_type":
                                item.WeaponType = (ItemTemplate.L2ItemWeaponType)Enum.Parse(typeof(ItemTemplate.L2ItemWeaponType), value);
                                break;
                            case "critical":
                                item.critical = Convert.ToInt32(value);
                                break;
                            case "hit_modify":
                                item.hit_modify = Convert.ToDouble(value);
                                break;
                            case "attack_range":
                                item.attack_range = Convert.ToInt32(value);
                                break;
                            case "damage_range":
                                item.setDamageRange(value);
                                break;
                            case "attack_speed":
                                item.attack_speed = Convert.ToInt32(value);
                                break;
                            case "matk":
                                item.magical_damage = Convert.ToInt32(value);
                                break;
                            case "shield_defense":
                                item.shield_defense = Convert.ToInt32(value);
                                break;
                            case "shield_defense_rate":
                                item.shield_defense_rate = Convert.ToInt32(value);
                                break;
                            case "mp_consume":
                                item.MpConsume = Convert.ToInt32(value);
                                break;
                            case "period":
                                item.LimitedMinutes = Convert.ToInt32(value);
                                break;
                            case "reuse_delay":
                                item.reuse_delay = Convert.ToInt32(value);
                                break;
                            case "is_trade":
                                item.is_trade = int.Parse(value);
                                break;
                            case "is_destruct":
                                item.is_destruct = int.Parse(value);
                                break;
                            case "is_drop":
                                item.is_drop = int.Parse(value);
                                break;
                            case "is_premium":
                                item.is_premium = int.Parse(value);
                                break;
                            case "is_private_store":
                                item.is_private_store = int.Parse(value);
                                break;
                            case "enchant_enable":
                                item.enchant_enable = int.Parse(value);
                                break;
                            case "elemental_enable":
                                item.elemental_enable = int.Parse(value);
                                break;
                            case "is_olympiad_can_use":
                                item.is_olympiad_can_use = int.Parse(value);
                                break;
                        }
                    }

                    item.buildEffect();
                    if (_items.ContainsKey(item.ItemID))
                    {
                        log.Error($"itemtable: dublicate { item.ItemID }");
                        _items.Remove(item.ItemID);
                    }

                    _items.Add(item.ItemID, item);
                }
            }

            file = new FileInfo(@"scripts\convertdata.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('>');

                    int id1 = int.Parse(pt[0]);
                    int id2 = int.Parse(pt[1]);

                    ConvertDataList.Add(id1, id2);
                    ConvertDataList.Add(id2, id1);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            log.Info($"ItemTable: #{ _items.Count } items, #{ _sets.Count } sets, #{ ConvertDataList.Count } convertable.");
        }

        public readonly SortedList<int, ItemTemplate> _items = new SortedList<int, ItemTemplate>();
        public readonly SortedList<int, ItemSetTemplate> _sets = new SortedList<int, ItemSetTemplate>();
        public readonly SortedList<int, int> ConvertDataList = new SortedList<int, int>();

        public ItemTemplate getItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                log.Error($"itemtable: error, cant find item for id { id }");
                return null;
            }

            return _items[id];
        }

        public void NotifyKeySetItem(L2Player owner, L2Item item, bool equip)
        {
            ItemSetTemplate set = _sets[item.Template.ItemID];
            owner.setKeyItems = set.getAllSetIds();
            owner.setKeyId = set.armorId;

            NotifySetItemEquip(owner, item, equip);
        }

        public void NotifySetItemEquip(L2Player owner, L2Item item, bool equip)
        {
            ItemSetTemplate set = _sets[owner.setKeyId];

            if (!equip)
            {
                bool b1 = false, b2 = false, b3 = false;
                foreach (TSkill skill in owner._skills.Values)
                {
                    if (set.set1Id > 0 && skill.skill_id == set.set1Id)
                        b1 = true;
                    if (set.set2Id > 0 && skill.skill_id == set.set2Id)
                        b2 = true;
                    if (set.set3Id > 0 && skill.skill_id == set.set3Id)
                        b3 = true;

                    if (b1 && b2 && b3)
                        break;
                }

                if (b1) owner.removeSkill(set.set1Id, false, false);
                if (b2) owner.removeSkill(set.set2Id, false, false);
                if (b3) owner.removeSkill(set.set3Id, false, false);

                if (b1 || b2 || b3)
                    owner.updateSkillList();
            }
            else
            {
                set.Validate(owner);
            }
        }


        public bool canConvert(int id)
        {
            return ConvertDataList.ContainsKey(id);
        }
    }

    public class ItemSetTemplate
    {
        public int armorId;

        public List<int> legs;
        public void addLeg(int p)
        {
            if (legs == null)
                legs = new List<int>();

            legs.Add(p);
        }

        public List<int> gloves;
        public void addGloves(int p)
        {
            if (gloves == null)
                gloves = new List<int>();

            gloves.Add(p);
        }

        public List<int> boots;
        public void addBoot(int p)
        {
            if (boots == null)
                boots = new List<int>();

            boots.Add(p);
        }

        public List<int> helms;
        public void addHelm(int p)
        {
            if (helms == null)
                helms = new List<int>();

            helms.Add(p);
        }

        public List<int> shields;
        public void addShield(int p)
        {
            if (shields == null)
                shields = new List<int>();

            shields.Add(p);
        }

        public int set1Id, set1Lvl;
        public void set1(int p, int p2)
        {
            set1Id = p; set1Lvl = p2;
        }

        public int set2Id, set2Lvl;
        public void set2(int p, int p2)
        {
            set2Id = p; set2Lvl = p2;
        }

        public int set3Id, set3Lvl;
        public void set3(int p, int p2)
        {
            set3Id = p; set3Lvl = p2;
        }

        public void Validate(L2Player owner)
        {
            byte set1sum = 0, set2sum = 0, set3sum = 0;
            foreach (L2Item item in owner.Inventory.Items.Values)
            {
                if (item._isEquipped == 0 || item.Template.Type != ItemTemplate.L2ItemType.armor)
                    continue;

                switch (item.Template.Bodypart)
                {
                    case ItemTemplate.L2ItemBodypart.chest:
                    case ItemTemplate.L2ItemBodypart.onepiece:
                        {
                            if (armorId == item.Template.ItemID)
                            {
                                set1sum++;
                                if (item.Enchant >= 6)
                                    set3sum++;
                                break;
                            }
                        }
                        break;
                    case ItemTemplate.L2ItemBodypart.legs:
                        {
                            if (legs != null)
                                foreach (int id in legs)
                                    if (id == item.Template.ItemID)
                                    {
                                        set1sum++;
                                        if (item.Enchant >= 6)
                                            set3sum++;
                                        break;
                                    }
                        }
                        break;
                    case ItemTemplate.L2ItemBodypart.head:
                        {
                            if (helms != null)
                                foreach (int id in helms)
                                    if (id == item.Template.ItemID)
                                    {
                                        set1sum++;
                                        if (item.Enchant >= 6)
                                            set3sum++;
                                        break;
                                    }
                        }
                        break;
                    case ItemTemplate.L2ItemBodypart.gloves:
                        {
                            if (gloves != null)
                                foreach (int id in gloves)
                                    if (id == item.Template.ItemID)
                                    {
                                        set1sum++;
                                        if (item.Enchant >= 6)
                                            set3sum++;
                                        break;
                                    }
                        }
                        break;
                    case ItemTemplate.L2ItemBodypart.feet:
                        {
                            if (boots != null)
                                foreach (int id in boots)
                                    if (id == item.Template.ItemID)
                                    {
                                        set1sum++;
                                        if (item.Enchant >= 6)
                                            set3sum++;
                                        break;
                                    }
                        }
                        break;
                    case ItemTemplate.L2ItemBodypart.lhand:
                        {
                            if (shields != null)
                                foreach (int id in shields)
                                    if (id == item.Template.ItemID)
                                    {
                                        set2sum = 1;
                                        //   if (item.Enchant >= 6)
                                        //      set3sum++;
                                        break;
                                    }
                        }
                        break;
                }
            }

            byte cnt = count();
            Console.WriteLine("set validation: cnt " + cnt + ", s1 " + set1sum + ", s2 " + set2sum + ", s3 " + set3sum);

            if (cnt == set1sum) // весь сет
            {
                owner.addSkill(TSkillTable.Instance.get(set1Id, set1Lvl), false, false);

                if (set2sum == 1) //со щитом
                    owner.addSkill(TSkillTable.Instance.get(set2Id, set2Lvl), false, false);

                if (set3sum == cnt) //весь сет +6
                    owner.addSkill(TSkillTable.Instance.get(set3Id, set3Lvl), false, false);

                owner.updateSkillList();
            }
        }

        private byte count()
        {
            byte s = 1;
            if (legs != null) s++;
            if (helms != null) s++;
            if (gloves != null) s++;
            if (boots != null) s++;
            return s;
        }

        public List<int> getAllSetIds()
        {
            List<int> list = new List<int>();
            list.Add(armorId);

            if (legs != null) list.AddRange(legs);
            if (helms != null) list.AddRange(helms);
            if (gloves != null) list.AddRange(gloves);
            if (boots != null) list.AddRange(boots);
            if (shields != null) list.AddRange(shields);

            return list;
        }

    }
}
