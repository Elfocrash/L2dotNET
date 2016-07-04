namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialCloseHtml : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xa3);
        }
    }
}