using L2dotNET.Controllers;

namespace L2dotNET.Network.serverpackets
{
    class ClientSetTime : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xEC);
            WriteInt(GameTime.IngameTime);
            WriteInt(6);
        }
    }
}