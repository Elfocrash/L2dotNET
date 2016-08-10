namespace L2dotNET.Network.serverpackets
{
    class Calculator : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xe2);
            WriteInt(4393);
        }
    }
}