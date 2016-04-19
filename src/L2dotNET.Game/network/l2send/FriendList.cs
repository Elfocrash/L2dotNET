
namespace L2dotNET.Game.network.l2send
{
    class FriendList : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x75);
            writeH(0x00);
            writeH(0x00);
        }
    }
}
