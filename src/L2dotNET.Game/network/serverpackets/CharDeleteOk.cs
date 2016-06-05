namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharDeleteOk : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x23);
        }
    }
}