using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ActionFailed : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x25);
        }
    }
}