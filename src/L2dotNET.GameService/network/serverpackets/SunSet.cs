using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunSet : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x1d);
        }
    }
}