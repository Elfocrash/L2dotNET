using System.Collections.Generic;
using System.Linq;
using L2dotNET.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Managers
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

            _items.Add(729, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, CrystalTypeId.A));
            _items.Add(730, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, CrystalTypeId.A));
            _items.Add(947, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, CrystalTypeId.B));
            _items.Add(948, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, CrystalTypeId.B));
            _items.Add(951, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, CrystalTypeId.C));
            _items.Add(952, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, CrystalTypeId.C));
            _items.Add(955, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, CrystalTypeId.D));
            _items.Add(956, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, CrystalTypeId.D));
            _items.Add(959, new EnchantScroll(EnchantType.Standart, EnchantTarget.Weapon, CrystalTypeId.S));
            _items.Add(960, new EnchantScroll(EnchantType.Standart, EnchantTarget.Armor, CrystalTypeId.S));

            _items.Add(731, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, CrystalTypeId.A));
            _items.Add(732, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, CrystalTypeId.A));
            _items.Add(949, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, CrystalTypeId.B));
            _items.Add(950, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, CrystalTypeId.B));
            _items.Add(953, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, CrystalTypeId.C));
            _items.Add(954, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, CrystalTypeId.C));
            _items.Add(957, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, CrystalTypeId.D));
            _items.Add(958, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, CrystalTypeId.D));
            _items.Add(961, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Weapon, CrystalTypeId.S));
            _items.Add(962, new EnchantScroll(EnchantType.Crystal, EnchantTarget.Armor, CrystalTypeId.S));

            _items.Add(6569, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, CrystalTypeId.A));
            _items.Add(6570, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, CrystalTypeId.A));
            _items.Add(6571, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, CrystalTypeId.B));
            _items.Add(6572, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, CrystalTypeId.B));
            _items.Add(6573, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, CrystalTypeId.C));
            _items.Add(6574, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, CrystalTypeId.C));
            _items.Add(6575, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, CrystalTypeId.D));
            _items.Add(6576, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, CrystalTypeId.D));
            _items.Add(6577, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Weapon, CrystalTypeId.S));
            _items.Add(6578, new EnchantScroll(EnchantType.Blessed, EnchantTarget.Armor, CrystalTypeId.S));
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
                case (CrystalTypeId)1:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.D;
                    break;
                case (CrystalTypeId)2:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.C;
                    break;
                case (CrystalTypeId)3:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.B;
                    break;
                case (CrystalTypeId)4:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.A;
                    break;
                case (CrystalTypeId)5:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.S;
                    break;
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
                case (CrystalTypeId)1:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.D;
                    break;
                case (CrystalTypeId)2:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.C;
                    break;
                case (CrystalTypeId)3:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.B;
                    break;
                case (CrystalTypeId)4:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.A;
                    break;
                case (CrystalTypeId)5:
                    next = player.EnchantItem.Template.CrystalType == CrystalType.S;
                    break;
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