using System.Collections.Generic;
using L2dotNET.Models.Player;
using NLog;

namespace L2dotNET.Models.Items
{
    class ItemHandlerScript : ItemEffect
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly int _id;

        public int EffectId = -1;
        public int EffectLv;

        public bool Pet = false;
        public bool Player = true;
        public bool Destroy = true;

        public int PetId = -1;
        public int SummonId = -1;
        public int SummonStaticId = -1;

        private SortedList<int, int> _exchangeItems;
        public int SkillId;
        public int SkillLv;

        public ItemHandlerScript(int id)
        {
            _id = id;
        }

        public void AddExchangeItem(int itemId, int count)
        {
            if (_exchangeItems == null)
                _exchangeItems = new SortedList<int, int>();

            _exchangeItems.Add(itemId, count);
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (!Player)
            {
                base.UsePlayer(player, item);
                return;
            }

            if (Destroy)
                player.DestroyItem(item, 1);

            if (_exchangeItems != null)
            {
                foreach (int val in _exchangeItems.Keys)
                    player.AddItem(val, _exchangeItems[val]);
            }

            if (PetId != -1)
                player.PetSummon(item, PetId);

            if (SummonId != -1)
                player.PetSummon(item, SummonId, false);

            if (SummonStaticId != -1)
            {
                //NpcTable.Instance.SpawnNpc(SummonStaticID, player.X, player.Y, player.Z, player.Heading);
            }
        }
        
    }
}