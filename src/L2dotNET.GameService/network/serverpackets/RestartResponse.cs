namespace L2dotNET.GameService.Network.Serverpackets
{
    class RestartResponse : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x5f);
            WriteInt(0x01);
        }
    }
}