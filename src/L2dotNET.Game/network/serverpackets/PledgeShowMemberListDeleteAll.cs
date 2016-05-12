
namespace L2dotNET.GameService.network.l2send
{
    class PledgeShowMemberListDeleteAll : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x88);
        }
    }
}
