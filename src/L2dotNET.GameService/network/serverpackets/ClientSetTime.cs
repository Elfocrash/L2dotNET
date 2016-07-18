using L2dotNET.GameService.Controllers;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ClientSetTime : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0xEC);
            WriteInt(GameTime.Instance.Time);
            WriteInt(6);
        }
    }
}