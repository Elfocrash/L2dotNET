namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharCreateOk : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x19);
            WriteInt(0x01);
        }
    }
}