using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
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
            writeC(0x2f);

            writeD(_obj.ObjID);

            writeD(_obj.destx);
            writeD(_obj.desty);
            writeD(_obj.destz);

            writeD(_obj.X);
            writeD(_obj.Y);
            writeD(_obj.Z);
        }
    }
}
