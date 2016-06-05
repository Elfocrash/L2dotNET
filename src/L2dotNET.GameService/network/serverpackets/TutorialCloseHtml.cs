namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialCloseHtml : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xa3);
        }
    }
}