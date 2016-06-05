namespace L2dotNET.GameService.network.serverpackets
{
    class CharCreateOk : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x19);
            writeD(0x01);
        }
    }
}