using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExShowOwnthingPos : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x93);

            WriteInt(0);
            WriteInt(0);
        }
    }
}