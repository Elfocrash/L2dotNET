using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharFlyToLocation : GameServerNetworkPacket
    {
        private readonly L2Object obj;
        private readonly int id;

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
}