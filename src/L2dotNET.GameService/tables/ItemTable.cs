using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Tables
{
    class ItemTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTable));
        private static volatile ItemTable _instance;
        private static readonly object SyncRoot = new object();

        public static ItemTable Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ItemTable();
                    }

                return _instance;
            }
        }

        public void Initialize()
        {
            FileInfo file = new FileInfo(@"scripts\items_set.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    ItemSetTemplate set = new ItemSetTemplate();
                    set.ArmorId = Convert.ToInt32(pt[0]);

                    for (byte ord = 1; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1);
                        value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "legs":
                                foreach (string str in value.Split(' '))
                                    set.AddLeg(Convert.ToInt32(str));

                                break;
                            case "helm":
                                foreach (string str in value.Split(' '))
                                    set.AddHelm(Convert.ToInt32(str));

                                break;
                            case "gloves":
                                foreach (string str in value.Split(' '))
                                    set.AddGloves(Convert.ToInt32(str));

                                break;
                            case "boots":
                                foreach (string str in value.Split(' '))
                                    set.AddBoot(Convert.ToInt32(str));

                                break;
                            case "shield":
                                foreach (string str in value.Split(' '))
                                    set.AddShield(Convert.ToInt32(str));

                                break;

                            case "set1":
                                set.Set1(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                            case "set2":
                                set.Set2(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                            case "set3":
                                set.Set3(Convert.ToInt32(value.Split('-')[0]), Convert.ToInt32(value.Split('-')[1]));
                                break;
                        }
                    }

                    Sets.Add(set.ArmorId, set);
                }
            }

            file = new FileInfo(@"scripts\items_off.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    ItemTemplate item = new ItemTemplate();
                    item.ItemId = Convert.ToInt32(pt[1]);
                    item.Type = (ItemTemplate.L2ItemType)Enum.Parse(typeof(ItemTemplate.L2ItemType), pt[0]);

                    if (Sets.ContainsKey(item.ItemId))
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
                            Log.Error($"eh {pt[ord]}");
                        }

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "body":
                                item.Bodypart = (ItemTemplate.L2ItemBodypart)Enum.Parse(typeof(ItemTemplate.L2ItemBodypart), value);
                                break;
                            case "armor_type":
                                item.ArmorType = value;
                                break;
                            case "etcitem_type":
                                item.EtcitemType = value;
                                break;
                            case "delay_share_group":
                                item.DelayShareGroup = int.Parse(value);
                                break;
                            case "item_multi_skill_list":
                                item.AddMultiSkills(value);
                                break;
                            case "item_skill":
                                item.AddItemSkill(value);
                                break;
                            case "item_skill_enchanted_four":
                                item.AddItemEnch4(value);
                                break;
                            case "recipe_id":
                                item.RecipeId = int.Parse(value);
                                break;
                            case "blessed":
                                item.Blessed = int.Parse(value);
                                break;
                            case "weight":
                                item.Weight = Convert.ToInt32(value);
                                break;
                            case "default_action":
                                item.DefaultAction = value;
                                break;
                            case "consume_type":
                                item.StackType = value != "normal" ? ItemTemplate.L2ItemConsume.Stackable : ItemTemplate.L2ItemConsume.Normal;
                                break;
                            case "soulshot_count":
                                item.SoulshotCount = int.Parse(value);
                                break;
                            case "spiritshot_count":
                                item.SpiritshotCount = int.Parse(value);
                                break;
                            case "reduced_soulshot":
                                item.SetReducingSoulShots(value);
                                break;
                            case "reduced_mp_consume":
                                item.SetReducingMpConsume(value);
                                break;
                            case "immediate_effect":
                                item.ImmediateEffect = int.Parse(value);
                                break;
                            case "ex_immediate_effect":
                                item.ExImmediateEffect = int.Parse(value);
                                break;
                            case "drop_period":
                                item.DropPeriod = int.Parse(value);
                                break;
                            case "ex_drop_period":
                                item.ExDropPeriod = int.Parse(value);
                                break;
                            case "duration":
                                item.Durability = Convert.ToInt32(value);
                                break;
                            case "use_skill_distime":
                                item.UseSkillDistime = int.Parse(value);
                                break;
                            case "equip_reuse_delay":
                                item.EquipReuseDelay = int.Parse(value);
                                break;
                            case "default_price":
                                item.Price = Convert.ToInt32(value);
                                break;
                            case "crystal_type":
                                item.CrystallGrade = (ItemTemplate.L2ItemGrade)Enum.Parse(typeof(ItemTemplate.L2ItemGrade), value);
                                break;
                            case "crystal_count":
                                item.CryCount = Convert.ToInt32(value);
                                break;
                            case "keep_type":
                                item.KeepType = int.Parse(value);
                                break;
                            case "avoid_modify":
                                item.AvoidModify = Convert.ToInt32(value);
                                break;
                            case "pdef":
                                item.PhysicalDefense = Convert.ToInt32(value);
                                break;
                            case "mdef":
                                item.MagicalDefense = Convert.ToInt32(value);
                                break;
                            case "mp_bonus":
                                item.MpBonus = Convert.ToInt32(value);
                                break;
                            case "enchanted":
                                item.Enchanted = Convert.ToInt16(value);
                                break;
                            case "patk":
                                item.PhysicalDamage = Convert.ToInt32(value);
                                break;
                            case "random_damage":
                                item.RandomDamage = Convert.ToInt32(value);
                                break;
                            case "equip_condition":
                                item.SetEquipCondition(value);
                                break;
                            case "item_equip_option":
                                item.SetEquipOption(value);
                                break;
                            case "use_condition":
                                item.SetUseCondition(value);
                                break;
                            case "base_attribute_attack":
                                item.SetAttributeAttack(value);
                                break;
                            case "base_attribute_defend":
                                item.SetAttributeDefend(value);
                                break;
                            case "can_move":
                                item.CanMove = Convert.ToInt32(value);
                                break;
                            case "html":
                                item.HtmFile = value;
                                break;
                            case "magic_weapon":
                                item.MagicWeapon = Convert.ToInt32(value);
                                break;
                            case "unequip_skill":
                                item.SetUnequipSkill(value);
                                break;
                            case "for_npc":
                                item.ForNpc = Convert.ToInt32(value);
                                break;
                            case "weapon_type":
                                item.WeaponType = (ItemTemplate.L2ItemWeaponType)Enum.Parse(typeof(ItemTemplate.L2ItemWeaponType), value);
                                break;
                            case "critical":
                                item.Critical = Convert.ToInt32(value);
                                break;
                            case "hit_modify":
                                item.HitModify = Convert.ToDouble(value);
                                break;
                            case "attack_range":
                                item.AttackRange = Convert.ToInt32(value);
                                break;
                            case "damage_range":
                                item.SetDamageRange(value);
                                break;
                            case "attack_speed":
                                item.AttackSpeed = Convert.ToInt32(value);
                                break;
                            case "matk":
                                item.MagicalDamage = Convert.ToInt32(value);
                                break;
                            case "shield_defense":
                                item.ShieldDefense = Convert.ToInt32(value);
                                break;
                            case "shield_defense_rate":
                                item.ShieldDefenseRate = Convert.ToInt32(value);
                                break;
                            case "mp_consume":
                                item.MpConsume = Convert.ToInt32(value);
                                break;
                            case "period":
                                item.LimitedMinutes = Convert.ToInt32(value);
                                break;
                            case "reuse_delay":
                                item.ReuseDelay = Convert.ToInt32(value);
                                break;
                            case "is_trade":
                                item.IsTrade = int.Parse(value);
                                break;
                            case "is_destruct":
                                item.IsDestruct = int.Parse(value);
                                break;
                            case "is_drop":
                                item.IsDrop = int.Parse(value);
                                break;
                            case "is_premium":
                                item.IsPremium = int.Parse(value);
                                break;
                            case "is_private_store":
                                item.IsPrivateStore = int.Parse(value);
                                break;
                            case "enchant_enable":
                                item.EnchantEnable = int.Parse(value);
                                break;
                            case "elemental_enable":
                                item.ElementalEnable = int.Parse(value);
                                break;
                            case "is_olympiad_can_use":
                                item.IsOlympiadCanUse = int.Parse(value);
                                break;
                        }
                    }

                    item.BuildEffect();
                    if (Items.ContainsKey(item.ItemId))
                    {
                        Log.Error($"itemtable: dublicate {item.ItemId}");
                        Items.Remove(item.ItemId);
                    }

                    Items.Add(item.ItemId, item);
                }
            }

            file = new FileInfo(@"scripts\convertdata.txt");
            using (StreamReader sreader = file.OpenText())
            {
                while (!sreader.EndOfStream)
                {
                    string line = sreader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
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
            Log.Info($"ItemTable: #{Items.Count} items, #{Sets.Count} sets, #{ConvertDataList.Count} convertable.");
        }

        public readonly SortedList<int, ItemTemplate> Items = new SortedList<int, ItemTemplate>();
        public readonly SortedList<int, ItemSetTemplate> Sets = new SortedList<int, ItemSetTemplate>();
        public readonly SortedList<int, int> ConvertDataList = new SortedList<int, int>();

        public ItemTemplate GetItem(int id)
        {
            if (!Items.ContainsKey(id))
            {
                Log.Error($"itemtable: error, cant find item for id {id}");
                return null;
            }

            return Items[id];
        }

        public void NotifyKeySetItem(L2Player owner, L2Item item, bool equip)
        {
            ItemSetTemplate set = Sets[item.Template.ItemId];
            owner.SetKeyItems = set.GetAllSetIds();
            owner.SetKeyId = set.ArmorId;

            NotifySetItemEquip(owner, item, equip);
        }

        public void NotifySetItemEquip(L2Player owner, L2Item item, bool equip)
        {
            ItemSetTemplate set = Sets[owner.SetKeyId];

            if (!equip)
            {
                bool b1 = false,
                     b2 = false,
                     b3 = false;
                foreach (Skill skill in owner.Skills.Values)
                {
                    if ((set.Set1Id > 0) && (skill.SkillId == set.Set1Id))
                        b1 = true;
                    if ((set.Set2Id > 0) && (skill.SkillId == set.Set2Id))
                        b2 = true;
                    if ((set.Set3Id > 0) && (skill.SkillId == set.Set3Id))
                        b3 = true;

                    if (b1 && b2 && b3)
                        break;
                }

                if (b1)
                    owner.RemoveSkill(set.Set1Id, false, false);
                if (b2)
                    owner.RemoveSkill(set.Set2Id, false, false);
                if (b3)
                    owner.RemoveSkill(set.Set3Id, false, false);

                if (b1 || b2 || b3)
                    owner.UpdateSkillList();
            }
            else
            {
                set.Validate(owner);
            }
        }

        public bool CanConvert(int id)
        {
            return ConvertDataList.ContainsKey(id);
        }
    }
}