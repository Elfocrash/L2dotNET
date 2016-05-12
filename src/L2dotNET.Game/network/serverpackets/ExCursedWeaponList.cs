
namespace L2dotNET.GameService.network.l2send
{
    class ExCursedWeaponList : GameServerNetworkPacket
    {
        private int[] ids;

        public ExCursedWeaponList(int[] ids) {
            this.ids = ids;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x46);
            writeD(ids.Length);

            foreach (int id in ids)
                writeD(id);
        }
    }
}
