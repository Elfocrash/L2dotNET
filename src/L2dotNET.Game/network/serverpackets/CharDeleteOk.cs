namespace L2dotNET.GameService.network.l2send
{
    class CharDeleteOk : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x23);
        }
    }
}