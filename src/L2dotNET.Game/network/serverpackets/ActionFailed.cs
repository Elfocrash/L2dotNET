namespace L2dotNET.GameService.network.l2send
{
    class ActionFailed : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x25);
        }
    }
}