namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExCursedWeaponList : GameServerNetworkPacket
    {
        private readonly int[] _ids;

        public ExCursedWeaponList(int[] ids)
        {
            _ids = ids;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x46);
            WriteD(_ids.Length);

            foreach (int id in _ids)
            {
                WriteD(id);
            }
        }
    }
}