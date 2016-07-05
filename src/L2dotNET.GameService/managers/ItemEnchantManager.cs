using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Managers
{
    public class ItemEnchantManager
    {
        private static readonly ItemEnchantManager Instance = new ItemEnchantManager();

        public static ItemEnchantManager GetInstance()
        {
            return Instance;
        }

        private readonly SortedList<int, EnchantScroll> _items;
        private readonly SortedList<int, EnchantScroll> _supports;

        public ItemEnchantManager()
        {
            _items = new SortedList<int, EnchantScroll>();
            _supports = new SortedList<int, EnchantScroll>();

            _items.Add(729, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A));
            _items.Add(730, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A));
            _items.Add(947, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B));
            _items.Add(948, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B));
            _items.Add(951, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C));
            _items.Add(952, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C));
            _items.Add(955, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D));
            _items.Add(956, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D));
            _items.Add(959, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S));
            _items.Add(960, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S));

            _items.Add(731, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A));
            _items.Add(732, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A));
            _items.Add(949, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B));
            _items.Add(950, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B));
            _items.Add(953, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C));
            _items.Add(954, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C));
            _items.Add(957, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D));
            _items.Add(958, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D));
            _items.Add(961, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S));
            _items.Add(962, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S));

            _items.Add(6569, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A));
            _items.Add(6570, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A));
            _items.Add(6571, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B));
            _items.Add(6572, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B));
            _items.Add(6573, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C));
            _items.Add(6574, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C));
            _items.Add(6575, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D));
            _items.Add(6576, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D));
            _items.Add(6577, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S));
            _items.Add(6578, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S));

            _items.Add(13540, new EnchantScroll(EnchantType.Blessed, EnchantTarget.YogiStaff, ItemTemplate.L2ItemGrade.None));

            _items.Add(22006, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D, 10));
            _items.Add(22007, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C, 10));
            _items.Add(22008, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 10));
            _items.Add(22009, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 10));
            _items.Add(22010, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D, 10));
            _items.Add(22011, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C, 10));
            _items.Add(22012, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B, 10));
            _items.Add(22013, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A, 10));
            _items.Add(20517, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 10));
            _items.Add(20518, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S, 10));

            _items.Add(22014, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 10));
            _items.Add(22015, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 10));
            _items.Add(22016, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B, 10));
            _items.Add(22017, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A, 10));
            _items.Add(20519, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 10));
            _items.Add(20520, new EnchantScroll(EnchantType.Ancient, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S, 10));

            _items.Add(22018, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 100));
            _items.Add(22019, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 100));
            _items.Add(22020, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B, 100));
            _items.Add(22021, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A, 100));
            _items.Add(20521, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 100));
            _items.Add(20522, new EnchantScroll(EnchantType.Bonus, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S, 100));

            _supports.Add(14702, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D, 20));
            _supports.Add(14703, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C, 18));
            _supports.Add(14704, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 15));
            _supports.Add(14705, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 12));
            _supports.Add(14706, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 10));
            _supports.Add(14707, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D, 35));
            _supports.Add(14708, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C, 27));
            _supports.Add(14709, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B, 23));
            _supports.Add(14710, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A, 18));
            _supports.Add(14711, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S, 15));

            _supports.Add(12362, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.D, 20));
            _supports.Add(12363, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.C, 18));
            _supports.Add(12364, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 15));
            _supports.Add(12365, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 12));
            _supports.Add(12366, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 10));
            _supports.Add(12367, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.D, 35));
            _supports.Add(12368, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.C, 27));
            _supports.Add(12369, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.B, 23));
            _supports.Add(12370, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.A, 18));
            _supports.Add(12371, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Armor, ItemTemplate.L2ItemGrade.S, 15));

            _supports.Add(20978, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.B, 15));
            _supports.Add(20979, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.A, 12));
            _supports.Add(20980, new EnchantScroll(EnchantType.Auxiliary, EnchantTarget.Weapon, ItemTemplate.L2ItemGrade.S, 10));
        }

        public const byte StatePutItem = 1;
        public const byte StateEnchantStart = 2;
        public const byte StateEnchantFinished = 3;

        public EnchantScroll GetScroll(int id)
        {
            return _items[id];
        }

        public EnchantScroll GetSupport(int id)
        {
            return _supports.ContainsKey(id) ? _supports[id] : null;
        }

        public int[] GetIds()
        {
            return _items.Keys.ToArray();
        }

        public void TryPutItem(L2Player player, L2Item item)
        {
            L2Item scroll = player.EnchantScroll;
            EnchantScroll dat = _items[scroll.Template.ItemId];

            bool next = false;
            switch (dat.Crystall)
            {
                case ItemTemplate.L2ItemGrade.D:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.D;
                    break;
                case ItemTemplate.L2ItemGrade.C:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.C;
                    break;
                case ItemTemplate.L2ItemGrade.B:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.B;
                    break;
                case ItemTemplate.L2ItemGrade.A:
                    next = item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.A;
                    break;
                case ItemTemplate.L2ItemGrade.S:
                    next = (item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S) || (item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S80) || (item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S84) || (item.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S86);
                    break;
            }

            if (next)
            {
                switch (dat.Target)
                {
                    case EnchantTarget.Weapon:
                        next = item.Template.Type == ItemTemplate.L2ItemType.Weapon;
                        break;
                    case EnchantTarget.Armor:
                        next = (item.Template.Type == ItemTemplate.L2ItemType.Armor) || (item.Template.Type == ItemTemplate.L2ItemType.Accessary);
                        break;
                    case EnchantTarget.YogiStaff:
                        next = item.Template.ItemId == 13539; //Staff of Master Yogi
                        break;
                }
            }

            if (!next)
            {
                player.SendPacket(new ExPutEnchantTargetItemResult());
                player.EnchantScroll = null;
                player.EnchantState = 0;
                player.SendSystemMessage(SystemMessage.SystemMessageId.DoesNotFitScrollConditions);
            }
            else
            {
                player.EnchantState = StateEnchantStart;
                player.EnchantItem = item;
                player.SendPacket(new ExPutEnchantTargetItemResult(item.ObjId));
            }
        }

        public void TryPutStone(L2Player player, L2Item stone)
        {
            if (!_supports.ContainsKey(stone.Template.ItemId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.IncorrectSupportEnhancementSpellbook);
                player.SendActionFailed();
                return;
            }

            EnchantScroll dat = _supports[stone.Template.ItemId];

            bool next = false;
            switch (dat.Crystall)
            {
                case ItemTemplate.L2ItemGrade.D:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.D;
                    break;
                case ItemTemplate.L2ItemGrade.C:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.C;
                    break;
                case ItemTemplate.L2ItemGrade.B:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.B;
                    break;
                case ItemTemplate.L2ItemGrade.A:
                    next = player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.A;
                    break;
                case ItemTemplate.L2ItemGrade.S:
                    next = (player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S) || (player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S80) || (player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S84) || (player.EnchantItem.Template.CrystallGrade == ItemTemplate.L2ItemGrade.S86);
                    break;
            }

            if (next)
            {
                switch (dat.Target)
                {
                    case EnchantTarget.Weapon:
                        next = player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.Weapon;
                        break;
                    case EnchantTarget.Armor:
                        next = (player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.Armor) || (player.EnchantItem.Template.Type == ItemTemplate.L2ItemType.Accessary);
                        break;
                }
            }

            if (next)
            {
                byte min = 0,
                     max = 0;
                switch (dat.Type)
                {
                    case EnchantType.Auxiliary:
                        min = player.EnchantItem.Template.Bodypart == ItemTemplate.L2ItemBodypart.Onepiece ? (byte)4 : (byte)3;
                        max = 9;
                        break;
                    case EnchantType.Ancient:
                        max = 16;
                        break;
                }

                next = (player.EnchantItem.Enchant >= min) && (player.EnchantItem.Enchant <= max);
            }

            if (!next)
            {
                player.SendPacket(new ExPutEnchantSupportItemResult());
                player.EnchantStone = null;
                player.SendSystemMessage(SystemMessage.SystemMessageId.ItemDoesNotMeetRequirementsForSupportEnhancementSpellbook);
            }
            else
            {
                player.EnchantStone = stone;
                player.SendPacket(new ExPutEnchantSupportItemResult(stone.ObjId));
            }
        }
    }
}