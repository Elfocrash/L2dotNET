namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowMemberListDeleteAll : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x88);
        }
    }
}