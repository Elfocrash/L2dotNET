namespace L2dotNET.GameService.network.serverpackets
{
    class CharDeleteOk : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x23);
        }
    }
}