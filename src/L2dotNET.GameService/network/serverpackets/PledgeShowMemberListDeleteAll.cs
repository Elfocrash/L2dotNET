namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowMemberListDeleteAll : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0x88);
        }
    }
}