namespace L2dotNET.GameService.network.serverpackets
{
    class PartySmallWindowDeleteAll : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x50);
        }
    }
}