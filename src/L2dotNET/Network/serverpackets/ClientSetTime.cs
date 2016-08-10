using L2dotNET.controllers;

namespace L2dotNET.Network.serverpackets
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