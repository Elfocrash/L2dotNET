using L2dotNET.model.player;

namespace L2dotNET.Network.serverpackets
{
    class ExStorageMaxCount : GameserverPacket
    {
        private readonly int _inventory;
        private readonly int _warehouse;
        private readonly int _clan;
        private readonly int _privateSell;
        private readonly int _privateBuy;
        private readonly int _receipeD;
        private readonly int _recipe;
        //private int _extra;
        //private int _quest;

        public ExStorageMaxCount(L2Player player)
        {
            _inventory = player.ItemLimitInventory;
            _warehouse = player.ItemLimitWarehouse;
            _clan = player.ItemLimitClanWarehouse;
            _privateSell = player.ItemLimitSelling;
            _privateBuy = player.ItemLimitBuying;
            _receipeD = player.ItemLimitRecipeDwarven;
            _recipe = player.ItemLimitWarehouse;
            //_extra = player.ItemLimit_Extra;
            //_quest = player.ItemLimit_Quest;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x2e);

            WriteInt(_inventory);
            WriteInt(_warehouse);
            WriteInt(_clan);
            WriteInt(_privateSell);
            WriteInt(_privateBuy);
            WriteInt(_receipeD);
            WriteInt(_recipe);
            //writeD(_extra);
            //writeD(_quest);
        }
    }
}