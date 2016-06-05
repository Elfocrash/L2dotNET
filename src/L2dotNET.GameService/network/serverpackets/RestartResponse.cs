namespace L2dotNET.GameService.Network.Serverpackets
{
    class RestartResponse : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x5f);
            writeD(0x01);
        }
    }
}