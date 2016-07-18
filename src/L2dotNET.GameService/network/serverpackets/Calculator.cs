using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class Calculator : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xe2);
            WriteInt(4393);
        }
    }
}