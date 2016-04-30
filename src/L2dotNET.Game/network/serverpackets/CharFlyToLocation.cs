using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
{
    class CharFlyToLocation : GameServerNetworkPacket
    {
        private L2Object obj;
        private int id;
        public CharFlyToLocation(L2Object obj, FlyType type)
        {
            this.obj = obj;
            this.id = (int)type;
        }

        protected internal override void write()
        {
            writeC(0xC5);

            writeD(obj.ObjID);

            writeD(obj.DestX);
            writeD(obj.DestY);
            writeD(obj.DestZ);

            writeD(obj.X);
            writeD(obj.Y);
            writeD(obj.Z);

            writeD(id);
        }
    }

    public enum FlyType
	{
        THROW_UP,
        THROW_HORIZONTAL,
        DUMMY
    }
}
