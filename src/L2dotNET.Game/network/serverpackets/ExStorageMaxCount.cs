
namespace L2dotNET.GameService.network.l2send
{
    class ExStorageMaxCount : GameServerNetworkPacket
    {
        private int _inventory;
        private int _warehouse;
        private int _clan;
        private int _privateSell;
        private int _privateBuy;
        private int _receipeD;
        private int _recipe;
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
