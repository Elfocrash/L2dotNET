namespace L2dotNET.Network.serverpackets
{
    class ActionFailed : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x25);
        }
    }
}