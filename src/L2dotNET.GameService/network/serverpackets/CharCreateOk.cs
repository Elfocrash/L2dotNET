using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharCreateOk : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x19);
            WriteInt(0x01);
        }
    }
}