namespace L2dotNET.GameService.network.serverpackets
{
    class LeaveWorld : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x7e);
        }
    }
}