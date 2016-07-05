using System.Collections.Generic;
using System.Linq;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Tables
{
    public class ItemSetTemplate
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemSetTemplate));

        public int ArmorId;

        public List<int> Legs;

        public void AddLeg(int p)
        {
            if (Legs == null)
            {
                Legs = new List<int>();
            }

            Legs.Add(p);
        }

        public List<int> Gloves;

        public void AddGloves(int p)
        {
            if (Gloves == null)
            {
                Gloves = new List<int>();
            }

            Gloves.Add(p);
        }

        public List<int> Boots;

        public void AddBoot(int p)
        {
            if (Boots == null)
            {
                Boots = new List<int>();
            }

            Boots.Add(p);
        }

        public List<int> Helms;

        public void AddHelm(int p)
        {
            if (Helms == null)
            {
                Helms = new List<int>();
            }

            Helms.Add(p);
        }

        public List<int> Shields;

        public void AddShield(int p)
        {
            if (Shields == null)
            {
                Shields = new List<int>();
            }

            Shields.Add(p);
        }

        public int Set1Id,
                   Set1Lvl;

        public void Set1(int p, int p2)
        {
            Set1Id = p;
            Set1Lvl = p2;
        }

        public int Set2Id,
                   Set2Lvl;

        public void Set2(int p, int p2)
        {
            Set2Id = p;
            Set2Lvl = p2;
        }

        public int Set3Id,
                   Set3Lvl;

        public void Set3(int p, int p2)
        {
            Set3Id = p;
            Set3Lvl = p2;
        }

        public void Validate(L2Player owner)
        {
            byte set1Sum = 0,
                 set2Sum = 0,
                 set3Sum = 0;
            foreach (L2Item item in owner.Inventory.Items.Where(item => (item.IsEquipped != 0) && (item.Template.Type == ItemTemplate.L2ItemType.Armor)))
                switch (item.Template.Bodypart)
                {
                    case ItemTemplate.L2ItemBodypart.Chest:
                    case ItemTemplate.L2ItemBodypart.Onepiece:
                    {
                        if (ArmorId == item.Template.ItemId)
                        {
                            set1Sum++;
                            if (item.Enchant >= 6)
                            {
                                set3Sum++;
                            }
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.Legs:
                    {
                        if (Legs != null)
                        {
                            if (Legs.Any(id => id == item.Template.ItemId))
                            {
                                set1Sum++;
                                if (item.Enchant >= 6)
                                {
                                    set3Sum++;
                                }
                            }
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.Head:
                    {
                        if (Helms != null)
                        {
                            if (Helms.Any(id => id == item.Template.ItemId))
                            {
                                set1Sum++;
                                if (item.Enchant >= 6)
                                {
                                    set3Sum++;
                                }
                            }
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.Gloves:
                    {
                        if (Gloves != null)
                        {
                            if (Gloves.Any(id => id == item.Template.ItemId))
                            {
                                set1Sum++;
                                if (item.Enchant >= 6)
                                {
                                    set3Sum++;
                                }
                            }
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.Feet:
                    {
                        if (Boots != null)
                        {
                            if (Boots.Any(id => id == item.Template.ItemId))
                            {
                                set1Sum++;
                                if (item.Enchant >= 6)
                                {
                                    set3Sum++;
                                }
                            }
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.Lhand:
                    {
                        if (Shields != null)
                        {
                            if (Shields.Any(id => id == item.Template.ItemId))
                            {
                                set2Sum = 1;
                            }
                        }
                    }
                        break;
                }

            byte cnt = Count();
            Log.Info($"set validation: cnt {cnt}, s1 {set1Sum}, s2 {set2Sum}, s3 {set3Sum}");

            if (cnt == set1Sum) // весь сет
            {
                owner.AddSkill(SkillTable.Instance.Get(Set1Id, Set1Lvl), false, false);

                if (set2Sum == 1) //со щитом
                {
                    owner.AddSkill(SkillTable.Instance.Get(Set2Id, Set2Lvl), false, false);
                }

                if (set3Sum == cnt) //весь сет +6
                {
                    owner.AddSkill(SkillTable.Instance.Get(Set3Id, Set3Lvl), false, false);
                }

                owner.UpdateSkillList();
            }
        }

        private byte Count()
        {
            byte s = 1;
            if (Legs != null)
            {
                s++;
            }
            if (Helms != null)
            {
                s++;
            }
            if (Gloves != null)
            {
                s++;
            }
            if (Boots != null)
            {
                s++;
            }
            return s;
        }

        public List<int> GetAllSetIds()
        {
            List<int> list = new List<int>();
            list.Add(ArmorId);

            if (Legs != null)
            {
                list.AddRange(Legs);
            }
            if (Helms != null)
            {
                list.AddRange(Helms);
            }
            if (Gloves != null)
            {
                list.AddRange(Gloves);
            }
            if (Boots != null)
            {
                list.AddRange(Boots);
            }
            if (Shields != null)
            {
                list.AddRange(Shields);
            }

            return list;
        }
    }
}