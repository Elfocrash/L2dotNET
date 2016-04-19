
namespace L2dotNET.Game.network.l2send
{
    class TutorialCloseHtml : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xa9);
        }
    }
}
