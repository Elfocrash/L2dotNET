namespace L2dotNET.GameService.Network.Serverpackets
{
    class LeaveWorld : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x7e);
        }
    }
}