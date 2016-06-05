using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Managers
{
    public class ItemEnchantManager
    {
        private static readonly ItemEnchantManager instance = new ItemEnchantManager();

        public static ItemEnchantManager getInstance()
        {
            return instance;
        }

        private readonly SortedList<int, EnchantScroll> items;
        private readonly SortedList<int, EnchantScroll> supports;

        public ItemEnchantManager()
        {
            items = new SortedList<int, EnchantScroll>();
            supports = new SortedList<int, EnchantScroll>();

            items.Add(729, new EnchantScroll(EnchantType.standart, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a));
            items.Add(730, new EnchantScroll(EnchantType.standart, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a));
            items.Add(947, new EnchantScroll(EnchantType.standart, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b));
            items.Add(948, new EnchantScroll(EnchantType.standart, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b));
            items.Add(951, new EnchantScroll(EnchantType.standart, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c));
            items.Add(952, new EnchantScroll(EnchantType.standart, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c));
            items.Add(955, new EnchantScroll(EnchantType.standart, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d));
            items.Add(956, new EnchantScroll(EnchantType.standart, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d));
            items.Add(959, new EnchantScroll(EnchantType.standart, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s));
            items.Add(960, new EnchantScroll(EnchantType.standart, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s));

            items.Add(731, new EnchantScroll(EnchantType.crystal, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a));
            items.Add(732, new EnchantScroll(EnchantType.crystal, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a));
            items.Add(949, new EnchantScroll(EnchantType.crystal, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b));
            items.Add(950, new EnchantScroll(EnchantType.crystal, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b));
            items.Add(953, new EnchantScroll(EnchantType.crystal, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c));
            items.Add(954, new EnchantScroll(EnchantType.crystal, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c));
            items.Add(957, new EnchantScroll(EnchantType.crystal, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d));
            items.Add(958, new EnchantScroll(EnchantType.crystal, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d));
            items.Add(961, new EnchantScroll(EnchantType.crystal, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s));
            items.Add(962, new EnchantScroll(EnchantType.crystal, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s));

            items.Add(6569, new EnchantScroll(EnchantType.blessed, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a));
            items.Add(6570, new EnchantScroll(EnchantType.blessed, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a));
            items.Add(6571, new EnchantScroll(EnchantType.blessed, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b));
            items.Add(6572, new EnchantScroll(EnchantType.blessed, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b));
            items.Add(6573, new EnchantScroll(EnchantType.blessed, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c));
            items.Add(6574, new EnchantScroll(EnchantType.blessed, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c));
            items.Add(6575, new EnchantScroll(EnchantType.blessed, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d));
            items.Add(6576, new EnchantScroll(EnchantType.blessed, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d));
            items.Add(6577, new EnchantScroll(EnchantType.blessed, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s));
            items.Add(6578, new EnchantScroll(EnchantType.blessed, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s));

            items.Add(13540, new EnchantScroll(EnchantType.blessed, EnchantTarget.yogi_staff, ItemTemplate.L2ItemGrade.none));

            items.Add(22006, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d, 10));
            items.Add(22007, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c, 10));
            items.Add(22008, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 10));
            items.Add(22009, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 10));
            items.Add(22010, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d, 10));
            items.Add(22011, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c, 10));
            items.Add(22012, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b, 10));
            items.Add(22013, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a, 10));
            items.Add(20517, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 10));
            items.Add(20518, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s, 10));

            items.Add(22014, new EnchantScroll(EnchantType.ancient, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 10));
            items.Add(22015, new EnchantScroll(EnchantType.ancient, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 10));
            items.Add(22016, new EnchantScroll(EnchantType.ancient, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b, 10));
            items.Add(22017, new EnchantScroll(EnchantType.ancient, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a, 10));
            items.Add(20519, new EnchantScroll(EnchantType.ancient, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 10));
            items.Add(20520, new EnchantScroll(EnchantType.ancient, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s, 10));

            items.Add(22018, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 100));
            items.Add(22019, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 100));
            items.Add(22020, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b, 100));
            items.Add(22021, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a, 100));
            items.Add(20521, new EnchantScroll(EnchantType.bonus, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 100));
            items.Add(20522, new EnchantScroll(EnchantType.bonus, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s, 100));

            supports.Add(14702, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d, 20));
            supports.Add(14703, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c, 18));
            supports.Add(14704, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 15));
            supports.Add(14705, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 12));
            supports.Add(14706, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 10));
            supports.Add(14707, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d, 35));
            supports.Add(14708, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c, 27));
            supports.Add(14709, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b, 23));
            supports.Add(14710, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a, 18));
            supports.Add(14711, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s, 15));

            supports.Add(12362, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.d, 20));
            supports.Add(12363, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.c, 18));
            supports.Add(12364, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 15));
            supports.Add(12365, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 12));
            supports.Add(12366, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 10));
            supports.Add(12367, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.d, 35));
            supports.Add(12368, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.c, 27));
            supports.Add(12369, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.b, 23));
            supports.Add(12370, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.a, 18));
            supports.Add(12371, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.armor, ItemTemplate.L2ItemGrade.s, 15));

            supports.Add(20978, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.b, 15));
            supports.Add(20979, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.a, 12));
            supports.Add(20980, new EnchantScroll(EnchantType.auxiliary, EnchantTarget.weapon, ItemTemplate.L2ItemGrade.s, 10));
        }

        public const byte STATE_PUT_ITEM = 1;
        public const byte STATE_ENCHANT_START = 2;
        public const byte STATE_ENCHANT_FINISHED = 3;

        public EnchantScroll getScroll(int id)
        {
            return items[id];
        }

        public EnchantScroll getSupport(int id)
        {
            if (supports.ContainsKey(id))
                return supports[id];
            else
                return null;
        }

        public int[] getIds()
        {
            return items.Keys.ToArray();
        }

        public void tryPutItem(L2Player player, L2Item item)
        {
            L2Item scroll = player.EnchantScroll;
            EnchantScroll dat = items[scroll.Template.ItemID];

            bool next = false;
            switch (dat.Crystall)
            {
                case ItemTemplate.L2ItemGrade.d:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.d;
                    break;
                case ItemTemplate.L2ItemGrade.c:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.c;
                    break;
                case ItemTemplate.L2ItemGrade.b:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.b;
                    break;
                case ItemTemplate.L2ItemGrade.a:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.a;
                    break;
                case ItemTemplate.L2ItemGrade.s:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s || item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s80 || item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s84 || item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s86;
                    break;
            }

            if (next)
            {
                switch (dat.Target)
                {
                    case EnchantTarget.weapon:
                        next = item.Template.Type == ItemTemplate.L2ItemType.weapon;
                        break;
                    case EnchantTarget.armor:
                        next = item.Template.Type == ItemTemplate.L2ItemType.armor || item.Template.Type == ItemTemplate.L2ItemType.accessary;
                        break;
                    case EnchantTarget.yogi_staff:
                        next = item.Template.ItemID == 13539; //Staff of Master Yogi
                        break;
                }
            }

            if (!next)
            {
                player.sendPacket(new ExPutEnchantTargetItemResult());
                player.EnchantScroll = null;
                player.EnchantState = 0;
                player.sendSystemMessage(SystemMessage.SystemMessageId.DOES_NOT_FIT_SCROLL_CONDITIONS);
            }
            else
            {
                player.EnchantState = STATE_ENCHANT_START;
                player.EnchantItem = item;
                player.sendPacket(new ExPutEnchantTargetItemResult(item.ObjID));
            }
        }

        public void tryPutStone(L2Player player, L2Item stone)
        {
            if (!supports.ContainsKey(stone.Template.ItemID))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.INCORRECT_SUPPORT_ENHANCEMENT_SPELLBOOK);
                player.sendActionFailed();
                return;
            }

            EnchantScroll dat = supports[stone.Template.ItemID];

            bool next = false;
            switch (dat.Crystall)
            {
                case ItemTemplate.L2ItemGrade.d:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.d;
                    break;
                case ItemTemplate.L2ItemGrade.c:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.c;
                    break;
                case ItemTemplate.L2ItemGrade.b:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.b;
                    break;
                case ItemTemplate.L2ItemGrade.a:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.a;
                    break;
                case ItemTemplate.L2ItemGrade.s:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s || player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s80 || player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s84 || player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.s86;
                    break;
            }

            if (next)
            {
                switch (dat.Target)
                {
                    case EnchantTarget.weapon:
                        next = player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.weapon;
                        break;
                    case EnchantTarget.armor:
                        next = player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.armor || player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.accessary;
                        break;
                }
            }

            if (next)
            {
                byte min = 0,
                     max = 0;
                switch (dat.Type)
                {
                    case EnchantType.auxiliary:
                        min = player.EnchantItem.Template.Bodypart == ItemTemplate.L2ItemBodypart.onepiece ? (byte)4 : (byte)3;
                        max = 9;
                        break;
                    case EnchantType.ancient:
                        max = 16;
                        break;
                }

                next = player.EnchantItem.Enchant >= min && player.EnchantItem.Enchant <= max;
            }

            if (!next)
            {
                player.sendPacket(new ExPutEnchantSupportItemResult());
                player.EnchantStone = null;
                player.sendSystemMessage(SystemMessage.SystemMessageId.ITEM_DOES_NOT_MEET_REQUIREMENTS_FOR_SUPPORT_ENHANCEMENT_SPELLBOOK);
            }
            else
            {
                player.EnchantStone = stone;
                player.sendPacket(new ExPutEnchantSupportItemResult(stone.ObjID));
            }
        }
    }
}