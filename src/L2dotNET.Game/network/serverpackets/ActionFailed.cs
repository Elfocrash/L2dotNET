namespace L2dotNET.GameService.Network.Serverpackets
{
    class ActionFailed : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x25);
        }
    }
}