using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class RestartResponse : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x5f);
            WriteInt(0x01);
        }
    }
}