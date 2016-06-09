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
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemSetTemplate));

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

        public int set1Id,
                   set1Lvl;

        public void set1(int p, int p2)
        {
            set1Id = p;
            set1Lvl = p2;
        }

        public int set2Id,
                   set2Lvl;

        public void set2(int p, int p2)
        {
            set2Id = p;
            set2Lvl = p2;
        }

        public int set3Id,
                   set3Lvl;

        public void set3(int p, int p2)
        {
            set3Id = p;
            set3Lvl = p2;
        }

        public void Validate(L2Player owner)
        {
            byte set1sum = 0,
                 set2sum = 0,
                 set3sum = 0;
            foreach (L2Item item in owner.Inventory.Items.Values.Where(item => (item._isEquipped != 0) && (item.Template.Type == ItemTemplate.L2ItemType.armor)))
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
                        }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.legs:
                    {
                        if (legs != null)
                            if (legs.Any(id => id == item.Template.ItemID))
                            {
                                set1sum++;
                                if (item.Enchant >= 6)
                                    set3sum++;
                            }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.head:
                    {
                        if (helms != null)
                            if (helms.Any(id => id == item.Template.ItemID))
                            {
                                set1sum++;
                                if (item.Enchant >= 6)
                                    set3sum++;
                            }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.gloves:
                    {
                        if (gloves != null)
                            if (gloves.Any(id => id == item.Template.ItemID))
                            {
                                set1sum++;
                                if (item.Enchant >= 6)
                                    set3sum++;
                            }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.feet:
                    {
                        if (boots != null)
                            if (boots.Any(id => id == item.Template.ItemID))
                            {
                                set1sum++;
                                if (item.Enchant >= 6)
                                    set3sum++;
                            }
                    }
                        break;
                    case ItemTemplate.L2ItemBodypart.lhand:
                    {
                        if (shields != null)
                            if (shields.Any(id => id == item.Template.ItemID))
                                set2sum = 1;
                    }
                        break;
                }

            byte cnt = count();
            log.Info($"set validation: cnt {cnt}, s1 {set1sum}, s2 {set2sum}, s3 {set3sum}");

            if (cnt == set1sum) // весь сет
            {
                owner.addSkill(TSkillTable.Instance.Get(set1Id, set1Lvl), false, false);

                if (set2sum == 1) //со щитом
                    owner.addSkill(TSkillTable.Instance.Get(set2Id, set2Lvl), false, false);

                if (set3sum == cnt) //весь сет +6
                    owner.addSkill(TSkillTable.Instance.Get(set3Id, set3Lvl), false, false);

                owner.updateSkillList();
            }
        }

        private byte count()
        {
            byte s = 1;
            if (legs != null)
                s++;
            if (helms != null)
                s++;
            if (gloves != null)
                s++;
            if (boots != null)
                s++;
            return s;
        }

        public List<int> getAllSetIds()
        {
            List<int> list = new List<int>();
            list.Add(armorId);

            if (legs != null)
                list.AddRange(legs);
            if (helms != null)
                list.AddRange(helms);
            if (gloves != null)
                list.AddRange(gloves);
            if (boots != null)
                list.AddRange(boots);
            if (shields != null)
                list.AddRange(shields);

            return list;
        }
    }
}