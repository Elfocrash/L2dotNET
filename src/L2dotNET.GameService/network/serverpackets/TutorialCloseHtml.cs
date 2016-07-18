using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialCloseHtml : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xa3);
        }
    }
}