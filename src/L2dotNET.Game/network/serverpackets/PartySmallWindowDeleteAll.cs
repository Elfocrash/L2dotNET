namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowDeleteAll : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x50);
        }
    }
}