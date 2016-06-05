namespace L2dotNET.GameService.network.serverpackets
{
    class TutorialCloseHtml : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xa3);
        }
    }
}