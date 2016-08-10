namespace L2dotNET.Network.serverpackets
{
    class CharDeleteOk : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x23);
        }
    }
}