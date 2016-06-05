using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.serverpackets
{
    class ExStorageMaxCount : GameServerNetworkPacket
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
            _inventory = player.ItemLimit_Inventory;
            _warehouse = player.ItemLimit_Warehouse;
            _clan = player.ItemLimit_ClanWarehouse;
            _privateSell = player.ItemLimit_Selling;
            _privateBuy = player.ItemLimit_Buying;
            _receipeD = player.ItemLimit_RecipeDwarven;
            _recipe = player.ItemLimit_Warehouse;
            //_extra = player.ItemLimit_Extra;
            //_quest = player.ItemLimit_Quest;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x2e);

            writeD(_inventory);
            writeD(_warehouse);
            writeD(_clan);
            writeD(_privateSell);
            writeD(_privateBuy);
            writeD(_receipeD);
            writeD(_recipe);
            //writeD(_extra);
            //writeD(_quest);
        }
    }
}