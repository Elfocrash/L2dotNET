using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunRise : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x1c);
        }
    }
}