using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class FriendList : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xfa);
            WriteShort(0x00);
            WriteShort(0x00);
        }
    }
}