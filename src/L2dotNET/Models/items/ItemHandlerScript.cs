using System.Collections.Generic;
using L2dotNET.Models.Player;
using NLog;

namespace L2dotNET.Models.Items
{
    class ItemHandlerScript : ItemEffect
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private int Id { get; };

        public bool Pet { get; set; }
        public bool Player { get; set; } = true;
        public bool Destroy { get; set; } = true;

        public int? PetId { get; set; }
        public int? SummonId { get; set; }
        public int? SummonStaticId { get; set; }

        public int EffectId { get; set; }
        public int EffectLvl { get; set; }
        public int SkillId { get; set; }
        public int SkillLvl { get; set; }

        private readonly Dictionary<int, int> _exchangeItems;


        public ItemHandlerScript(int id)
        {
            Id = id;
            _exchangeItems = new Dictionary<int, int>();
        }

        public void AddExchangeItem(int itemId, int count)
        {
            _exchangeItems.Add(itemId, count);
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            // TODO: Debug this code
            if (!Player)
            {
                base.UsePlayer(player, item);
                return;
            }

            if (Destroy)
            {
                player.DestroyItem(item, 1);
            }

            if (_exchangeItems != null)
            {
                foreach (int val in _exchangeItems.Keys)
                    player.AddItem(val, _exchangeItems[val]);
            }

            if (PetId.HasValue)
            {
                player.PetSummon(item, PetId.Value);
            }

            if (SummonId.HasValue)
            {
                player.PetSummon(item, SummonId.Value, false);
            }

            if (SummonStaticId.HasValue)
            {
                //NpcTable.Instance.SpawnNpc(SummonStaticID.Value, player.X, player.Y, player.Z, player.Heading);
            }
        }
        
    }
}