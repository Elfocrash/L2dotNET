using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    class CharMoveToLocation : GameServerNetworkPacket
    {
        private L2Object _obj;
        public CharMoveToLocation(L2Object obj)
        {
            _obj = obj;
        }

        protected internal override void write()
        {
            writeC(0x01);

            writeD(_obj.ObjID);

            writeD(_obj.DestX);
            writeD(_obj.DestY);
            writeD(_obj.DestZ);

            writeD(_obj.X);
            writeD(_obj.Y);
            writeD(_obj.Z);
        }
    }
}
