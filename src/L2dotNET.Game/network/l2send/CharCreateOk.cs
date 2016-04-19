
namespace L2dotNET.Game.network.l2send
{
    class CharCreateOk : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x0f);
            writeD(0x01);
        }
    }
}
