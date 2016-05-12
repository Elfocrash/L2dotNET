
namespace L2dotNET.GameService.network.l2send
{
    class TutorialCloseHtml : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xa3);
        }
    }
}
