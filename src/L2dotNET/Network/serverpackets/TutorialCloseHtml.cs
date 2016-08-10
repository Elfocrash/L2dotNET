namespace L2dotNET.Network.serverpackets
{
    class TutorialCloseHtml : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xa3);
        }
    }
}