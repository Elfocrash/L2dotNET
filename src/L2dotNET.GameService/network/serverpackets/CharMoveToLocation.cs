using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharMoveToLocation : GameServerNetworkPacket
    {
        private readonly L2Object _obj;

        public CharMoveToLocation(L2Object obj)
        {
            _obj = obj;
        }

        protected internal override void Write()
        {
            WriteC(0x01);

            WriteD(_obj.ObjId);

            WriteD(_obj.DestX);
            WriteD(_obj.DestY);
            WriteD(_obj.DestZ);

            WriteD(_obj.X);
            WriteD(_obj.Y);
            WriteD(_obj.Z);
        }
    }
}