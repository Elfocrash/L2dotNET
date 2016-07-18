using L2dotNET.GameService.Controllers;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ClientSetTime : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xEC);
            WriteInt(GameTime.Instance.Time);
            WriteInt(6);
        }
    }
}