namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialCloseHtml : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0xa3);
        }
    }
}